using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Data.Enums;
using App.Core.Eq.Entities.Schedule;

namespace App.Core.Eq.Business
{
    public class SlotGenerator
    {
        private List<ScheduleTime> _scheduleTimeData { get; set; }
        private Schedule _slotSettings { get; set; }
        private List<ScheduleSlots> _existedSlots { get; set; }
        private List<ScheduleSlots> _returnSlots;

        public SlotGenerator(List<ScheduleTime> schedTime, Schedule slotSettings, List<ScheduleSlots> existedSlots)
        {
            _existedSlots = existedSlots;
            _scheduleTimeData = schedTime;
            _slotSettings = slotSettings;
            _returnSlots = new List<ScheduleSlots>(); //Just Init
        }

        public List<ScheduleSlots> Generate()
        {
            TimeSpan slotLenght = _slotSettings.Duration.Value;

            DateTime startSlotDateTime = DateTime.MinValue.Date; // Just init
            DateTime endSlotDateTime = DateTime.MinValue.Date; // Just init

            foreach (var item in _scheduleTimeData)
            {
                startSlotDateTime = item.WorkDateTimeFrom.Value;
                endSlotDateTime = DateTime.Parse($"{startSlotDateTime.Date.ToShortDateString()} 00:00:00");
                var tmpEndSlotTime = startSlotDateTime.Add(slotLenght);

                endSlotDateTime = item.WorkTimeTo.Value <= tmpEndSlotTime.TimeOfDay ? item.WorkDateTimeTo.Value : tmpEndSlotTime;

                if (!_slotSettings.IsAllDay)
                {
                    if (endSlotDateTime.TimeOfDay < item.WorkTimeTo.Value)
                    {
                        //endSlotDateTime = new DateTime();
                        var tmpNewSlots = GetSlots(startSlotDateTime, endSlotDateTime, item, item.Id, null);
                        _returnSlots.AddRange(tmpNewSlots);
                        //Take last item and use TimeTo for next slot
                        var tmpLastSlot = tmpNewSlots[tmpNewSlots.Count - 1];
                        startSlotDateTime = tmpLastSlot.WorkDateTimeTo.Value;
                        var timeInterval = new TimeSpan(slotLenght.Ticks);
                        endSlotDateTime = startSlotDateTime.Add(timeInterval);// (slotLenght);
                    }
                    while (endSlotDateTime.TimeOfDay < item.WorkTimeTo.Value)
                    {
                        //endSlotDateTime = new DateTime();
                        var tmpNewSlots = GetSlots(startSlotDateTime, endSlotDateTime, item, item.Id, item.BreakBetweenSlot);
                        _returnSlots.AddRange(tmpNewSlots);
                        //Take last item and use TimeTo for next slot
                        var tmpLastSlot = tmpNewSlots[tmpNewSlots.Count - 1];
                        startSlotDateTime = tmpLastSlot.WorkDateTimeTo.Value;
                        var timeInterval = new TimeSpan(slotLenght.Ticks);
                        endSlotDateTime = startSlotDateTime.Add(timeInterval);// (slotLenght);
                    }
                    if (endSlotDateTime.TimeOfDay == item.WorkTimeTo.Value)
                    {
                        //endSlotDateTime = new DateTime();
                        var tmpNewSlots = GetSlots(startSlotDateTime, endSlotDateTime, item, item.Id, null);
                        _returnSlots.AddRange(tmpNewSlots);
                        //Take last item and use TimeTo for next slot
                        var tmpLastSlot = tmpNewSlots[tmpNewSlots.Count - 1];
                        startSlotDateTime = tmpLastSlot.WorkDateTimeTo.Value;
                        var timeInterval = new TimeSpan(slotLenght.Ticks);
                        endSlotDateTime = startSlotDateTime.Add(timeInterval);// (slotLenght);
                    }
                }
                else
                {
                    while (endSlotDateTime.TimeOfDay < item.WorkTimeTo.Value)
                    {
                        var tmpNewSlots = GetSlots(startSlotDateTime, endSlotDateTime, item, item.Id, item.BreakBetweenSlot);
                        _returnSlots.AddRange(tmpNewSlots);

                        //Take last item and use TimeTo for next slot
                        var tmpLastSlot = tmpNewSlots[tmpNewSlots.Count - 1];
                        startSlotDateTime = tmpLastSlot.WorkDateTimeTo.Value;
                        endSlotDateTime = startSlotDateTime.Add(slotLenght);
                        if (endSlotDateTime.Date > startSlotDateTime.Date)
                        {
                             tmpNewSlots = GetSlots(startSlotDateTime, DateTime.Parse("23:59:59"), item, item.Id, item.BreakBetweenSlot);
                            _returnSlots.AddRange(tmpNewSlots);
                            break;
                        }
                    }
                }
            }
            return _returnSlots;
        }


