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
    public class CurrentLicenseDTO: BaseDTO
    {
        public bool IsRelevant { get; set; }
        public string LicState { get; set; }
        public Guid OrgUnitId { get; set; }
        public string LicType { get; set; }
        public Guid LicenseParentId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
