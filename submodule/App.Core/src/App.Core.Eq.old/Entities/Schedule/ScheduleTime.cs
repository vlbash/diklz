using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Eq.Entities.Schedule
{
    [Table("ScheduleTime")]
    public class ScheduleTime : EqBaseEntity
    {
        public long ScheduleId { get; set; }
        public long ResourceId { get; set; }
        public long? OrgUnitId { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public TimeSpan? WorkTimeFrom { get; set; }
        public TimeSpan? WorkTimeTo { get; set; }
        public TimeSpan? WorkTimeDuration { get; set; }
        public TimeSpan? BreakBetweenSlot { get; set; }

        public long? OriginDbId { get; set; }
        public long? OriginDbRecordId { get; set; }

        [NotMapped]
        public DateTime? WorkDateTimeFrom => DateTime.Parse($"{ScheduleDate.Value.Date.ToString("yyyy-MM-dd")} {WorkTimeFrom.Value.ToString()}");

        [NotMapped]
        public DateTime? WorkDateTimeTo =>DateTime.Parse($"{ScheduleDate.Value.Date.ToString("yyyy-MM-dd")} {WorkTimeTo.Value.ToString()}");
    }
}
