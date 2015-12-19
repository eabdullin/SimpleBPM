using System;
using System.Collections.Generic;
using System.Linq;
using BPM.Common;
using CustomBPM.Core.DataModel;
using CustomBPM.Core.Exceptions;
using CustomBPM.Core.Interfaces;
using CustomBPM.Core.MetaData;
using Status = CarAdvisor.BPM.Common.ProcessStatus;

namespace CustomBPM
{
    public class LocalProcessManager:IProcessManager
    {
        private readonly IProcessProvider _processProvider;
        private readonly IUsersRepository _usersRepository;
        private IDossiersRepository _dossiersRepository;
        private IDealsRepository _dealsRepository;
        public LocalProcessManager(IProcessProvider processProvider, IUsersRepository usersRepository, IDossiersRepository dossiersRepository, IDealsRepository dealsRepository)
        {
            _processProvider = processProvider;
            _usersRepository = usersRepository;
            _dossiersRepository = dossiersRepository;
            _dealsRepository = dealsRepository;
        }

        public ProcessDetails StartProcess(
            string processKey, 
            long dossierId,
            string[] roles, 
            long dealId,
            string userId = null,
            AfterCreateProcess afterCreateProcess = null, 
            int? parentProcessId = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {ProcessConstants.DossierId, dossierId.ToString()},
                {ProcessConstants.DealId, dealId.ToString()}
            };
            ProcessInstance processInstance = _processProvider.StartProcess(processKey,roles,parameters, userId);
            Status status;
            Enum.TryParse(processInstance.Status.ToString(), true,out status);
            return new ProcessDetails()
            {
                ProcessId = processInstance.Id,
                Name = processInstance.Name,
                Status = status
            };
        }

        public IEnumerable<StepDetails> GetSteps(string processKey)
        {
            if(processKey == null)
                throw new ArgumentNullException("processKey");
            var process = _processProvider.GetProcessDescription(processKey);

            var processInstances = _processProvider.GetActiveProcessInstances(processKey);
            List<Activity> mainWorkFlow = process.GetMainWorkFlow();
            return mainWorkFlow.Select(x => new StepDetails()
            {
                Name = x.Name,
                Key = x.Code,
                View = x["View"],
                ProcessInstanceCount = processInstances.Count(p => p.CurrentActivityCode == x.Code),
                Roles = x.AllowRoles
            }).ToList();

        }

        public void CheckNextStepAvailableForDossier(long dossierId, long userId)
        {
            var dossier = _dossiersRepository.Find(dossierId);
            foreach (var deal in dossier.Deals)
            {
                CheckNextStepAvailableForDeal(deal,userId);
            }
        }

        protected void CheckNextStepAvailableForDeal(Deal deal, long userId)
        {
            var user = _usersRepository.Find(userId);
            _processProvider.CheckNextStepAvailable(deal.ProcessId, user.RolesString, user.Id.ToString());
        }
        public void CheckNextStepAvailableForDeal(long dealId, long userId)
        {
            var deal = _dealsRepository.Find(dealId);
            CheckNextStepAvailableForDeal(deal, userId);
        }



        public bool StopProcess(long processId)
        {
            throw new NotImplementedException();
        }

        public WorkItem GetActualWorkItem(long processId)
        {
            ProcessInstance processInstance = _processProvider.GetActiveInstance(processId);
            ActivityInstance activityInstance = processInstance.CurrentActivity;
            var activity = _processProvider.GetActivityDescription(processInstance.Code, activityInstance.Code);
            return new WorkItem()
            {
                ActivityId = activityInstance.Id,
                ProcessId = processId,
                Title = activity.Name
            };
        }


        public GoToStepResult GoToStep(long processId, string stepName, string[] roles, string userId = null)
        {
            try
            {
                if (_processProvider.GoToStep(processId, stepName, roles, userId))
                {
                    return new GoToStepResult();
                }
            }
            catch (GoToStepException exception)
            {
                return new GoToStepResult(exception.ConditionCode,exception.Message);
            }
            return new GoToStepResult("Переход на новый шаг невозможен");
        }

        public GoToStepResult GoToNextStep(long processId, string[] roles, string userId = null)
        {
            try
            {
                string nextStepCode = _processProvider.GetNextStepCode(processId);
                if (_processProvider.GoToStep(processId, nextStepCode, roles, userId))
                {
                    return new GoToStepResult();
                }
            }
            catch (GoToStepException exception)
            {
                return new GoToStepResult(exception.ConditionCode, exception.Message);
            }

            return new GoToStepResult("Переход на следующий шаг невозможен");
        }
        public GoToStepResult GoToPrevStep(long processId, string[] roles, string userId = null)
        {
            try
            {
                string prevStepCode = _processProvider.GetPrevStepCode(processId);
                if (_processProvider.GoToStep(processId, prevStepCode, roles,userId))
                {
                    return new GoToStepResult();
                }
            }
            catch (GoToStepException exception)
            {
                return new GoToStepResult(exception.ConditionCode, exception.Message);
            }

            return new GoToStepResult("Переход на предыдущий шаг невозможен");
        }

        public bool FinishProcess(long processId)
        {
            return _processProvider.FinishProcess(processId);
        }
    }
}
