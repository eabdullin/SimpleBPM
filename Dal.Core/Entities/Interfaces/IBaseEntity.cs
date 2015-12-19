using System;

namespace Dal.Core.Entities.Interfaces
{
    public interface IBaseEntity
    {
        /// <summary>
        /// date of creation
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// date of last modifying
        /// </summary>
        DateTime LastModifiedDate { get; set; }
    }
}
