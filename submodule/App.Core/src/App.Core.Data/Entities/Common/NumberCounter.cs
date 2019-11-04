using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Enums;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay(name: "Лічильник вхідник/вихідних номерів документів")]
    [Display(Name = "Лічильник вхідник/вихідних номерів документів")]
    public class NumberCounter : BaseEntity
    {
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Сутність")]
        public string EntityName { get; set; }

        [DisplayName("Тип нумерації")]
        public RegNumberCounterType CounterType { get; set; }

        [DisplayName("Шаблон номеру")]
        public NumberCounterPattern Pattern { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер")]
        public string Value { get; set; }
    }
}
