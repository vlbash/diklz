using System.ComponentModel;
using App.Core.Base;
using App.Core.Data.Attributes;

namespace App.Data.DTO.SEC
{
    public class ApplicationRowLevelRightDTO: CoreDTO
    {
        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Назва")]
        public string EntityName { get; set; }

        [DisplayName("Активовано")]
        [PredicateCase(PredicateOperation.Equals)]
        public bool IsActive { get; set; }
    }
}
