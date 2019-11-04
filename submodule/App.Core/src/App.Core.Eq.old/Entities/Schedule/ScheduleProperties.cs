using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Eq.Entities.Schedule
{
    [Table("ScheduleProperties")]
    public class ScheduleProperty : EqBaseEntity
    {
        public long ScheduleId { get; set; }

        [Column(TypeName = "jsonb")]
        public string Value { get; set; }
        public SchedulePropertyTypeModel ModelEnumId { get; set; }

        public long? OriginDbId { get; set; }
        public long? OriginDbRecordId { get; set; }
    }
}
