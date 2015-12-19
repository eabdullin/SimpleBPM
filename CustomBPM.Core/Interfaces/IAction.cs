using System.Collections.Generic;

namespace CustomBPM.Core.Interfaces
{
    public interface IAction
    {
        void Execute(IDictionary<string, string> parameters);
    }
}
