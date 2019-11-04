using App.Core.Base;
using App.Core.Data.Entities.Common;
using App.Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core.Business.Services
{
    public interface IEntityService<TEntity> where TEntity : CoreEntity
    {
        IEntityRepository<TEntity> Repository { get; set; }

        IQueryable<TEntity> GetEntity();

        TEntity Disable(Guid id, bool transacted = false);
        TEntity Disable(TEntity entity, bool transacted = false);

        TEntity Remove(Guid id, bool transacted = false);
        TEntity Remove(TEntity entity, bool transacted = false);

        void RemoveList(IList<Guid> ids, bool transacted = false);

        Task<Guid> SaveAsync(TEntity entity, bool transacted = false);
        Task<Guid> AttachAsync(TEntity entity);

        Task SaveAsync();
    }
}

