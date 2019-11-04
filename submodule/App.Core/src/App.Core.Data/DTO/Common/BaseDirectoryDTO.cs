using App.Core.Data.Attributes;

namespace App.Core.Data.DTO.Common
{
    public class BaseDirectoryDTO : BaseDTO
    {
        [PredicateCase(PredicateOperation.Contains)]
        public string Code { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }
    }
}
