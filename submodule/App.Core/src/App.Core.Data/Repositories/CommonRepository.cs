using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Data.Extensions;
using App.Core.Data.Helpers;
using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using App.Core.Common.Extensions;
using App.Core.Data.CustomAudit;
using App.Core.Security;
using LinqKit;

namespace App.Core.Data.Repositories
{
    public class CommonRepository: ICommonRepository
    {
        protected CoreDbContext Context { get; }
        protected ISqlRepositoryHelper RepositoryHelper { get; }
        protected IQueryableCacheService QueryableCache { get; }
        protected UserApplicationRights CurrentRights { get; set; } = null;

        public CommonRepository(CoreDbContext context, ISqlRepositoryHelper repositoryHelper, IQueryableCacheService qcache)
        {
            Context = context;
            RepositoryHelper = repositoryHelper;
            QueryableCache = qcache;
        }

        public virtual IEnumerable<TDto> GetDto<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class
        {
            var dtoType = typeof(TDto);
            var addCount = typeof(IPagingCounted).IsAssignableFrom(dtoType);
            var sqlText = RepositoryHelper.GetParameterizedQueryString(dtoType, parameters, addCount, CurrentRights, extraParameters);
            return GetDtoInternal<TDto>(sqlText, predicate, orderBy, cacheResultDuration, skip, take, addCount);
        }

        public virtual async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class
        {
            var dtoType = typeof(TDto);
            var addCount = typeof(IPagingCounted).IsAssignableFrom(dtoType);
            var sqlText = await RepositoryHelper.GetParameterizedQueryStringAsync(dtoType,
                                    parameters,
                                    addCount,
                                    CurrentRights,
                                    extraParameters);
            return GetDtoInternal<TDto>(sqlText, predicate, orderBy, cacheResultDuration, skip, take, addCount);
        }

        public virtual IEnumerable<TDto> GetDto<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class
        {
            var dtoType = typeof(TDto);
            var addCount = typeof(IPagingCounted).IsAssignableFrom(dtoType);
            var sqlText = RepositoryHelper.GetParameterizedQueryString(dtoType, parameters, addCount, CurrentRights, extraParameters);
            return GetDtoInternal<TDto>(sqlText, predicate, orderBy, cacheResultDuration, skip, take, addCount);
        }

        public virtual async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class
        {
            var dtoType = typeof(TDto);
            var addCount = typeof(IPagingCounted).IsAssignableFrom(dtoType);
            var sqlText = await RepositoryHelper.GetParameterizedQueryStringAsync(dtoType,
                                    parameters,
                                    addCount,
                                    CurrentRights,
                                    extraParameters);

            return GetDtoInternal(sqlText, predicate, orderBy, cacheResultDuration, skip, take, addCount);
        }

        public virtual IEnumerable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool doNotTrackChanges = false,
            params string[] includeProperties) where TEntity : class, IEntity
        {
            IQueryable<TEntity> result;
            if (predicate != null)
            {
                result = Context.Set<TEntity>().Where(predicate);
            }
            else
            {
                result = Context.Set<TEntity>();
            }

            result = AddRightsExpression(result);

            foreach (var property in includeProperties)
            {
                result = result.Include(property);
            }

            if (doNotTrackChanges)
            {
                result = result.AsNoTracking();
            }

            if (orderBy != null)
            {
                result = orderBy(result);
            }

            if (skip > 0)
            {
                result = result.Skip(skip);
            }
            
            if (take > 0)
            {
                result = result.Take(take);
            }

            var data = result.ToArray();
            foreach (var item in data)
            {
                yield return item;
            }
        }

        public virtual void Add(IEntity entity)
        {
            Context.Add_Auditable(entity);
        }

        public virtual async Task AddAsync(IEntity entity)
        {
            await Context.AddAsync_Auditable(entity);
        }

        public virtual void Remove(IEntity entity)
        {
            Context.Remove_Auditable(entity);
        }

