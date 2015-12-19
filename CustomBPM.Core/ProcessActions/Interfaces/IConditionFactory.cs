using CustomBPM.Core.Interfaces;

namespace CustomBPM.Core.ProcessActions.Interfaces
{
    public interface IConditionFactory
    {
        ICondition Create(string key);
    }
}
