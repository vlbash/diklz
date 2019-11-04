using System;
using System.ComponentModel;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.SEC
{
    public class ProfileRoleListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid RoleId { get; set; }

        public Guid ProfileId { get; set; }

        [DisplayName("Активовано")]
        public bool IsActive { get; set; }
    }
}
