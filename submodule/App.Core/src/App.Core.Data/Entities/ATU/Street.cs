using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник вулиць")]
    [Display(Name = "Довідник вулиць")]
    [Table("Atu" + nameof(Street))]
    public class Street : BaseEntity
	{
	    public string Name { get; set; }
        public Guid CityId { get; set; }
		public City City { get; set; }
    }
}
