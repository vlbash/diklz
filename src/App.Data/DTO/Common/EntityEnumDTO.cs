using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.APP;

namespace App.Data.DTO.Common
{
    public class EntityEnumDTO: BaseDTO
    {
        public Guid? ApplicationId { get; set; }
        public Guid? BranchId { get; set; }
        public Guid EnumRecordId { get; set; }
        public string EntityType { get; set; }
        public string EnumCode { get; set; }
        public string EnumName { get; set; }
        public string ExParam1 { get; set; }
        public string ExParam2 { get; set; }
    }
}
