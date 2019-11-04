using App.Core.Data.Entities.Common;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Base;

namespace App.Core.Business.Services
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : CoreEntity
    {
        public IEntityRepository<TEntity> Repository { get; set; }

        //private static readonly IMapper _mapper;
        protected static readonly string EntityName;

        static EntityService()
        {
            EntityName = typeof(TEntity).Name;
            //_mapper = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TEntity>().MapOnlyIfChanged(EntityName)).CreateMapper();
        }

        public EntityService(IEntityRepository<TEntity> entityRepository)
        {
            Repository = entityRepository;
        }
        
        public IQueryable<TEntity> GetEntity()
        {
            return Repository.GetEntity();
        }

        /// <summary>
        /// Add/Update Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(TEntity entity, bool transacted = false)
        {
            await AttachAsync(entity);

            if (!transacted) {
                await Repository.SaveAsync();
            }

            return entity.Id;
        }

        /// <summary>
        /// Attach without saving
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Guid> AttachAsync(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await Repository.AddAsync(entity);
            }
            else
            {
                var entityUpdate = Repository.GetEntity().FirstOrDefault(x => x.Id == entity.Id);
                if (entityUpdate == null)
                {
                    await Repository.AddAsync(entity);
                }
                else
                {
                    Repository.Update(entityUpdate);
                }
            }

            return entity.Id;
        }

        /// <summary>
        /// Saves data for all repositories
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync() {
            await Repository.SaveAsync();
        }

        public TEntity Disable(Guid id, bool transacted = false)
        {
            return Disable(Repository.GetEntity().SingleOrDefault(x => x.Id == id && x.RecordState != RecordState.D), transacted);
        }

        public TEntity Disable(TEntity entity, bool transacted = false)
        {
            entity.RecordState = RecordState.D;
            Repository.Update(entity);

            if (!transacted) {
                Repository.Save();
            }

            return entity;
        }

        public TEntity Remove(Guid id, bool transacted = false)
        {
            return Remove(Repository.GetEntity().SingleOrDefault(x => x.Id == id && x.RecordState != RecordState.D), transacted);
        }

        public TEntity Remove(TEntity entity, bool transacted = false)
        {
            Repository.Remove(entity);

            if (!transacted) {
                Repository.Save();
            }

            return entity;
        }

        public void RemoveList(IList<Guid> ids, bool transacted = false)
        {
            Repository.GetEntity()
                .Where(x => x.RecordState != RecordState.D && ids.Contains(x.Id))
                .ToList()
                .ForEach(x => Repository.Remove(x));

            if (!transacted) {
                Repository.Save();
            }
        }
    }
}
