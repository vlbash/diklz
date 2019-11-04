using System.Threading.Tasks;
using System.Linq;
using App.Core.Base;
using App.Core.Data.Extensions;
using App.Core.Data.Interfaces;
using App.Core.Data.Entities.Common;
using App.Core.Business.Services;
using App.Core.Data.CustomAudit;

namespace App.Core.Data.Repositories
{
    public class EntityRepository<TEntity>: IEntityRepository<TEntity> where TEntity : CoreEntity
    {
        protected CoreDbContext Context { get; }

        public EntityRepository(CoreDbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> GetEntity()
        {
            return Context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await Context.AddAsync_Auditable(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Remove_Auditable(entity);
        }

        public virtual async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                Remove(entity);
            });
        }

        public virtual void Update(TEntity entity)
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
    }
}
