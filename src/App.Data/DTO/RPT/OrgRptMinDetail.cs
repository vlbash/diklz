using System;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.RPT
{
    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class OrgRptNameField: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        public string OrgName { get; set; }
    }

    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class OrgUnitRptMinDetail: BaseDTO
    {
        public string MpdName { get; set; }

        public Guid OrgUnitId { get; set; }
    }

    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDAddressRptMinDetail: BaseDTO
    {
        public Guid MpdAddress { get; set; }

        public Guid OrgUnitId { get; set; }
    }

    public class OrgInnEdrpouRptMinDetail: BaseDTO
    {
        public string INN { get; set; }
        public string EDRPOU { get; set; }
    }
}
