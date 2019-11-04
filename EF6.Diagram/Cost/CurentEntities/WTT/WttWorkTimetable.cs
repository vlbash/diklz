using System;
using System.Collections.Generic;
using App.Data.Entities.CPL;
using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Interfaces;

namespace App.Data.Entities.WTT
{
    public class WttWorkTimetable: BaseEntity, IDocument {

        public string RegNumber { get; set; }
        public DateTime? RegDate { get; set; }
        public string DocStateCode { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid CalendarPlanId { get; set; }
        public CplCalendarPlan CalendarPlan { get; set; }
        public List<WttNetworkTimetable> NetworkTimetable { get; set; }
    }
}
