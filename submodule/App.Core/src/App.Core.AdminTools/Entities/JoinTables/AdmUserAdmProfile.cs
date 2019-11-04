using System;
using App.Core.Data.Entities;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities.JoinTables
{
    public class AdmUserAdmProfile : BaseEntity
    {
        public Guid AdmUserId { get; set; }
        public AdmUser AdmUser { get; set; }
        public Guid AdmProfileId { get; set; }
        public AdmProfile AdmProfile { get; set; }
    }
}
