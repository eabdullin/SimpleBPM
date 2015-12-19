using System.Collections.Generic;
using CustomBPM.Core.DataModel;
using CustomBPM.Core.MetaData;

namespace CustomBPM.Core.Interfaces
{
    public interface IProcessProvider
    {
        Activity GetActivityDescription(string processCode, string acitivityCode);
        ProcessInstance StartProcess(string key, string[] roles, Dictionary<string,string> prameters,  string userId = null);
        Process GetProcessDescription(string processKey);
        IList<ProcessInstance> GetActiveProcessInstances(string processKey);
        IList<ProcessInstance> GetActiveProcessInstances();
        ProcessInstance GetActiveProcessInstance(long processId);
        ProcessInstance GetActiveInstance(long processId);
        bool FinishProcess(long processId);
        bool GoToStep(long processId, string stepName, string[] roles, string userId = null);
        string GetNextStepCode(long processId);
        string GetPrevStepCode(long processId);
        string GetActualWorkflowStepKey(long processId);
        bool SetProcessVariable(long processId, string variableName, string value);
        bool GetProcessVariable(long processId, string variableName, ref string value);
        void CheckNextStepAvailable(long processId, string[] roles, string userId);
    }
}
