using System;
using App.Core.Data.Entities.Common;
using App.Data.Models.DOS;
using App.Data.Models.ORG;

namespace App.Data.Models.DOC
{
    public class BranchEDocument : BaseEntity
    {
            public Guid BranchId { get; set; }
            public Branch Branch { get; set; }

            public Guid EDocumentId { get; set; }
            public EDocument EDocument { get; set; }
    }
}
