using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay("Документ")]
    public sealed class Document : BaseEntity, IDocument, IDerivableEntity
    {
        public string DerivedClass { get; set; }

        [DisplayName("Номер документу")]
        public string RegNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата")]
        public DateTime? RegDate { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }

    }
}
