using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("Address")]
	public class Address : BaseEntity
	{
        public long CountryId { get; set; }
		public Country Country { get; set; }
		public long? CityId { get; set; }
        public City City { get; set; }
		public long? StreetId { get; set; }
        public Street Street { get; set; }
		public double Latitude { get; set; } //todo presision
		public double Longitude { get; set; }
	}
}
