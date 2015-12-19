using CustomBPM.Core.Interfaces;

namespace CustomBPM.Core.ProcessActions.Interfaces
{
    public interface IActionFactory
    {
        IAction Create(string key);
    }
}
