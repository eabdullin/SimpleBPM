using System.Collections.Generic;
using CustomBPM.Core.Interfaces;
using CustomBPM.Core.ProcessActions.Interfaces;

namespace CustomBPM.Core.ProcessActions
{
    public class ProcessActionProvider :IProcessActionProvider
    {
        private readonly IActionFactory _actionFactory;
        private readonly IConditionFactory _conditionFactory;

        public ProcessActionProvider(IActionFactory actionFactory, IConditionFactory conditionFactory)
        {
            _actionFactory = actionFactory;
            _conditionFactory = conditionFactory;
        }

        public void ExecuteAction(string key, IDictionary<string, string> parameters = null)
        {
            
            IAction action = _actionFactory.Create(key);
            action.Execute(parameters);
        }

        public bool ExecuteCondition(string key, out string reason, IDictionary<string, string> parameters = null)
        {
            if (key == null)
            {
                reason = null;
                return true;
            }
            ICondition action = _conditionFactory.Create(key);
            return action.Execute(out reason, parameters);
        }
    }
}
