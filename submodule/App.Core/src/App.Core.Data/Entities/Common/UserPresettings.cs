using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.Entities.Common
{
    [Display(Name = "Попередні налаштування користувача")]
    public class UserPresettings: BaseEntity
    {
        public string User { get; set; }

        public string JournalName { get; set; }

        public string PresettingsJson { get; set; }
    }
}
