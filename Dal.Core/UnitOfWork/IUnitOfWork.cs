using System;

namespace Dal.Core.UnitOfWork
{
    ///<summary>
    /// Åäèíèöà ðàáîòû
    ///</summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// save changes to database
        /// </summary>
        void SaveChanges();

        ///<summary>
        /// commit transaction to database
        ///</summary>
        void SaveChangesAndCommit();
    }
}
