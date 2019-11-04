using Microsoft.EntityFrameworkCore;

namespace Astum.Core.Data.Interfaces
{
    public interface IDerivedEntity : IEntity
    {
        string BaseClass { get; set; }
        IEntity BaseClone { get; }
        IEntity BaseQuery(DbContext context);
        IEntity BaseUpdate(DbContext context);
    }
}
