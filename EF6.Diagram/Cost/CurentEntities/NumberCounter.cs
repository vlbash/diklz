using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay(name: "Лічильник вхідник/вихідних номерів документів")]
    public class NumberCounter : BaseEntity
    {
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Сутність")]
        public string EntityName { get; set; }
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Тип нумерації")]
        public string CounterType { get; set; } 
        [DisplayName("Шаблон номеру")]
        public string Pattern { get; set; }
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер")]
        public string Value { get; set; } 
    }
}
