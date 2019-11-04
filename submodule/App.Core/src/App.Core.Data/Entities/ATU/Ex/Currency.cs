using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("Currency")]
	public class Currency : BaseEntity
    {
		[MaxLength(128)]
		public string Name { get; set; }
	}
}
