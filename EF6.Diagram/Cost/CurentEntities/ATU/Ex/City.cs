using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("City")]
	public class City : BaseEntity
    {
		public long CountryId { get; set; }
		public Country Country { get; set; }
		[MaxLength(128)]
		public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual List<Address> Address { get; set; }
    }
}