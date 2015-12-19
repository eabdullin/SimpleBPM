using System;
using Dal.Core.Entities.Interfaces;

namespace Dal.Core.Entities
{
    public abstract class EntityBase<T> : IBaseEntity, IEntity<T>
    {

        public virtual T Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public DateTime LastModifiedDate { get; set; }


        //public bool IsHistoric { get { return HistoryDate != null; } }
        //public DateTime? HistoryDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EntityBase<T>)
            {
                var entityBase = (EntityBase<T>)obj;
                return this.Id.Equals(entityBase.Id);
            }
            return false;
        }
    }
}
