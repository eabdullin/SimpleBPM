using System;

namespace CustomBPM.Core.Exceptions
{
    public class ProcessAlreadyInStepException:Exception
    {
        public ProcessAlreadyInStepException() : base("Процесс уже на этом шаге")
        {
            
        }
    }
}
