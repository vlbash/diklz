using System;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Eq.Data.Entities
{
    [Table("Eq" + nameof(Schedule))]
    public class Schedule: CoreEntity
    {
        public Guid ResourceId { get; set; }
        public string ResourceTypeCode { get; set; }

        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan BreakBetweenSlot { get; set; }
        public TimeSpan? LunchBreakFrom { get; set; }
        public TimeSpan? LunchBreakTo { get; set; }
        public TimeSpan? ReserveFrom { get; set; }
        public TimeSpan? ReserveTo { get; set; }

        public bool Day1 { get; set; }
        public bool Day2 { get; set; }
        public bool Day3 { get; set; }
        public bool Day4 { get; set; }
        public bool Day5 { get; set; }
        public bool Day6 { get; set; }
        public bool Day7 { get; set; }
        public bool IsAllDay { get; set; }

        public string ServiceList { get; set; }
        public bool NeedConfirm { get; set; }

        public string ScheduleTypeCode { get; set; }
        public string SchedulePeriodTypeCode { get; set; }

        public string Note { get; set; }
    }
}
