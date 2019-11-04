using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Data.Entities.Common
{
    [Table("Sys" + nameof(ApplicationSetting))]
    [Display(Name = "Існуюючі системні налаштування")]
    public class ApplicationSetting: CoreEntity
    {
        [DisplayName("Назва")]
        public override string Caption { get; set; }

        [DisplayName("Налаштування")]
        public string Name { get; set; }

        [DisplayName("Внутрішній тип")]
        public string Type { get; set; }

        public string TypeName { get; set; }

        [DisplayName("Увімкнено")]
        public bool IsEnabled { get; set; }
    }
}
