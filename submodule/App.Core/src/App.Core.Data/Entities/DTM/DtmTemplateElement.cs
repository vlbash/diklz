using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.DTM
{
    [AuditInclude]
    [AuditDisplay("DtmTemplateElement")]
    [Display(Name = "Елемент шаблона")]
    public class DtmTemplateElement : BaseEntity
    {
        [DisplayName("Порядок сортування")]
        public double SortOrder { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Шаблон")]
        public Guid TemplateId { get; set; }
        [DisplayName("Шаблон")]
        public DtmTemplate Template { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Елемент")]
        public Guid ElementId { get; set; }
        [DisplayName("Елемент")]
        public DtmElement Element { get; set; }

        [DisplayName("Батьківський елемент")]
        public Guid? ParentId { get; set; }
        [DisplayName("Батьківський елемент")]
        public DtmTemplateElement Parent { get; set; }

        [NotMapped]
        [DisplayName("Ім'я")]
        public string Name => Template?.Name + " : " + Parent?.Element.Name + " / " + Element?.Name;
    }
}
