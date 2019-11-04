using System.ComponentModel;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.SEC
{
    public class RoleListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Активовано")]
        [PredicateCase(PredicateOperation.Equals)]
        public bool IsActive { get; set; }
    }
}
