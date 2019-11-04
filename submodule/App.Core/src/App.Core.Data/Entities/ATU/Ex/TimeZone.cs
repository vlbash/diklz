using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("TimeZone")]
	public class TimeZone : BaseEntity
    {
        [MaxLength(1)]
		public string Cc { get; set; }
		[MaxLength(128)]
		public string Name { get; set; }
		public string UtcText { get; set; }
		public string UtcDstText { get; set; }
	}
}
