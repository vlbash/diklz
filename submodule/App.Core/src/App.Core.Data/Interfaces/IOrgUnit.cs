using App.Core.Base;
using App.Core.Data.Entities.ORG;
using System;
using System.Collections.Generic;

namespace App.Core.Data.Interfaces
{
    public interface IOrgUnit : ICoreEntity
    {
        string Name { get; set; }
        Guid? ParentId { get; set; }
        string Description { get; set; }
        string Code { get; set; }
        string State { get; set; }
        string Category { get; set; }
    }
}
