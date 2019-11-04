using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using App.Core.Eq.Entities.Schedule;

namespace App.Core.Eq.Business
{
    public class TimeTableGenerator
    {
        private List<Schedule> _schedules = new List<Schedule>();
        private Schedule _settings;
        private List<DateTime> _listOfHolidays;

        public TimeTableGenerator(List<Schedule> schedules, Schedule settings, List<DateTime> listOfHolidays)
        {
            _schedules = schedules;
            _settings = settings;
            _listOfHolidays = listOfHolidays;
        }

        public List<ScheduleTime> Generate()
        {
            var res = new List<ScheduleTime>();

            DateTime currentDT = DateTime.MinValue;
            DateTime endGenerationDT = DateTime.MinValue;

            foreach (var item in _schedules)
            {
                // Checking work days in schedule period
                List<int> workDaysOfWeek = CheckWorkDays(item);

                currentDT = item.WorkDateFrom.Value;
                var tmpEndDate = item.WorkDateTo.Value;

                endGenerationDT = item.WorkDateTo.Value >= tmpEndDate ? tmpEndDate : item.WorkDateTo.Value;

                if (item.Repeat == Repeat.EveryDay)
                {
                    while (endGenerationDT >= currentDT)
                    {
                        var dayOfWeek = (int)currentDT.DayOfWeek;
                        if (!_settings.IsAllDay)
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek) && !_listOfHolidays.Contains(currentDT))
                            {
                                var newSchedTime = CreateScheduleTime(currentDT, item);
                                res.Add(newSchedTime);
                            }
                        }
                        else
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek))
                            {
                                var newSchedTime = CreateScheduleTime(currentDT, item);
                                res.Add(newSchedTime);
                            }
                        }
                        currentDT = currentDT.AddDays(1);
                    }
                }

                if (item.Repeat == Repeat.EveryCoupleDay)
                {
                    while (endGenerationDT >= currentDT)
                    {
                        var dayOfWeek = (int)currentDT.DayOfWeek;
                        if (!_settings.IsAllDay)
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek) && !_listOfHolidays.Contains(currentDT))
                            {
                                if (currentDT.Day % 2 == 0)
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                        }
                        else
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek))
                            {
                                if (currentDT.Day % 2 == 0)
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                        }
                        currentDT = currentDT.AddDays(1);
                    }
                }

                if (item.Repeat == Repeat.EveryOddDay)
                {
                    while (endGenerationDT >= currentDT)
                    {
                        var dayOfWeek = (int)currentDT.DayOfWeek;
                        if (!_settings.IsAllDay)
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek) && !_listOfHolidays.Contains(currentDT))
                            {
                                if (currentDT.Day % 2 == 1)
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                        }
                        else
                        {
                            if (workDaysOfWeek.Contains(dayOfWeek))
                            {
                                if (currentDT.Day % 2 == 1)
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                        }
                        currentDT = currentDT.AddDays(1);
                    }
                }

                if (item.Repeat == Repeat.EveryOddWeek)
                {
                    while (endGenerationDT >= currentDT)
                    {
                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        Calendar cal = dfi.Calendar;
                        var week = cal.GetWeekOfYear(currentDT, dfi.CalendarWeekRule,
                            dfi.FirstDayOfWeek);
                        if (week % 2 == 1)
                        {
                            var dayOfWeek = (int)currentDT.DayOfWeek;
                            if (!_settings.IsAllDay)
                            {
                                if (workDaysOfWeek.Contains(dayOfWeek) && !_listOfHolidays.Contains(currentDT))
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                            else
                            {
                                if (workDaysOfWeek.Contains(dayOfWeek))
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                        }
                        currentDT = currentDT.AddDays(1);
                    }
                }

                if (item.Repeat == Repeat.EveryCoupleWeek)
                {
                    while (endGenerationDT >= currentDT)
                    {
                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        Calendar cal = dfi.Calendar;
                        var week = cal.GetWeekOfYear(currentDT, dfi.CalendarWeekRule,
                            dfi.FirstDayOfWeek);
                        if (week % 2 == 0)
                        {
                            var dayOfWeek = (int)currentDT.DayOfWeek;
                            if (!_settings.IsAllDay)
                            {
                                if (workDaysOfWeek.Contains(dayOfWeek)&& !_listOfHolidays.Contains(currentDT))
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                            else
                            {
                                if (workDaysOfWeek.Contains(dayOfWeek))
                                {
                                    var newSchedTime = CreateScheduleTime(currentDT, item);
                                    res.Add(newSchedTime);
                                }
                            }
                            
                        }
                        currentDT = currentDT.AddDays(1);
                    }
                }
            }
            return res;
        }

        private ScheduleTime CreateScheduleTime(DateTime schedDT, Schedule scheduleItem)
        {
            ScheduleTime tmpSchedTime = new ScheduleTime();
            tmpSchedTime.ScheduleDate = schedDT;
            tmpSchedTime.ScheduleId = scheduleItem.Id;
            tmpSchedTime.ResourceId = scheduleItem.ResourceId;
            tmpSchedTime.RecordState = scheduleItem.RecordState;
            tmpSchedTime.WorkTimeDuration = scheduleItem.Duration;
            tmpSchedTime.WorkTimeFrom = scheduleItem.WorkTimeFrom.Value.TimeOfDay;
            tmpSchedTime.WorkTimeTo = scheduleItem.WorkTimeTo.Value.TimeOfDay;
            tmpSchedTime.BreakBetweenSlot = scheduleItem.BreakBetweenSlot;
            return tmpSchedTime;
        }

        private List<int> CheckWorkDays(Schedule schedule)
        {
            List<int> workDaysOfWeek = new List<int>();
            if (schedule.Day1) workDaysOfWeek.Add(1);
            if (schedule.Day2) workDaysOfWeek.Add(2);
            if (schedule.Day3) workDaysOfWeek.Add(3);
            if (schedule.Day4) workDaysOfWeek.Add(4);
            if (schedule.Day5) workDaysOfWeek.Add(5);
            if (schedule.Day6) workDaysOfWeek.Add(6);
            if (schedule.Day7) workDaysOfWeek.Add(0);

            return workDaysOfWeek;
        }
    }
}
