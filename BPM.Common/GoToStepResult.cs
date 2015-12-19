namespace BPM.Common
{
    public class GoToStepResult
    {

        public GoToStepResult()
        {
            Success = true;
        }
        public GoToStepResult(string reason)
        {
            Success = false;
            Reason = reason;
        }
        public GoToStepResult(string code, string reason)
        {
            Success = false;
            ReasonCode = code;
            Reason = reason;
        }
        public bool Success { get; set; }
        public string ReasonCode { get; set; }
        public string Reason { get; set; }
    }
}