        private List<ScheduleSlots> GetSlots(DateTime timeFrom, DateTime timeTo, ScheduleTime scheduleTime, long? timeTableId, TimeSpan? breakBetweenSlot)
        {

            List<ScheduleSlots> retSlot = new List<ScheduleSlots>();

            #region [from] DateEntry
            //Check start path entrying
            var tryToFindExistSlot = _existedSlots.Find(x =>
                                      (x.TimeFrom >= timeFrom.TimeOfDay && x.TimeFrom <= timeTo.TimeOfDay) && x.TimeTo >= timeTo.TimeOfDay
                                      && x.ResourceId == scheduleTime.ResourceId);

            if (tryToFindExistSlot != null)
            {
                //Create pre slot 
                var startPS = timeFrom;
                var endPS = DateTime.Parse($"{startPS.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeFrom.Value.TimeOfDay}");

                if (startPS.TimeOfDay < endPS.TimeOfDay)
                {
                    var tmpLS = CreateSlot(startPS, endPS, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, null);
                    retSlot.Add(tmpLS);
                }

                //Create exist Slot
                var startExistSlot = endPS;
                var endExistSlot = DateTime.Parse($"{endPS.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeTo.Value.TimeOfDay}");

                var tmpES = CreateSlot(startExistSlot, endExistSlot, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, tryToFindExistSlot.Type, timeTableId.Value, null);
                retSlot.Add(tmpES);

                var newStartSlotTime = endExistSlot;
                var tmpEndSlotTime = timeFrom;
                while (tmpEndSlotTime <= newStartSlotTime)
                {
                    tmpEndSlotTime = tmpEndSlotTime.Add(_slotSettings.Duration.Value);
                }
                //Create postSlot
                startPS = newStartSlotTime;
                endPS = tmpEndSlotTime;

                var tmpPS = CreateSlot(startPS, endPS, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, null);
                retSlot.Add(tmpPS);
                //}

                return retSlot;
            }
            #endregion

            #region allDateEntry
            //Check all date path entrying
            tryToFindExistSlot = _existedSlots.Find(x =>
                                      (
                                      x.TimeFrom >= timeFrom.TimeOfDay && x.TimeFrom < timeTo.TimeOfDay) && x.TimeTo <= timeTo.TimeOfDay
                                      && x.ResourceId == scheduleTime.ResourceId
                                      );

            if (tryToFindExistSlot != null)
            {
                //Create pre Slot
                var startPS = timeFrom;
                var endPS = DateTime.Parse($"{timeFrom.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeFrom.Value.TimeOfDay}");

                ScheduleSlots tmpPS;
                if (startPS < endPS)
                {
                    tmpPS = CreateSlot(startPS, endPS, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, null);
                    retSlot.Add(tmpPS);
                }

                //Create exist slot
                var startExistSlot = endPS;
                var endExistSlot = DateTime.Parse($"{timeFrom.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeTo.Value.TimeOfDay}");

                var tmpES = CreateSlot(startExistSlot, endExistSlot, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, tryToFindExistSlot.Type, timeTableId.Value, null);
                retSlot.Add(tmpES);

                var newStartSlotTime = endExistSlot;
                var tmpEndSlotTime = timeFrom;
                while (tmpEndSlotTime <= newStartSlotTime)
                {
                    tmpEndSlotTime = tmpEndSlotTime.Add(_slotSettings.Duration.Value);
                }
                //Create postSlot
                startPS = newStartSlotTime;
                endPS = tmpEndSlotTime;

                tmpPS = CreateSlot(startPS, endPS, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, null);
                retSlot.Add(tmpPS);
                //}

                return retSlot;
            }

            #endregion
            #region [to] DateEntrying
            //Check end path entrying
            tryToFindExistSlot = _existedSlots.Find(x =>
                                      (x.TimeFrom <= timeFrom.TimeOfDay)
                                      && x.TimeTo > timeFrom.TimeOfDay && x.TimeTo <= timeTo.TimeOfDay
                                      && x.ResourceId == scheduleTime.ResourceId
                                      );

            if (tryToFindExistSlot != null)
            {
                //Create exist slot
                var startExistSlot = DateTime.Parse($"{timeFrom.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeFrom.Value.TimeOfDay}");
                var endExistSlot = DateTime.Parse($"{timeFrom.ToShortDateString()} {tryToFindExistSlot.WorkDateTimeTo.Value.TimeOfDay}");

                var tmpES = CreateSlot(startExistSlot, endExistSlot, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, tryToFindExistSlot.Type, timeTableId.Value, null);
                retSlot.Add(tmpES);

                var newStartSlotTime = endExistSlot;
                var tmpEndSlotTime = timeFrom;
                while (tmpEndSlotTime <= newStartSlotTime)
                {
                    tmpEndSlotTime = tmpEndSlotTime.Add(_slotSettings.Duration.Value);
                }
                //Create postSlot
                var startPS = newStartSlotTime;
                var endPS = tmpEndSlotTime;

                var tmpPS = CreateSlot(startPS, endPS, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, null);
                retSlot.Add(tmpPS);
                //}

            }
            #endregion

            if (retSlot.Count == 0)
            {
                var tmpPS = CreateSlot(timeFrom, timeTo, scheduleTime.ScheduleId, scheduleTime.ResourceId, scheduleTime.RecordState, SlotType.Shedule, timeTableId.Value, breakBetweenSlot);
                retSlot.Add(tmpPS);
            }
            return retSlot;
        }

        private ScheduleSlots CreateSlot(DateTime timeFrom, DateTime timeTo, long scheduleId, long resourceId, RecordState recordStateCode, SlotType sType, long timeTableId, TimeSpan? breakBetweenSpan)
        {
            if (breakBetweenSpan.HasValue)
            {
                timeFrom = timeFrom + breakBetweenSpan.Value;
                timeTo = timeTo + breakBetweenSpan.Value;
            }
            var ret = new ScheduleSlots();
            ret.TimeFrom = timeFrom.TimeOfDay;
            ret.TimeTo = timeTo.TimeOfDay;
            ret.ScheduleTimetableId = timeTableId;
            ret.Date = timeFrom.Date;
            ret.ResourceId = resourceId;
            ret.RecordState = recordStateCode;
            ret.Type = sType;
            return ret;
        }
    }
}
