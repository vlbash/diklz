using System;
using App.Core.Data.Entities.Common;
using App.Data.Models.ORG;

namespace App.Data.Models.PRL
{
    public class PrlBranchContractor: BaseEntity
    {
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }

        public Guid ContractorId { get; set; }
        public PrlContractor Contractor { get; set; }
    }
}
