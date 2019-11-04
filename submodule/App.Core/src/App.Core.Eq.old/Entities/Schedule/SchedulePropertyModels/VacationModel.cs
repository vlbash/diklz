using System;
using System.Xml.Serialization;
using App.Core.Data.Enums;
using Newtonsoft.Json;

namespace App.Core.Eq.Entities.Schedule.SchedulePropertyModels
{
    public class VacationModel
    {
        public DateTime BreakTimeFrom { get; set; }
        public DateTime BreakTimeTo { get; set; }
        public string Comments { get; set; }

        [XmlIgnore]
        public long? ResourceId { get; set; }

        [XmlIgnore]
        public long? ScheduleTimeId { get; set; }

        public ScheduleSlots ToSlot()
        {
            ScheduleSlots tmpSlot = new ScheduleSlots
            {
                Date = BreakTimeFrom.Date,
                TimeFrom = BreakTimeFrom.TimeOfDay,
                TimeTo = BreakTimeTo.TimeOfDay,
                Type = SlotType.Vacation,
                RecordState = RecordState.N
            };
            //tmpSlot.RecordStateCode = "N";
            if (ResourceId.HasValue)
            {
                tmpSlot.ResourceId = ResourceId.Value;
            }

            return tmpSlot;
        }
        public string ToJson()
        {
            var jsonRes = JsonConvert.SerializeObject(this);
            return jsonRes;
        }

        public VacationModel FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return this;
            }

            var desRes = JsonConvert.DeserializeObject<BreakModel>(json);
            this.BreakTimeFrom = desRes.BreakTimeFrom;
            this.BreakTimeTo = desRes.BreakTimeTo;
            this.Comments = desRes.Comments;

            return this;
        }
    }
}
