using Astum.Core.Data.Enums;
using System;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ATU
{
	public class AtuStreet : BaseEntity
	{
	    public string Name { get; set; }
        public Guid CityId { get; set; }
		public AtuCity City { get; set; }
    }
}
