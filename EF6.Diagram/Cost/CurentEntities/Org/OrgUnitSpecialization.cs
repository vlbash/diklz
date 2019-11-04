using System.ComponentModel;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.ORG
{
    [AuditDisplay("Спеціалізація")]
    public class OrgUnitSpecialization : BaseEntity
	{
	    [DisplayName("Назва")]
        public string Name { get; set; }
	    [DisplayName("Код")]
	    public string Code { get; set; }
    }
}
