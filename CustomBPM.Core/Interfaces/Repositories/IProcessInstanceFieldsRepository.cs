// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessInstanceFieldsRepository.cs" company="Ingenius Systems">
//   Copyright (c) Ingenius Systems
//   Create on 20:07:53 by Еламан Абдуллин
// </copyright>
// <summary>
//   Defines the IProcessInstanceFieldsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CustomBPM.Core.DataModel;
using Dal.Core.Repositories;

namespace CustomBPM.Core.Interfaces.Repositories
{
    public interface IProcessInstanceFieldsRepository : IRepository<ProcessInstanceField>
    {
         
    }
}