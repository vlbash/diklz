using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.ORG
{
	public class OrgPosition : BaseEntity
	{
        public string Name { get; set; }
	    public string Code { get; set; }
    }
}
