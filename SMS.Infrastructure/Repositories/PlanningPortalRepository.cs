using Microsoft.EntityFrameworkCore;
using SMS.Application.Interfaces;
using System.Data.Common;
using System.Data;
using System.Linq.Expressions;
using SMS.Infrastructure.Data;

namespace SMS.Presistence.Repositories
{
    public class PlanningPortalRepository<T> : IPlanningPortalRepository<T> where T : class
    {
        #region Private Members

        private readonly ApplicationDbContext _context;
        private DbSet<T> _entities;

        #endregion Private Members

        #region Properties

        protected virtual DbSet<T> Entities => _entities ??= _context.Set<T>();

        public DbSet<T> Table => Entities;

        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        #endregion Properties

        #region Constructors

        public PlanningPortalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> Add(T entity)
        {
            await Entities.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Add(IList<T> entities)
        {
            await Entities.AddRangeAsync(entities);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(IList<T> entities)
        {
            _context.UpdateRange(entities);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> predicate, IReadOnlyDictionary<string, object?> fieldValues)
        {
            var entities = await Entities.Where(predicate).ToListAsync();

            foreach (var entity in entities)
            {
                foreach (var field in fieldValues)
                {
                    var property = entity.GetType().GetProperty(field.Key);
                    if (property != null && field.Value != null)
                    {
                        property.SetValue(entity, field.Value);
                    }
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).ExecuteDeleteAsync() > 0;
        }

        public async Task<bool> Delete(T entity)
        {
            Entities.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(IList<T> entities)
        {
            Entities.RemoveRange(entities);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExecuteNonQuery(string sqlQuery)
        {
            return await _context.Database.ExecuteSqlRawAsync(sqlQuery) == -1;
        }

        public async Task<int> ExecuteStoredProcedure(string Sql, IDictionary<string, object> Parameters)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection?.State != ConnectionState.Open)
                    cmd.Connection?.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public IEnumerable<dynamic> GetCollectionFromSql(string Sql, IDictionary<string, object> Parameters)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection?.State != ConnectionState.Open)
                    cmd.Connection?.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }

        private dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new Dictionary<string, object>();
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }

        #endregion Methods
    }
}
