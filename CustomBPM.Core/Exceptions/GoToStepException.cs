using System;

namespace CustomBPM.Core.Exceptions
{
    public class GoToStepException:Exception
    {

        public GoToStepException(string message, long processInstanceId, string processCode, string conditionCode = null, string sourceStepCode = null, string destinationStepCode = null)
            : base(message)
        {
            SourceStepCode = sourceStepCode;
            ProcessInstanceId = processInstanceId;
            DestinationStepCode = destinationStepCode;
            ProcessCode = processCode;
            ConditionCode = conditionCode;
        }

        public string ConditionCode { get; private set; }
        public string SourceStepCode { get; private set; }
        public string DestinationStepCode { get; private set; }
        public long ProcessInstanceId { get; private set; }
        public string ProcessCode { get; private set; }
    }
}
