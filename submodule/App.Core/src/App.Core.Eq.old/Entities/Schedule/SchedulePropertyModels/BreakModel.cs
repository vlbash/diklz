using System;
using System.Xml.Serialization;
using App.Core.Data.Enums;
using Newtonsoft.Json;

namespace App.Core.Eq.Entities.Schedule.SchedulePropertyModels
{
    public class BreakModel
    {
        public DateTime BreakTimeTo { get; set; }
        public DateTime BreakTimeFrom { get; set; }
        public string Comments { get; set; }

        [XmlIgnore]
        public long? ResourceId { get; set; }

        [XmlIgnore]
        public long? ScheduleTimeId { get; set; }


        public ScheduleSlots ToSlot()
        {
            ScheduleSlots tmpSlot = new ScheduleSlots();
            tmpSlot.Date = BreakTimeFrom.Date;
            tmpSlot.TimeFrom = BreakTimeFrom.TimeOfDay;
            tmpSlot.TimeTo = BreakTimeTo.TimeOfDay;
            tmpSlot.Type = SlotType.Break;
            tmpSlot.RecordState = RecordState.N;
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

        public BreakModel FromJson(string json)
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
