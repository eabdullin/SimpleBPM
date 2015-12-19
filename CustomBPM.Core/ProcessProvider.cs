using System;
using System.Collections.Generic;
using System.Linq;
using CustomBPM.Core.DataModel;
using CustomBPM.Core.Exceptions;
using CustomBPM.Core.Interfaces;
using CustomBPM.Core.Interfaces.Repositories;
using CustomBPM.Core.MetaData;

namespace CustomBPM.Core
{
    public class ProcessProvider : IProcessProvider
    {
        private readonly IProcessInstanceRepository _processInstanceRepository;
        private readonly IAcivityInstanceRepository _acivityInstanceRepository;
        private readonly IProcessActionProvider _processActionProvider;
        private readonly IMetadataProvider _metadataProvider;
        public ProcessProvider(IProcessInstanceRepository processInstanceRepository, IAcivityInstanceRepository acivityInstanceRepository, IProcessActionProvider processActionProvider, IMetadataProvider metadataProvider = null)
        {
            _processInstanceRepository = processInstanceRepository;
            _acivityInstanceRepository = acivityInstanceRepository;
            _processActionProvider = processActionProvider;
            _metadataProvider = metadataProvider ?? new XmlMetadataProvider();
        }

        public Activity GetActivityDescription(string processCode, string acitivityCode)
        {
            Process process =  _metadataProvider.GetProcess(processCode);
            return process.Activities.Find(x => x.Code == acitivityCode);
        }

        public ProcessInstance StartProcess(string key, string[] roles, Dictionary<string, string> parameters, string userId = null)
        {
            Process process = _metadataProvider.GetProcess(key);
            ProcessInstance processInstance = new ProcessInstance()
            {
                Code = process.Code,
                Name = process.Name,
                Status = ProcessStatus.Active,
                StartDate = DateTime.Now
            };
            foreach (var p in parameters)
            {
                processInstance.AddField(p.Key,p.Value);
            }
            Activity activity = process.Activities.First();
            _processInstanceRepository.Create(processInstance);
            GoToStep(processInstance.Id, activity.Code, roles, userId);
            return processInstance;
        }

        public IList<ProcessInstance> GetActiveProcessInstances(string processKey)
        {
            var process = _metadataProvider.GetProcess(processKey);
            return _processInstanceRepository.GetActiveProcessInstances(process.Code);
        }

        public IList<ProcessInstance> GetActiveProcessInstances()
        {
            return _processInstanceRepository.GetAllActiveProcessInstances();
        }

        public ProcessInstance GetActiveProcessInstance(long processId)
        {
            return _processInstanceRepository.FindAsync(x =>
                x.Id == processId
                && x.Status != ProcessStatus.Deleted
                && x.Status != ProcessStatus.Error
                && x.Status != ProcessStatus.Stopped).Result;
        }

        public ProcessInstance GetActiveInstance(long processId)
        {
            return _processInstanceRepository.FindAsync(x => x.Id == processId).Result;
        }

        public bool FinishProcess(long processId)
        {
            var processInstance = GetActiveProcessInstance(processId);
            processInstance.Status = ProcessStatus.Stopped;
            processInstance.CurrentActivity.EndDate = DateTime.Now;
            processInstance.EndDate = DateTime.Now;
            _processInstanceRepository.Update(processInstance);
            return true;
        }

