using System;
using System.ComponentModel;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;

namespace App.Data.DTO.SEC
{
    public class RoleRightListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid RoleId { get; set; }

        public Guid RightId { get; set; }
        
        [DisplayName("Активовано")]
        [PredicateCase(PredicateOperation.Equals)]
        public bool IsActive { get; set; }
        
        [DisplayName("Рівень доступу")]
        [PredicateCase(PredicateOperation.Equals)]
        public EntityAccessLevel EntityAccessLevel { get; set; } = EntityAccessLevel.No;
    }
}
