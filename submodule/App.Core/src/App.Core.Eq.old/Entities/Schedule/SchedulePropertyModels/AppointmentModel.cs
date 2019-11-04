using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace App.Core.Eq.Entities.Schedule.SchedulePropertyModels
{
    public class AppointmentModel
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public string Comments { get; set; }

        [XmlIgnore]
        public long? ResourceId { get; set; }

        [XmlIgnore]
        public long? ScheduleTimetableId { get; set; }

        public ScheduleSlots ToSlot()
        {
            ScheduleSlots tmpSlot = new ScheduleSlots
            {
                Date = DateTimeFrom.Date,
                TimeFrom = DateTimeFrom.TimeOfDay,
                TimeTo = DateTimeTo.TimeOfDay,
                Type = SlotType.Break
            };
            //tmpSlot.RecordStateCode = "N";
            return tmpSlot;
        }
        public string ToJson()
        {
            var jsonRes = JsonConvert.SerializeObject(this);
            return jsonRes;
        }

        public AppointmentModel FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return this;
            }

            var desRes = JsonConvert.DeserializeObject<AppointmentModel>(json);
            this.DateTimeFrom = desRes.DateTimeFrom;
            this.DateTimeTo = desRes.DateTimeTo;
            this.Comments = desRes.Comments;

            return this;
        }


    }
}
