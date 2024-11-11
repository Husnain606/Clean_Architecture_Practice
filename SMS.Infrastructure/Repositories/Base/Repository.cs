using Microsoft.EntityFrameworkCore;
using SMS.Infrastructure.Data;
using SMS.Common.ViewModels;
using SMS.Application.Interfaces;
using System.Linq.Expressions;

namespace SMS.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> _entities;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // IQueryable<T> implementation
        protected virtual DbSet<T> Entities => _entities ??= _dbContext.Set<T>();

        public DbSet<T> Table => Entities;

        // IQueryable<T> with AsNoTracking
        public IQueryable<T> TableNoTracking => _dbContext.Set<T>().AsNoTracking();
        // CREATE 
        public async Task<ResponseModel> CreateAsync(T createModel)
        {
            ResponseModel<T> model = new ResponseModel<T>();
            try
            {
                await _dbContext.AddAsync<T>(createModel);
                await _dbContext.SaveChangesAsync();
                model.Result = createModel;
                model.Successful = true;
                model.Message = "Entity Created Successfully";
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public async Task<bool> CreateAsync(IList<T> entities)
        {
            try
            {
                await Entities.AddRangeAsync(entities);
                return await _dbContext.SaveChangesAsync() > 0;
                
            }
            catch (Exception)
            {
                throw;
            }
           
            
        }
        // GET THE LIST OF ALL 
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().AsQueryable().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // GET THE LIST OF ALL with AsNoTracking
        public async Task<List<T>> GetAllNoTrackingAsync()
        {
            try
            {
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // GET  DETAILS BY  ID
        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Set<T>()
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE 
        public async Task<ResponseModel> UpdateAsync(T updateModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                _dbContext.Update<T>(updateModel);
                await _dbContext.SaveChangesAsync();
                model.Successful = true;
                model.Message = "Entity Updated Successfully";
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        public async Task<bool> UpdateAsync(IList<T> entities)
        {
            try
            {
                _dbContext.UpdateRange(entities);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        // DELETE 
        public async Task<ResponseModel> DeleteAsync(Guid id)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                T entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbContext.Remove<T>(entity);
                    await _dbContext.SaveChangesAsync();
                    model.Successful = true;
                    model.Message = "Entity Deleted Successfully";
                }
                else
                {
                    model.Successful = false;
                    model.Message = "Entity Not Found";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            try 
            {
                return await Entities.Where(predicate).ExecuteDeleteAsync() > 0;
            }
            catch (Exception) 
            {
                throw;
            }
            
        }
        public async Task<bool> DeleteAsync(IList<T> entities)
        {
            try
            {
                Entities.RemoveRange(entities);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}