        public virtual void Update(IEntity entity)
        {
            Context.Update_Auditable(entity);
        }

        public virtual void Save()
        {
            Context.SaveChanges_Auditable();
        }
        public virtual async Task SaveAsync()
        {
            await Context.SaveChangesAsync_Auditable();
        }

        protected virtual IEnumerable<T> GetSetCacheData<T>(IQueryable<T> data, string sqlText, int cacheResultDuration) where T : class
        {
            IEnumerable<T> result;
            if (cacheResultDuration > 0)
            {
                var cachedResult = QueryableCache.GetData<T>(sqlText, data.Expression);
                if (cachedResult.Any())
                {
                    result = cachedResult;
                }
                else
                {
                    result = QueryableCache.SetData<T>(data, cacheResultDuration, sqlText);
                }
            }
            else
            {
                result = data.ToArray();
            }

            return result;
        }

        protected virtual IEnumerable<T> GetSetCacheData<T>(IOrderedQueryable<T> data, string sqlText, int cacheResultDuration) where T : class
        {
            IEnumerable<T> result;
            if (cacheResultDuration > 0)
            {
                var cachedResult = QueryableCache.GetData<T>(sqlText, data.Expression);
                if (cachedResult.Any())
                {
                    result = cachedResult;
                }
                else
                {
                    result = QueryableCache.SetData<T>(data, cacheResultDuration, sqlText);
                }
            }
            else
            {
                result = data.ToArray();
            }

            return result;
        }

        private IEnumerable<TDto> GetDtoInternal<TDto>(string sqlText,
            Expression<Func<TDto, bool>> predicate,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy,
            int cacheResultDuration,
            int skip = 0,
            int take = 0,
            bool addCount = false) where TDto : class
        {
            var query = GetFilteredDto(sqlText, predicate);
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // under these conditions there is no way calculate records count otherwise than executing an extra query
            var count = 0;
            if (addCount && predicate != null)
            {
                count = query.Count();
            }

            query = GetSkipTakeDto(query, skip, take);

            var result = GetSetCacheData<TDto>(query, sqlText, cacheResultDuration);

            if (addCount && predicate != null)
            {
                foreach (var item in (IEnumerable<IPagingCounted>)result)
                {
                    item.TotalRecordCount = count;
                }
            }

            return result;
        }

        private IEnumerable<TDto> GetDtoInternal<TDto>(string sqlText,
            Expression<Func<TDto, bool>> predicate,
            string orderBy,
            int cacheResultDuration,
            int skip = 0,
            int take = 0,
            bool addCount = false) where TDto : class
        {
            var query = GetFilteredDto(sqlText, predicate);
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy<TDto>(orderBy);
            }

            // under these conditions there is no way calculate records count otherwise than executing an extra query
            var count = 0;
            if (addCount && predicate != null)
            {
                count = query.Count();
            }

            query = GetSkipTakeDto(query, skip, take);

            var result = GetSetCacheData(query, sqlText, cacheResultDuration);

            if (addCount && predicate != null)
            {
                foreach (var item in (IEnumerable<IPagingCounted>)result)
                {
                    item.TotalRecordCount = count;
                }
            }

            return result;
        }

        private IQueryable<TDto> GetFilteredDto<TDto>(string sqlText, Expression<Func<TDto, bool>> predicate) where TDto: class
        {
            var query = Context.Query<TDto>().FromSql(sqlText);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        private IQueryable<TDto> GetSkipTakeDto<TDto>(IQueryable<TDto> query, int skip, int take) where TDto: class
        {
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            
            if (take != 0)
            {
                query = query.Take(take);
            }
            return query;
        }

        private IQueryable<TEntity> AddRightsExpression<TEntity>(IQueryable<TEntity> query) where TEntity : class
        {
            var result = query;
            if (CurrentRights == null)
            {
                return result;
            }

            result = result.Where(x => CurrentRights.RlsAllowsAccessToObject(x));

            return result;
        }
    }
}
