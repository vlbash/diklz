using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.DTM
{
    [AuditInclude]
    [AuditDisplay("DtmElement")]
    public class DtmElement : BaseEntity
    {
        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Ім'я")]
        public string Name { get; set; }

        [DisplayName("Тип елементу")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string ElementTypeCode { get; set; }



        [NotMapped]
        [DisplayName("Тип елементу")]
        public string ElementTypeName { get; set; }

        [NotMapped]
        [DataType(DataType.MultilineText)]
        [DisplayName("Ім'я (перелік)")]
        public string NameList { get; set; }

        [NotMapped]
        [DisplayName("Батьківський елемент")]
        public Guid? ParentId { get; set; }

        [NotMapped]
        [DisplayName("Шаблон")]
        public Guid? TemplateId { get; set; }

        [NotMapped]
        [DisplayName("Элемент (инфо)")]
        public string Info
        {
            get => string.Format("{0} (Id:{1}, Code:{2}, TypeCode:{3})", Name, Id, Code, ElementTypeCode);
            set { }
        }
    }
}
