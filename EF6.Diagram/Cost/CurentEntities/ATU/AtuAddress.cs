using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuAddress : BaseEntity
	{
		public Guid CityId { get; set; }
        public AtuCity City { get; set; }
		public Guid StreetId { get; set; }
        public AtuStreet Street { get; set; }
	    [MaxLength(20)]
	    public string PostIndex { get; set; }
        [MaxLength(50)]
        public string Building { get; set; }
        public string AddressType { get; set; }
    }
}
