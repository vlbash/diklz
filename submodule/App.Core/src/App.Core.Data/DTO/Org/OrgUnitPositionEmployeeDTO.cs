using App.Core.Data.DTO.Common;
using System;
using System.ComponentModel;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    [RightsCheckList(nameof(OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Employee), nameof(Person), 
        nameof(OrgUnit))]
    public class OrgUnitPositionEmployeeDetailDTO : BaseDTO
    {
        public Guid OrgUnitPositionId { get; set; }
        public string OrgUnitPositionName { get; set; }

        public Guid EmployeeId { get; set; }
        public string OrgEmployeeName { get; set; }
        public string OrgEmployeeMiddleName { get; set; }
        public string OrgEmployeeFullName { get; set; }
        public string OrgPositionCode { get; set; }
        public Guid PersonId { get; set; }

        [DisplayName("П.I.Б.")]
        public string OrgEmployeeFIO { get; set; }

        [DisplayName("П.I.Б.")]
        public string OrgEmployeeFIOShort { get; set; }

        public Guid OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
    }

    [RightsCheckList(nameof(OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Employee), nameof(Person),
        nameof(OrgUnit))]
    public class OrgUnitPositionEmployeeSelectDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }
    }
}
