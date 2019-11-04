using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("Street")]
	public class Street : BaseEntity
    {
		public long CityId { get; set; }
		public City City { get; set; }
		[MaxLength(128)]
		public string Name { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
