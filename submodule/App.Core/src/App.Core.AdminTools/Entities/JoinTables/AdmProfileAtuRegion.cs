using App.Core.Data.Entities.ATU;
using System;
using App.Core.Data.Entities;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities.JoinTables
{
    public class AdmProfileAtuRegion : BaseEntity
    {
        public Guid AdmProfileId { get; set; }
        public AdmProfile AdmProfile { get; set; }
        public Guid AtuRegionId { get; set; }
        public AtuRegion AtuRegion { get; set; }
    }
}
