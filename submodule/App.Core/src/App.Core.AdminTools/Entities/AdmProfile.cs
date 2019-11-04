using App.Core.AdminTools.Entities.JoinTables;
using App.Core.Data.Entities;
using System.Collections.Generic;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities
{
    public class AdmProfile : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public List<AdmUserAdmProfile> AdmUsers { get; set; }
        public List<AdmProfileAtuRegion> Regions { get; set; }
        public List<AdmProfileAdmRole> Roles { get; set; }
        public List<AdmProfileOrgUnit> Owners { get; set; }
    }
}
