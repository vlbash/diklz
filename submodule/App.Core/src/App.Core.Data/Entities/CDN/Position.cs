using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.CDN
{
    [AuditInclude]
    [AuditDisplay("Довідник посад")]
    [Display(Name = "Довідник посад")]
    [Table("Cdn" + nameof(Position))]
	public class Position : BaseDictionary
	{
    }
}
