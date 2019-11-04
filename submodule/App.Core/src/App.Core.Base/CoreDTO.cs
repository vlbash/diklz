using App.Core.Data.Attributes;
using System;
using System.ComponentModel;

namespace App.Core.Base
{
    public abstract class CoreDTO
    {
        [PredicateCase(PredicateOperation.Equals)]
        public Guid Id { get; set; } = Guid.Empty;

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Назва")]
        public string Caption { get; set; }
    }
}
