using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("AddressGeoCoordinate")]
	public class AddressGeoCoordinate : BaseEntity
	{
        public long AddressId { get; set; }
		public Address Address { get; set; }
		public long GeoCoordinateId { get; set; }
		public GeoCoordinate GeoCoordinate { get; set; }
	}
}
