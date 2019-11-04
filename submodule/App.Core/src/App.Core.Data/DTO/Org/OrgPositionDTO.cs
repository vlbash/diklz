using System.ComponentModel;
using System;
using App.Core.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    public class OrgPositionDetailDTO : BaseDTO
    {
        [DisplayName("Назва")]
        public string Name { get; set; }
    }

    [RightsCheckList(nameof(OrgUnitPosition))]
    [RlsRight(nameof(Organization), nameof(OrganizationId))]
    public class OrgPositionListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
    
        [Required]
        [DisplayName("Назва")]
        public string Name { get; set; }

        public Guid OrganizationId { get; set; }
    }
}

