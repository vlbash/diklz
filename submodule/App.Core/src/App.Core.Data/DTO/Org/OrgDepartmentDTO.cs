using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using System;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    [RightsCheckList(nameof(Department), nameof(Organization))]
    [RlsRight(nameof(Organization), nameof(ParentId))]
    public class OrgDepartmentDetailDTO: BaseDTO, IOrgUnitDetailDTO
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Parent { get; set; }
        public Guid? ParentId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    [RightsCheckList(nameof(Department))]
    public class OrgDepartmentListDTO: BaseDTO, IOrgUnitListDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }
    }
}
