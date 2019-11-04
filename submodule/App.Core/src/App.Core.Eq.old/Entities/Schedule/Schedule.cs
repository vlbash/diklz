using System;

namespace App.Core.Eq.Entities.Schedule
{
    public class Schedule : EqBaseEntity
    {
        public long? OwnerId { get; set; }
        public long ResourceId { get; set; }
        public DateTime? WorkDateFrom { get; set; }
        public DateTime? WorkDateTo { get; set; }
        public long ScheduleTypeId { get; set; }
        public bool Day1 { get; set; }
        public bool Day2 { get; set; }
        public bool Day3 { get; set; }
        public bool Day4 { get; set; }
        public bool Day5 { get; set; }
        public bool Day6 { get; set; }
        public bool Day7 { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime? WorkTimeFrom { get; set; }
        public DateTime? WorkTimeTo { get; set; }
        public TimeSpan? Duration { get; set; }
        public long? SchedulePeriodTypeId { get; set; }
        public string Note { get; set; }
        public long? OriginDbId { get; set; }
        public long? OriginDbRecordId { get; set; }
        public Repeat Repeat { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public TimeSpan? BreakBetweenSlot { get; set; }
    }
}
