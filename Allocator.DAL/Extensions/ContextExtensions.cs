using Microsoft.EntityFrameworkCore;

namespace Allocator.DAL.Extensions
{
    public static class ContextExtensions
    {
        /// <summary>
        /// Get the table name of Entity
        /// </summary>
        /// <typeparam name="T">Entity's type</typeparam>
        /// <param name="dbContext">DbContext</param>
        /// <returns>Table name</returns>
        public static string GetTableName<T>(this DbContext dbContext) where T : class
        {
            var mapping = dbContext.Model.FindEntityType(typeof(T).FullName).Relational();
            var schema = mapping.Schema;
            return mapping.TableName;
        }
    }
}