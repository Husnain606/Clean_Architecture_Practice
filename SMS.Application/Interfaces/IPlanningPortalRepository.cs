using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SMS.Application.Interfaces
{
    public interface IPlanningPortalRepository<T> where T : class
    {
        Task<bool> Add(T entity);

        Task<bool> Add(IList<T> entities);

        Task<bool> Update(T entity);

        Task<bool> Update(IList<T> entities);

        Task<bool> UpdateAsync(Expression<Func<T, bool>> predicate, IReadOnlyDictionary<string, object?> fieldValues);

        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);

        Task<bool> Delete(T entity);

        Task<bool> Delete(IList<T> entities);

        DbSet<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }

        Task<bool> ExecuteNonQuery(string sqlQuery);

        IEnumerable<dynamic> GetCollectionFromSql(string Sql, IDictionary<string, object> Parameters);

        Task<int> ExecuteStoredProcedure(string Sql, IDictionary<string, object> Parameters);
    }
}
