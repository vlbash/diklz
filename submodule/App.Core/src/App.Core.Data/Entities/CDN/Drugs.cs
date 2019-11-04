using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.CDN
{
    [AuditInclude]
    [AuditDisplay("Довідник лікарских засобів")]
    [Display(Name = "Довідник лікарских засобів")]
    [Table("Cdn" + nameof(Drugs))]
    public class Drugs: BaseDictionary
    {
    }
}
