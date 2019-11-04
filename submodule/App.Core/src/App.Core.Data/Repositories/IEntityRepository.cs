using System.Linq;
using System.Threading.Tasks;
using App.Core.Base;

namespace App.Core.Data.Repositories
{
    public interface IEntityRepository<TEntity> where TEntity : CoreEntity
    {
        IQueryable<TEntity> GetEntity();
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void Save();
        Task SaveAsync();
        void Update(TEntity entity);
    }
}
