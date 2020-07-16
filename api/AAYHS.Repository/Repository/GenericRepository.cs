using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBContext;
using AAYHS.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        protected DbSet<T> DbSet;
        public GenericRepository(AAYHSDBContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<T>();
        }
        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            int result = Save();
            if (result > 0)
                return entity;
            else
                return null;
        }

        public void Delete(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            Save();
        }

        public T Get<Tkey>(T id)
        {
            return DbSet.Find(id);
        }

        public List<T> GetAll()
        {
            return DbSet.ToList<T>();
        }

        public List<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return DbSet.Where(whereCondition).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return DbSet.Where(whereCondition).FirstOrDefault<T>();
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }
        private int Save()
        {
            int result = _dbContext.SaveChanges();
            return result;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            int result = await SaveAsync();
            if (result > 0)
                return entity;
            else
                return null;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }

        private async Task<int> SaveAsync()
        {
            int result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task SPExecuteNonQueryAsync(DynamicParameters dynamicParameters, String StorePorcedureName)
        {
            using (IDbConnection db = new SqlConnection(AppSettingConfigurations.AppSettings.ApplicationContext))
            {
                await db.ExecuteAsync(StorePorcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> SPReadDataListAsync(DynamicParameters dynamicParameters, String StorePorcedureName)
        {
            using (IDbConnection db = new SqlConnection(AppSettingConfigurations.AppSettings.ApplicationContext))
            {
                var result = await db.QueryAsync(StorePorcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result.ToList());
            }
        }

        public async Task<string> SPReadSingleDataAsync(DynamicParameters dynamicParameters, String StorePorcedureName)
        {
            using (IDbConnection db = new SqlConnection(AppSettingConfigurations.AppSettings.ApplicationContext))
            {
                var result = await db.QueryAsync(StorePorcedureName, dynamicParameters, commandType: CommandType.StoredProcedure);
                return JsonConvert.SerializeObject(result.FirstOrDefault());
            }
        }


        public List<T> GetRecordsWithFilters(int page, int limit, string orderBy, bool orderByDescending, bool AllRecords)
        {
            IQueryable<T> dbSet;
            if (orderByDescending)
            {
                dbSet = DbSet.OrderByDescending(p => EF.Property<T>(p, orderBy));
            }
            else
            {
                dbSet = DbSet.OrderBy(p => EF.Property<T>(p, orderBy));
            }
            if (AllRecords)
            {
                return DbSet.ToList<T>();
            }
            else
            {
                return dbSet.Skip((page - 1) * limit).Take(limit).ToList<T>();
            }
        }

        public List<T> GetRecordsWithFilters(int page, int limit, string orderBy, bool orderByDescending, bool AllRecords, Expression<Func<T, bool>> whereCondition)
        {
            var dbSet = DbSet.Where(whereCondition);
            if (orderByDescending)
            {
                dbSet = dbSet.OrderByDescending(p => EF.Property<T>(p, orderBy));
            }
            else
            {
                dbSet = dbSet.OrderBy(p => EF.Property<T>(p, orderBy));
            }
            if (AllRecords)
            {
                return dbSet.ToList<T>();
            }
            else
            {
                return dbSet.Skip((page - 1) * limit).Take(limit).ToList<T>();
            }

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    dbSet = dbSet.Where(x => Convert.ToString(x.GetType().GetProperty(searchBy).
            //                             GetValue(x)).ToLower().Contains(searchString.ToLower()));
            //}
        }

        public int GetTotalCount()
        {
            return DbSet.Count();
        }

        public int GetTotalCount(Expression<Func<T, bool>> whereCondition)
        {
            return DbSet.Count(whereCondition);
        }
    }
}
