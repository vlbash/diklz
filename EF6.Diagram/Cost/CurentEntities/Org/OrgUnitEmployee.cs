using Astum.Core.Data.Enums;
using System;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ORG
{
	public class OrgUnitEmployee : BaseEntity
	{
        public Guid OrgUnitId { get; set; }
	    public Guid EmployeeId { get; set; }
	}
}
