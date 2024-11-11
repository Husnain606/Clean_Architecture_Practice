using Microsoft.EntityFrameworkCore;
using SMS.Common.ViewModels;
using System.Linq.Expressions;

namespace SMS.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<ResponseModel> CreateAsync(T CreateModel);
        Task<bool> CreateAsync(IList<T> CreateModel);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid ID);
        Task<ResponseModel> UpdateAsync(T UpdateModel);
        Task<bool> UpdateAsync(IList<T> UpdateModel);
        Task<ResponseModel> DeleteAsync(Guid ID);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<bool> DeleteAsync(IList<T> entities);

        DbSet<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}

