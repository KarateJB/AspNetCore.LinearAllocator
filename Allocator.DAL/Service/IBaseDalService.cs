using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Allocator.DAL.Service
{
    /// <summary>
    /// DAL service's interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IBaseDalService<T> : IDisposable
    {
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>IQueryable T</returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// Get records with filters
        /// </summary>
        /// <param name="filter">Filter func</param>
        /// <returns>IQueryable T</returns>
        IQueryable<T> Get(Expression<Func<T, bool>> filter);
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity">Entity</param>
        void Add(T entity);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity">Entity</param>
        void Remove(T entity);
        /// <summary>
        /// Truncate (Be carefule! This method is not transaction-protected.)
        /// </summary>
        /// <remark>It's recommanded that you override this function.</remark>
        void Clear();
        /// <summary>
        /// Update (Partial update is not supported.)
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);
       

    }
}