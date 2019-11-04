using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(RowLevelRight))]
    [Display(Name = "Рівень доступу")]
    public class RowLevelRight: CoreEntity
    {
        public Guid ProfileId { get; set; }
        public string EntityName { get; set; }
        public RowLevelAccessType AccessType { get; set; }
        public List<RowLevelSecurityObject> RowLevelSecurityObjects { get; set; }
    }
}
