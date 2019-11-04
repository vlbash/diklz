using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.ORG
{
    public sealed class OrgUnit : BaseEntity, IOrgUnit, IDerivableEntity
    {
        public string DerivedClass { get; set; }

        [DisplayName("Назва")]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        
        public ICollection<OrgUnitAtuAddress> OrgUnitAtuAddresses { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
    }
}
