using System;
using Dal.Core.Entities;

namespace CustomBPM.Core.DataModel
{
    public class ActivityInstance : EntityBase<long>
    {
        /// <summary>
        /// Identifier of Activity instance 
        /// must be unique in particular Procces
        /// </summary>
        public string Code { get; set; }
        public virtual ProcessInstance ProcessInstance { get; set; }
        public virtual long ProcessInstanceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DisplayName { get; set; }
    }
}
