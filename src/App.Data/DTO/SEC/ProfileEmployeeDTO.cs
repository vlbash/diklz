using System;
using System.ComponentModel;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.SEC
{
    public class ProfileEmployeeListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid UserId { get; set; }

        public Guid ProfileId { get; set; }
        
        [DisplayName("Назва організації")]
        public string Organization { get; set; }

        [DisplayName("Активовано")]
        public bool IsActive { get; set; }

    }

    public class ProfileEmployeeListMinDTO: BaseDTO
    {
        public string UserId { get; set; }

        public string Name { get; set; }
    }
}
