using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(ApplicationRowLevelRight))]
    [Display(Name = "Рівень доступу")]
    public class ApplicationRowLevelRight: CoreEntity
    {
        public string EntityName { get; set; }
        public bool IsActive { get; set; }
    }
}
