using App.Core.Data.Entities.ORG;
using System;
using App.Core.Data.Entities;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities.JoinTables
{
    public class AdmProfileOrgUnit : BaseEntity
    {
        public Guid AdmProfileId { get; set; }
        public AdmProfile AdmProfile { get; set; }
        public Guid OrgUnitId { get; set; }
        public OrgUnit AdmOrgUnit { get; set; }
    }
}
