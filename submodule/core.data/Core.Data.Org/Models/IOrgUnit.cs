using System;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    public interface IOrgUnit : ICoreEntity
    {
        Guid? ParentId { get; set; }
        string Description { get; set; }
        string Code { get; set; }
        string State { get; set; }
        string Category { get; set; }
    }
}
