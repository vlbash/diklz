using System;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Eq.Data.Entities
{
    [Table("Eq" + nameof(ScheduleSlot))]
    public class ScheduleSlot: CoreEntity
    {
        public Guid ScheduleId { get; set; }
        public Guid PatientId { get; set; }

        public bool? IsConfirmed { get; set; }

        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }

        public Guid ParentId { get; set; }
    }
}
