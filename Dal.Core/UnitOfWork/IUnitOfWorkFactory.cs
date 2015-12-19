using System.Data;

namespace Dal.Core.UnitOfWork
{
    ///<summary>
    /// UOW factory
    ///</summary>
    public interface IUnitOfWorkFactory
    {
        ///<summary>
        /// create an unit of work with isolation level. by default it Read.Committed
        ///</summary>
        ///<param name="isolationLevel"></param>
        ///<returns></returns>
        IUnitOfWork Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    }
}
