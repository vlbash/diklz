using System;
using System.ComponentModel;
using Astum.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;
using Astum.Core.Data.Entities.ATU;

namespace Astum.Core.Data.Entities.ORG
{
    [AuditInclude]
    [AuditDisplay("Співробітник")]
    public sealed class OrgEmployee : BaseEntity
    {
        [DisplayName("Персона")]
        public Guid PersonId { get; set; }
        [DisplayName("Персона")]
        public Person Person { get; set; }

        [DisplayName("Спеціалізація")]
        public Guid? OrgUnitSpecializationId { get; set; }
        [DisplayName("Спеціалізація")]
        public OrgUnitSpecialization OrgUnitSpecialization { get; set; }

        [DisplayName("Область")]
        public Guid? AtuRegionId { get; set; }
        [DisplayName("Область")]
        public AtuRegion AtuRegion { get; set; }
    }
}
