using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.CDN;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    public class OrgUnitPositionDetailDTO : BaseDTO
    {
        public Guid OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
        public Guid OrgPositionId { get; set; }
        public string OrgPositionName { get; set; }


        public string OrgPositionType { get; set; }
        public string OrgPositionTypeName { get; set; }
        public bool IsResource { get; set; }

        [NotMapped]
        [DisplayName("Назва")]
        string Name => String.Format("{0}-{1}", OrgUnitName, OrgPositionName);
    }

    [RightsCheckList(nameof(OrgUnitPosition), nameof(Position))]
    public class OrgUnitPositionListDTO : BaseDTO
    {
        public Guid OrgUnitId { get; set; }
    }
}
