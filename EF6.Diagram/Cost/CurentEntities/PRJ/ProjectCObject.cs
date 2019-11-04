using System;
using Astum.Core.Data.Entities.Common;
using System.Collections.Generic;
using Astum.Core.Data.Entities.ATU;
using App.Data.Entities.Common;

namespace App.Data.Entities.PRJ
{
    public class ProjectCObject : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid ConstructionObjectId { get; set; }
        public ConstructionObject ConstructionObject { get; set; }

        public List<EntityExProperty> EntityExProperties { get; set; }
    }
}
