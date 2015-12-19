using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dal.Core.Entities.Interfaces;

namespace Dal.Core.Repositories
{
    public interface IRepository<T> : IDisposable where T : IBaseEntity
    {
        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        void Create(T t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>        
        void Delete(T t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update an object to database.
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);

        /// <summary>
        /// Find object by specified expression.
        /// <exception cref="InvalidOperationException">the database contains more than one element by the expression</exception>
        /// <exception cref="ArgumentNullException">the expression cannot be null</exception>
        /// </summary>
        /// <param name="expression">expression for indicate entity</param>
        T Find(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Find object by specified expression.
        /// <exception cref="InvalidOperationException">the database contains more than one element by the expression</exception>
        /// <exception cref="ArgumentNullException">the expression cannot be null</exception>
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Find an object from database by key
        /// </summary>
        /// <param name="keys">primary keys of entity</param>
        /// <returns></returns>
        T Find(params object[] keys);

        /// <summary>
        /// Find an object from database by key
        /// </summary>
        /// <param name="keys">primary keys of entity</param>
        /// <returns></returns>
        Task<T> FindAsync(params object[] keys);

        /// <summary>
        /// Get objects count.
        /// </summary>
        /// <param name="expression">Specified the filter expression. if expression is null method will return the total objects count </param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> expression = null);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        bool Contains(Expression<Func<T, bool>> predicate);

    }
}