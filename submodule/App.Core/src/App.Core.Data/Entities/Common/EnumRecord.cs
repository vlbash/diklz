using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay("Перерахування")]
    [Display(Name = "Перерахування")]
    public class EnumRecord : BaseEntity, IEnumRecord
    {
        [DisplayName("Тип")]
        public string EnumType { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Значення")]
        public string Name { get; set; }

        public string ExParam1 { get; set; }
        public string ExParam2 { get; set; }
    }
}
