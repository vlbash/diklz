using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(FieldRight))]
    [Display(Name = "Права на конкретне поле сутності")]
    public class FieldRight: CoreEntity
    {
        public Guid RightId { get; set; }
        public Right Right { get; set; }
        public string FieldName { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
