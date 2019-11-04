using System;
using App.Data.Entities.CPL;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.WTT
{
    public class WttNetworkTimetable: BaseEntity
    {
        public string WorkNumber { get; set; }
        public DateTime WorkDate { get; set; }
        public Guid WorkTimetableId { get; set; }
        public WttWorkTimetable WorkTimetable { get; set; }
        public Guid CalendarPlanWorkId { get; set; }
        public CplCalendarPlanWork CalendarPlanWork { get; set; }
        public double NumberOfUnits { get; set; }
        public double TargetValue { get; set; }
    }
}
