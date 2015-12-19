using System.Collections.Generic;

namespace CustomBPM.Core.Interfaces
{
    public interface ICondition
    {
        bool Execute(out string reasons, IDictionary<string, string> parameters = null);
    }
}
