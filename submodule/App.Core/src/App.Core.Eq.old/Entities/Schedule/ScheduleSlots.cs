using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace App.Core.Eq.Entities.Schedule
{
    [Table("Slots")]
    public class ScheduleSlots : EqBaseEntity
    {
        public SlotType Type { get; set; }
        public long ScheduleTimetableId { get; set; }
        public long ResourceId { get; set; }
        public long OrgUnitId { get; set; }
        public long? OwnerId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }

        [NotMapped]
        public TimeSpan? Duration => TimeTo.Subtract(TimeFrom);

        [NotMapped]
        public DateTime? WorkDateTimeFrom
        {
            get
            {
                return DateTime.ParseExact($"{Date.ToString("dd-MM-yy")} {TimeFrom.ToString(@"hh\:mm\:ss")}", "dd-MM-yy HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        [NotMapped]
        public DateTime? WorkDateTimeTo
        {
            get
            {
                return DateTime.ParseExact($"{Date.ToString("dd-MM-yy")} {TimeTo.ToString(@"hh\:mm\:ss")}", "dd-MM-yy HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        public static bool operator ==(ScheduleSlots s1, ScheduleSlots s2)
        {
            if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null))
                return true;
            else if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null))
                return false;

            var retValue = s1.WorkDateTimeFrom.Equals(s2.WorkDateTimeFrom) && s1.WorkDateTimeTo.Equals(s2.WorkDateTimeTo);
            return retValue;
        }

        public static bool operator !=(ScheduleSlots s1, ScheduleSlots s2)
        {

            if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null))
                return false;
            else if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null))
                return true;

            var retValue = !s1.WorkDateTimeFrom.Equals(s2.WorkDateTimeFrom) || !s1.WorkDateTimeTo.Equals(s2.WorkDateTimeTo);
            return retValue;
        }
    }
}
