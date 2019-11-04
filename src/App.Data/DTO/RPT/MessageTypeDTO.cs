using System;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.RPT
{

    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MessageTypeDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        public string MessType { get; set; }
    }
}
