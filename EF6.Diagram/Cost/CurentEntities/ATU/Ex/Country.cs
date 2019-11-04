using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("Country")]
	public class Country : BaseEntity
    {
		[MaxLength(64)]
		public string Name { get; set; }
		public string Iso2 { get; set; }
		public string Iso3 { get; set; }
		public string TopLevelDomain { get; set; }
		public string Fips { get; set; }
		public string IsoNumeric { get; set; }
		public long? GeoNameId { get; set; }
		public string E164 { get; set; }
		public long? ContinentId { get; set; }
		public Continent Continent { get; set; }
		//public City Capital { get; set; } //todo fluent
		public long? TimeZoneId { get; set; }
		public TimeZone TimeZone { get; set; }
		public long? CurrencyId { get; set; }
		public Currency Currency { get; set; }
		public Language Language { get; set; }
		public long? AreaKm2 { get; set; }
		public int? InternetHosts { get; set; }
		public int? InternetUsers { get; set; }
		public int? PhonesMobile { get; set; }
		public int? Gdp { get; set; }
		public string PhoneCode { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
