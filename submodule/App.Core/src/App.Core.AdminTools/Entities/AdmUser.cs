using System;
using App.Core.Data.Entities;
using System.Collections.Generic;
using App.Core.AdminTools.Entities.JoinTables;
using App.Core.Data.Entities.Common;

namespace App.Core.AdminTools.Entities
{
    public class AdmUser : BaseEntity
    {
        public string Name { get; set; }

        public Guid PersonId { get; set; }
        public Person Person { get; set; }

        public List<AdmUserAdmProfile> AdmProfiles { get; set; }
    }
}
