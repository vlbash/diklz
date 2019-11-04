using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Data.Extensions;
using App.Core.Data.Helpers;

namespace App.Core.Data.Repositories
{
    public class SafeCommonRepository: CommonRepository
    {
        public SafeCommonRepository(CoreDbContext context, ISqlRepositoryHelper repositoryHelper, IQueryableCacheService qcache):
            base(context, repositoryHelper, qcache)
        {
            CurrentRights = context.UserInfo?.Rights;
        }

        public override IEnumerable<TDto> GetDto<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters)
        {
            Context.UserInfo.AssertCanReadTypeData(typeof(TDto));
            return base.GetDto(predicate, orderBy, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public override async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters)
        {
            Context.UserInfo.AssertCanReadTypeData(typeof(TDto));
            return await base.GetDtoAsync(predicate, orderBy, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public override IEnumerable<TDto> GetDto<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters)
        {
            Context.UserInfo.AssertCanReadTypeData(typeof(TDto));
            return base.GetDto(orderBy, predicate, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public override async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters)
        {
            Context.UserInfo.AssertCanReadTypeData(typeof(TDto));
            return await base.GetDtoAsync(orderBy, predicate, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public override IEnumerable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool doNotTrackChanges = false,
            params string[] includeProperties)
        {
            Context.UserInfo.AssertCanReadTypeData(typeof(TEntity));
            return base.GetEntity(predicate, orderBy, skip, take, doNotTrackChanges, includeProperties);
        }

        public override void Add(IEntity entity)
        {
            Context.UserInfo.Rights.AssertCanWriteEntity(entity.GetType().Name, entity);
            base.Add(entity);
        }

        public override async Task AddAsync(IEntity entity)
        {
            Context.UserInfo.Rights.AssertCanWriteEntity(entity.GetType().Name, entity);
            await base.AddAsync(entity);
        }

        public override void Remove(IEntity entity)
        {
            Context.UserInfo.Rights.AssertCanWriteEntity(entity.GetType().Name, entity);
            base.Remove(entity);
        }

        public override void Update(IEntity entity)
        {
            Context.UserInfo.Rights.AssertCanWriteEntity(entity.GetType().Name, entity);
            base.Update(entity);
        }
    }
}
