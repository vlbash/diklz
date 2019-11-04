using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник адрес")]
    [Display(Name = "Довідник адрес")]
    [Table("Atu" + nameof(SubjectAddress))]
    public class SubjectAddress : BaseEntity
    {
        public Guid StreetId { get; set; }
        [MaxLength(20)]
        public string PostIndex { get; set; }
        [MaxLength(300)]
        public string Building { get; set; }
        public string AddressType { get; set; }
        public Guid SubjectId { get; set; }
    }
}
