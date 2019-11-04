using System;
using App.Core.Data.Entities;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities.JoinTables
{
    public class AdmRoleAdmRight : BaseEntity
    {
        public Guid AdmRoleId { get; set; }
        public AdmRole AdmRole { get; set; }
        public Guid AdmRightId { get; set; }
        public AdmRight AdmRight { get; set; }
    }
}
