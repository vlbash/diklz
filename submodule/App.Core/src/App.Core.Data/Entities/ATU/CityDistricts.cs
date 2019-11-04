using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник районів міст")]
    [Display(Name = "Довідник районів міст")]
    [Table("Atu" + nameof(CityDistricts))]
    public class CityDistricts : BaseEntity
    {
	    [MaxLength(128)]
	    public string Name { get; set; }
	    public Guid CityId { get; set; }
        public City City { get; set; }

        [Range(0,99,ErrorMessage="Код повинен бути не більше 2 цифр")]
        public int Code { get; set; }
    }
}
