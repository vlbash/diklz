using System;
using App.Core.Data.Entities.Common;
using App.Data.Models.ORG;

namespace App.Data.Models.APP
{
    public class AppAssigneeBranch: BaseEntity
    {
        public Guid AssigneeId { get; set; }
        public AppAssignee Assignee { get; set; }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
