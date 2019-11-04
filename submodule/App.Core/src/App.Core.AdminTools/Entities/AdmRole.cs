using App.Core.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Core.AdminTools.Entities.JoinTables;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities
{
    public class AdmRole : BaseEntity
    {
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }
        public bool IsActive { get; set; }

        public List<AdmProfileAdmRole> Profiles { get; set; }
        public List<AdmRoleAdmRight> Rights { get; set; }
    }
}