        public bool GoToStep(long processId, string stepName, string[] roles, string userId = null)
        {
            var processInstance = GetActiveProcessInstance(processId);
            if(processInstance == null)
                throw new ProcessNotFoundException();
            if(processInstance.CurrentActivityCode == stepName)
                throw new ProcessAlreadyInStepException();
            var process = _metadataProvider.GetProcess(processInstance.Code);
            var fromActivity = process.Activities.FirstOrDefault(x => x.Code == processInstance.CurrentActivityCode);
            //if (fromActivity == null)
            //{
            //    throw new GoToStepException("Не найден текущий шаг");

            //}
            var toActivity = process.Activities.FirstOrDefault(x => x.Code == stepName);
            if (toActivity == null)
            {
                throw new GoToStepException("Не найден следующий шаг", processInstance.Id, processInstance.Code, null, fromActivity != null ? fromActivity.Code : null, stepName);
            }
            if (fromActivity!= null && !toActivity.Allow(fromActivity, roles))
            {
                throw new GoToStepException("Переход на этот шаг невозможен, возможно нет прав доступа", processInstance.Id, processInstance.Code,null,  fromActivity.Code , stepName);
            }
            
            //конвертировали параметры
            //converts params
            IDictionary<string, string> parameters = processInstance.ConvertFields();
            if(userId != null) parameters.Add("UserId",userId);

            //флаг что мы прошли все проверки успешно
            bool conditionsSuccess = true;
            
            string conditionResult;

            //выполняем выходную проверку в текущем шаге
            //exexcute output conditions on current step
            if (fromActivity != null)
            {
                var outputConditions = fromActivity.Conditions.Where(c => c.Type == ConditionType.Output);
                foreach (var outputCondition in outputConditions)
                {
                    if (!(conditionsSuccess &= _processActionProvider.ExecuteCondition(outputCondition.Key, out conditionResult, parameters)))
                    {
                        throw new GoToStepException(conditionResult, processInstance.Id, processInstance.Code, outputCondition.Key);
                    }
                }

                //выполняем переходную проверку
                //execute transitional condition
                var conditionKey = fromActivity.AllowOutputActivities.First(x => x.Code == toActivity.Code).ConditionCode;
                if (!(conditionsSuccess &= _processActionProvider.ExecuteCondition(conditionKey, out conditionResult, parameters)))
                {
                    throw new GoToStepException(conditionResult, processInstance.Id, processInstance.Code, conditionKey);
                }
            }



            
            //выполняем входную проверку в следующем шаг
            //checking the input condition in next step
            var inputConditions = toActivity.Conditions.Where(c => c.Type == ConditionType.Input);
            foreach (var inputCondition in inputConditions)
            {
                if (!(conditionsSuccess &= _processActionProvider.ExecuteCondition(inputCondition.Key, out conditionResult, parameters)))
                {
                    throw new GoToStepException(conditionResult, processInstance.Id, processInstance.Code, inputCondition.Key);

                }
            }

            //если проверки пройдены
            //if all conditions passed
            if (conditionsSuccess)
            {
                if (fromActivity != null)
                {
                    //выполняем выходное действие
                    //execute the output action
                    if (fromActivity.ActivityActions.Count(x => x.Type == ActionType.Output) > 0)
                    {
                        foreach (var action in fromActivity.ActivityActions.Where(x => x.Type == ActionType.Output))
                        {
                            _processActionProvider.ExecuteAction(action.Code, parameters);
                        }
                    }
                }

                    

                //завершаем текущий шаг
                //end of current step
                if (processInstance.CurrentActivity != null)
                {
                    processInstance.CurrentActivity.EndDate = DateTime.Now;
                    _acivityInstanceRepository.Update(processInstance.CurrentActivity);
                }
                
                

                //создаем следующий шаг
                //create the next step
                ActivityInstance activityInstance = new ActivityInstance()
                {
                    Code = toActivity.Code,
                    ProcessInstance = processInstance,
                    StartDate = DateTime.Now
                };
                processInstance.Activities.Add(activityInstance);
                processInstance.CurrentActivityCode = activityInstance.Code;

                //выполняем входное действие
                //input action
                if (toActivity.ActivityActions.Count(x => x.Type == ActionType.Input) > 0)
                {
                    foreach (ActivityAction action in toActivity.ActivityActions.Where(x => x.Type == ActionType.Input))
                    {
                        _processActionProvider.ExecuteAction(action.Code, parameters);
                    }
                }
                _processInstanceRepository.Update(processInstance);

                // Если конечный Activity
                //if it last activity
                if (toActivity.IsProcessEnd)
                {
                    FinishProcess(processId);
                }
                else
                {
                    //если нет действаия которое нужно совершить пользователю то процес автоматически должен перейти на след шаг
                    //if there is no one action which have to be done by user then proccess gone to next step automatically
                    CheckNextStepAvailable(toActivity, processId, roles, userId);
                }

                //возвращаем success
                //success
                return true;
            }
            
            throw new GoToStepException("Переход на новый шаг невозможен", processInstance.Id, processInstance.Code);
        }
        public string GetNextStepCode(long processId)
        {
            var processInstance = GetActiveProcessInstance(processId);
            if (processInstance != null)
            {
                var process = _metadataProvider.GetProcess(processInstance.Code);
                var nextActivity =
                    process.Activities.FirstOrDefault(
                        x => x.AllowInputActivities.Any(z => z.Code == processInstance.CurrentActivityCode && z.IsMain));

                if (nextActivity != null)
                {
                    return nextActivity.Code;
                }
                throw new StepNotFoundException();
            }
            throw new ProcessNotFoundException();
        }
        public string GetPrevStepCode(long processId)
        {
            var processInstance = GetActiveProcessInstance(processId);
            if (processInstance != null)
            {
                var process = _metadataProvider.GetProcess(processInstance.Code);
                var prevActivity = process.Activities.FirstOrDefault( x => x.AllowOutputActivities.Any(z => z.Code == processInstance.CurrentActivityCode && z.IsMain));

                if (prevActivity != null)
                {
                    return prevActivity.Code;
                }
                throw new StepNotFoundException();
            }
            throw new ProcessNotFoundException();
        }

