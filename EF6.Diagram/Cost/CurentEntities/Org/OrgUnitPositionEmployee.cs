using Astum.Core.Data.Enums;
using System;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ORG
{
 	public class OrgUnitPositionEmployee : BaseEntity
	{
		public Guid OrgUnitPositionId { get; set; }
		public OrgUnitPosition OrgUnitPosition { get; set; }
        public Guid OrgEmployeeId { get; set; }
        public OrgEmployee OrgEmployee { get; set; }
    }
}
