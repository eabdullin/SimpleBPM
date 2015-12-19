using System.Collections.Generic;

namespace CustomBPM.Core.Interfaces
{
    public interface IProcessActionProvider
    {
        void ExecuteAction(string key, IDictionary<string, string> parameters = null);
        bool ExecuteCondition(string key, out string reason, IDictionary<string, string> parameters = null);
    }
}
