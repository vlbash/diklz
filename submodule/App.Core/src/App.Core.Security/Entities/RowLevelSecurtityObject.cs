using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(RowLevelSecurityObject))]
    [Display(Name = "Об'єкт рівня доступу")]
    public class RowLevelSecurityObject: CoreEntity
    {
        public Guid RowLevelRightId { get; set; }
        public RowLevelRight RowLevelRight { get; set; }
        public Guid EntityId { get; set; }
    }
}
