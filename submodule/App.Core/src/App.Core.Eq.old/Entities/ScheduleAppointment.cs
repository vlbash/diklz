using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Eq.Entities
{
    public class ScheduleAppointment : EqBaseEntity
    {
        public DateTime AppDate { get; set; }
        public long ResourceId { get; set; }
        public long VisitorId { get; set; }
        public long? SlotId { get; set; }
        public long? OrgUnitId { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string Note { get; set; }
    }
}
