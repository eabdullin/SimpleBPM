using System.Collections.Generic;

namespace BPM.Common {
	public delegate void AfterCreateProcess(ProcessDetails processDetails);

	public interface IProcessManager
	{
        ProcessDetails StartProcess(
            string processKey,
            long dossierId,
            string[] roles,
            long dealId,
            string userId = null,
            AfterCreateProcess afterCreateProcess = null,
            int? parentProcessId = null);
        IEnumerable<StepDetails> GetSteps(string processKey);
        void CheckNextStepAvailableForDossier(long dossierId, long userId);
        void CheckNextStepAvailableForDeal(long dealId, long userId);
        WorkItem GetActualWorkItem(long processId);
        GoToStepResult GoToStep(long processId, string stepName, string[] roles, string userId = null);
        GoToStepResult GoToNextStep(long processId, string[] roles, string userId = null);
        GoToStepResult GoToPrevStep(long processId, string[] roles, string userId = null);
        bool FinishProcess(long processId);
    }
}