        public string GetActualWorkflowStepKey(long processId)
        {
            var processInstance = GetActiveProcessInstance(processId);
            if (processInstance == null)
                throw new ProcessNotFoundException();
            return processInstance.CurrentActivityCode;
        }

        protected bool CheckNextStepAvailable(Activity activity, long processId, string[] roles, string userId)
        {
            if (activity.ActivityActions.All(x => x.Type != ActionType.ByUser))
            {
                try
                {
                    ActivityLink nextStep = activity.AllowOutputActivities.SingleOrDefault(x => x.IsMain);
                    if (nextStep != null)
                    {
                        return GoToStep(processId, nextStep.Code, roles, userId);
                    }
                }
                catch (Exception)
                {
                    // ignored todo
                }
            }

            return false;
        }
        public void CheckNextStepAvailable(long processId, string[] roles, string userId)
        {
            var processInstance = GetActiveProcessInstance(processId);
            if (processInstance != null)
            {
                var process = _metadataProvider.GetProcess(processInstance.Code);
                Activity currentActivity =  process.Activities.FirstOrDefault(x => x.Code == processInstance.CurrentActivityCode);
                if (currentActivity != null)
                {
                    CheckNextStepAvailable(currentActivity,processId, roles,userId);
                }
                throw new StepNotFoundException();
            }
            throw new ProcessNotFoundException();
        }

        public bool SetProcessVariable(long processId, string variableName, string value)
        {
            ProcessInstance processInstance = GetActiveProcessInstance(processId);
            processInstance.AddField(variableName,value);
            _processInstanceRepository.Update(processInstance);
            return true;
        }

        public bool GetProcessVariable(long processId, string variableName, ref string value)
        {
            ProcessInstance processInstance = GetActiveProcessInstance(processId);
            if (processInstance.Fields.Any(x => x.Name == variableName))
            {
                value = processInstance.Fields.Single(x => x.Name == variableName).Value;
            }
            return false;
        }

        public Process GetProcessDescription(string processKey)
        {
            return _metadataProvider.GetProcess(processKey);
        }
    }
}
