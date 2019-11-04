using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Business.Services
{
    public class CommonDataService: ICommonDataService
    {
        protected ICommonRepository Repository { get; }
        private readonly IObjectMapper _mapper;

        public CommonDataService(ICommonRepository entityRepository, IObjectMapper objectMapper)
        {
            Repository = entityRepository;
            _mapper = objectMapper;
        }

        public virtual IEnumerable<TDto> GetDto<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO
        {
            return Repository.GetDto(predicate, orderBy, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public virtual async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(Expression<Func<TDto, bool>> predicate = null,
            Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO
        {
            return await Repository.GetDtoAsync(predicate, orderBy, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public virtual IEnumerable<TDto> GetDto<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO
        {
            return Repository.GetDto(orderBy, predicate, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public virtual async Task<IEnumerable<TDto>> GetDtoAsync<TDto>(string orderBy,
            Expression<Func<TDto, bool>> predicate = null,
            IDictionary<string, string> parameters = null,
            int skip = 0,
            int take = 0,
            int cacheResultDuration = 0,
            params object[] extraParameters) where TDto : CoreDTO
        {
            return await Repository.GetDtoAsync(orderBy, predicate, parameters, skip, take, cacheResultDuration, extraParameters);
        }

        public IEnumerable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int skip = 0,
            int take = 0,
            bool doNotTrackChanges = false,
            params string[] includeProperties) where TEntity : class, IEntity
        {
            return Repository.GetEntity(predicate, orderBy, skip, take, doNotTrackChanges, includeProperties);
        }

        public virtual Guid Add<TEntity>(CoreDTO dto, bool? isUpdating = null)
            where TEntity : class, IEntity
        {
            return AddInternal<TEntity>(dto, isUpdating);
        }

        public Guid Add<TEntity>(TEntity entity, bool? isUpdating = null) where TEntity : class, IEntity
        {
            return AddInternal(entity, isUpdating);
        }

        public TEntity Disable<TEntity>(Guid id) where TEntity : class, IEntity
        {
            var entity = Repository.GetEntity<TEntity>(x => x.Id == id && x.RecordState != RecordState.D).SingleOrDefault();
            return Disable(entity);
        }

        public TEntity Disable<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            entity.RecordState = RecordState.D;
            Repository.Update(entity);

            return entity;
        }

        public TEntity Remove<TEntity>(Guid id) where TEntity : class, IEntity
        {
            var entity = Repository.GetEntity<TEntity>(x => x.Id == id && x.RecordState != RecordState.D).SingleOrDefault();
            return Remove(entity);
        }

        public TEntity Remove<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Repository.Remove(entity);
            return entity;
        }

        public void SaveChanges()
        {
            Repository.Save();
        }

        public async Task SaveChangesAsync()
        {
            await Repository.SaveAsync();
        }

        private Guid AddInternal<TEntity>(TEntity entity, bool? isUpdating = null) where TEntity : class, IEntity
        {
            if (isUpdating == false || entity.Id == Guid.Empty)
            {
                Repository.Add(entity);
            }
            else if (isUpdating == true)
            {
                Repository.Update(entity);
            }
            else
            {
                var entityToUpdate = Repository.GetEntity<TEntity>(x => x.Id == entity.Id).FirstOrDefault();
                if (entityToUpdate == null)
                {
                    Repository.Add(entity);
                }
                else
                {
                    Repository.Update(_mapper.Map(entity, entityToUpdate));
                }
            }

            return entity.Id;
        }

        private Guid AddInternal<TEntity>(CoreDTO dto, bool? isUpdating = null) where TEntity : class, IEntity
        {
            TEntity entity = null;
            if (isUpdating == false || dto.Id == Guid.Empty)
            {
                entity = _mapper.Map<TEntity>(dto);
                Repository.Add(entity);
            }
            else
            {
                var entityToUpdate = Repository.GetEntity<TEntity>(x => x.Id == dto.Id).FirstOrDefault();
                if (entityToUpdate == null)
                {
                    if (isUpdating == true)
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        entity = _mapper.Map<TEntity>(dto);
                        Repository.Add(entity);
                    }
                }
                else
                {
                    Repository.Update(_mapper.Map(dto, entityToUpdate));
                    entity = entityToUpdate;
                }
            }

            return entity == null ? Guid.Empty : entity.Id;
        }
    }
}
