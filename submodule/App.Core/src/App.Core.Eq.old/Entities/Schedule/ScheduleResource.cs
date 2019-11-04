using System.ComponentModel.DataAnnotations;

namespace App.Core.Eq.Entities.Schedule
{
    public class ScheduleResource : EqBaseEntity
    {
        public long EntityRecordId { get; set; }
        [MaxLength(256)]
        public string EntityName { get; set; }

        public long? OrgUnitId { get; set; }

        public long? OrgEmployeeId { get; set; }
    }
}
