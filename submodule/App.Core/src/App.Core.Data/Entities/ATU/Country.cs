using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник країн")]
    [Display(Name = "Довідник країн")]
    [Table("Atu" + nameof(Country))]
    public class Country : BaseEntity
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }
        public List<Region> Region { get; set; }
    }
}
