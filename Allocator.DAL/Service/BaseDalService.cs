using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Allocator.DAL.Extensions;
using Allocator.DAL.Models;

namespace Allocator.DAL.Service
{
    /// <summary>
    /// Base Service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDalService<T> : IBaseDalService<T> where T : UowEntity
    {
        protected DbContext _dbContext = null;
        protected DbSet<T> _entities = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">DbContext for injection</param>
        public BaseDalService(DbContext dbContext)
        {
            this._dbContext = dbContext;
            this._entities = this._dbContext.Set<T>();
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>IQueryable T</returns>
        public virtual IQueryable<T> GetAll()
        {
            return this._entities.AsQueryable();
        }

        /// <summary>
        /// Get records with filters
        /// </summary>
        /// <param name="filter">Filter func</param>
        /// <returns>IQueryable T</returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return this._entities.Where(filter).AsQueryable();
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Add(T entity)
        {
            try
            {
                this._entities.Add(entity);
                this._dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Remove(T entity)
        {
            try
            {
                this._entities.Remove(entity);
                this._dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Truncate 
        /// (Be carefule! Though this method is transaction-protected, you cannot rollback in your codes.)
        /// </summary>
        /// <remark>It's recommanded that you override this function.</remark>
        public virtual void Clear()
        {
            var tableName = this._dbContext.GetTableName<T>();

            var sql = new StringBuilder(300);
            sql.Append(String.Format(@"DELETE {0}; ", tableName));

            try
            {
                using (var dbContextTransaction =
                    this._dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        this._dbContext.Database.SetCommandTimeout(new TimeSpan(0, 1, 0));//Timeout = 1 min
                        this._dbContext.Database.ExecuteSqlCommand(sql.ToString());
                        this._dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sql.Clear();
            }

        }

        /// <summary>
        /// Update (Partial update is not supported.)
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            try
            {
                this._dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Detach the entity from current DbContext
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Detach(T entity)
        {
            try
            {
                //Detach method 1 : Using ObjectContext
                //ObjectContext objContext = ((IObjectContextAdapter)this._dbContext).ObjectContext;
                //objContext.Detach(bloodType);
                //Detach method 2 : Using EntityState to detach
                this._dbContext.Entry(entity).State = EntityState.Detached;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Attach the entity to the current DbContext
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Attach(T entity)
        {
            try
            {
                //Detach method 2 : Using EntityState to attach
                this._dbContext.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}