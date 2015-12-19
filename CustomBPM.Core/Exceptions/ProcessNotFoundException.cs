using System;

namespace CustomBPM.Core.Exceptions
{
    public class ProcessNotFoundException:Exception
    {
        public ProcessNotFoundException() : base("Процесс не найден")
        {
            
        }
    }
}
