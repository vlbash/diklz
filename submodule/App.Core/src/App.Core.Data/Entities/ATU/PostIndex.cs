using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник поштових індексів")]
    [Display(Name = "Довідник поштових індексів")]
    [Table("Atu" + nameof(PostIndex))]
    public class PostIndex: BaseEntity
    {
        public Guid CityId { get; set; }
        public City City { get; set; }
        public string PostIndexStr { get; set; }
    }
}
