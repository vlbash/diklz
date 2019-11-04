using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using App.Core.Security;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник міст")]
    [Display(Name = "Довідник міст")]
    [Table("Atu" + nameof(City))]
    [RlsRight(nameof(Region), nameof(RegionId))]
    public class City : BaseEntity
	{
	    [MaxLength(128)]
	    public string Name { get; set; }
	    public string Code { get; set; }
	    public string TypeEnum { get; set; }
        public List<CityDistricts> Dictricts { get; set; }
	    public List<Street> Streets { get; set; }
        public Guid RegionId { get; set; }
        public Region Region { get; set; }
    }
}
