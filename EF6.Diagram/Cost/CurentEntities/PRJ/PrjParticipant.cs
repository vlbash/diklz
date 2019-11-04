using Astum.Core.Data.Entities;
using System;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Entities.ORG;

namespace App.Data.Entities.PRJ
{
    public class PrjParticipant : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        //роль з "Класифікатор ролей" ParticipantRole
        public string ParticipantRoleEnum { get; set; } //enum

        public Guid OrgUnitId { get; set; }
        public OrgUnit OrgUnit { get; set; }

        public Guid? OrgEmployeeId { get; set; }
        public OrgEmployee OrgEmployee { get; set; }

        public string Comment { get; set; }
    }
}
