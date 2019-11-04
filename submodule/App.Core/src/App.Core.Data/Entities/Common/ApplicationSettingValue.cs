using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Data.Entities.Common
{
    [Table("Sys" + nameof(ApplicationSettingValue))]
    [Display(Name = "Системні налаштування")]
    public class ApplicationSettingValue: CoreEntity
    {
        public Guid ApplicationSettingId { get; set; }
        public ApplicationSetting ApplicationSetting { get; set; }

        public string Value { get; set; }
    }
}
