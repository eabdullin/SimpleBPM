using System.Collections;
using System.Collections.Generic;
using CustomBPM.Core.DataModel;
using Dal.Core.Repositories;

namespace CustomBPM.Core.Interfaces.Repositories
{
    public interface IProcessInstanceRepository : IRepository<ProcessInstance>
    {
        IList<ProcessInstance> GetActiveProcessInstances(string processCode);
        IList<ProcessInstance> GetAllActiveProcessInstances();
    }
}
