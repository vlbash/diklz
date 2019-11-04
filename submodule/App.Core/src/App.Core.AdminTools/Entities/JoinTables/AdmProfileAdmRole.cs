using System;
using App.Core.Data.Entities;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities.JoinTables
{
    public class AdmProfileAdmRole : BaseEntity
    {
        public Guid AdmProfileId { get; set; }
        public AdmProfile AdmProfile { get; set; }
        public Guid AdmRoleId { get; set; }
        public AdmRole AdmRole { get; set; }
    }
}
