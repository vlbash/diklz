using System;
using App.Data.Entities.GWS;
using App.Data.Entities.WTT;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.REP
{
    public class WorkProgress: BaseEntity
    {
        public Guid ReportId { get; set; }

        public Report Report { get; set; }

        public Guid WorkTimetableId { get; set; }

        public WttWorkTimetable WorkTimetable { get; set; }

        public Guid GeneralWorksheetId { get; set; }

        public GeneralWorksheet GeneralWorksheet { get; set; }
    }
}
