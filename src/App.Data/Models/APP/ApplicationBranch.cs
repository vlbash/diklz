 using System;
using App.Core.Data.Entities.Common;
using App.Data.Models.ORG;

namespace App.Data.Models.APP
{
    public class ApplicationBranch: BaseEntity
    {
        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }

        public Guid LimsDocumentId { get; set; }
        public LimsDoc LimsDocument { get; set; }
    }
}
