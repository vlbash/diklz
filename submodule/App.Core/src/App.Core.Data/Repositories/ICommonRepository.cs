using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Base;

namespace App.Core.Data.Repositories
{
    public interface ICommonRepository
    {
        void Add(IEntity entity);
        Task AddAsync(IEntity entity);
        IEnumerable<TDto> GetDto<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto: class;
        Task<IEnumerable<TDto>> GetDtoAsync<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class;
        IEnumerable<TDto> GetDto<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class;
        Task<IEnumerable<TDto>> GetDtoAsync<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : class;
        IEnumerable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool doNotTrackChanges = false,
            params string[] includeProperties) where TEntity : class, IEntity;
        void Remove(IEntity entity);
        void Save();
        Task SaveAsync();
        void Update(IEntity entity);
    }
}
