using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Data.Extensions;

namespace App.Core.Data.Repositories
{
    public class SafeEntityRepository<TEntity>: EntityRepository<TEntity> where TEntity : CoreEntity
    {
        private static readonly string _entityName;

        static SafeEntityRepository()
        {
            _entityName = typeof(TEntity).Name;
        }

        public SafeEntityRepository(CoreDbContext context): base(context)
        {
        }

        public override async Task AddAsync(TEntity entity)
        {
            Context.UserInfo.AssertWritableEntity(_entityName);
            await base.AddAsync(entity);
        }

        public override void Remove(TEntity entity)
        {
            Context.UserInfo.AssertWritableEntity(_entityName);
            base.Remove(entity);
        }

        public override async Task RemoveAsync(TEntity entity)
        {
            Context.UserInfo.AssertWritableEntity(_entityName);
            await base.RemoveAsync(entity);
        }

        public override void Update(TEntity entity)
        {
            Context.UserInfo.AssertWritableEntity(_entityName);
            base.Update(entity);
        }
    }
}
