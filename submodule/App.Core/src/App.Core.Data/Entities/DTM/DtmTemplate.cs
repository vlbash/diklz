using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.DTM
{
    [AuditInclude]
    [AuditDisplay("DtmTemplate")]
    [Display(Name = "Доповнення шаблону документа")]
    public class DtmTemplate : BaseEntity
    {
        [DisplayName("Код")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Code { get; set; }

        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }
    }
}
