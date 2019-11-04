using Astum.Core.Data.Entities.ORG;
using System;
using System.Collections.Generic;

namespace Astum.Core.Data.Interfaces
{
    public interface IOrgUnit : IBaseEntity
    {
        string Name { get; set; }
        Guid? ParentId { get; set; }
        string Description { get; set; }
        string Code { get; set; }
        ICollection<OrgUnitAtuAddress> OrgUnitAtuAddresses { get; set; }
        string State { get; set; }
        string Category { get; set; }
    }
}
