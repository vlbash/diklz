using App.Data.Entities.Common;
using App.Data.Entities.CPL;
using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Entities.ORG;
using System;
using System.Collections.Generic;

namespace App.Data.Entities.GWS
{
    public class GeneralWorksheet : BaseEntity
    {
        public string Number { get; set; }

        public DateTime WorkSheetDate { get; set; }

        //Стан документу з "Класифікатор станів документів" DocStatus
        public string WorkSheetDocStatusEnum { get; set; }

        //календарний план
        public Guid CplId { get; set; }

        public CplCalendarPlan Cpl { get; set; }

        //ресурсна відомість - ???

        //відповідальна особа
        public Guid OrgEmployeeId { get; set; }

        public OrgEmployee OrgEmployee { get; set; }

        public Guid ConstructionObjectId { get; set; }

        public ConstructionObject ConstructionObject { get; set; }

        public string Description { get; set; }

        public List<GwsPerformanceReport> PerformanceReports { get; set; }
    }
}
