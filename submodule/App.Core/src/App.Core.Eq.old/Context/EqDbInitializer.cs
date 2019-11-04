using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Core.Data.Enums;
using App.Core.Eq.Entities.Schedule;


namespace App.Core.Eq.Context
{
    public static class EqDbInitializer
    {
        public static void InitializeEq(EqContext context)
        {
            context.Database.EnsureCreated();
        }

        public static void Initialize(EqContext context)
        {
            context.Database.EnsureCreated();

            if (!context.ScheduleTime.Any())
            {
                context.ScheduleTime.AddRange(
                            new ScheduleTime() {
                                Id =1
                                , ResourceId=1
                                , RecordState= RecordState.N
                                , ScheduleId=1
                                , ScheduleDate = DateTime.Parse("15.02.2018 00:00:00")
                                , WorkTimeFrom = TimeSpan.Parse("8:00")
                                , WorkTimeTo = TimeSpan.Parse("18:00")
                                , WorkTimeDuration = TimeSpan.Parse("8:00")},
                               new ScheduleTime() {
                                Id =2
                                , ResourceId=1
                                , RecordState= RecordState.N
                                , ScheduleId=1
                                , ScheduleDate = DateTime.Parse("16.02.2018 00:00:00")
                                , WorkTimeFrom = TimeSpan.Parse("8:00")
                                , WorkTimeTo = TimeSpan.Parse("18:00")
                                , WorkTimeDuration = TimeSpan.Parse("8:00")}
                            
                       
                            //,  new ScheduleTime() {
                            //    Id = 2
                            //    , ResourceId = 2
                            //    , RecordState = RecordState.N
                            //    , ScheduleId = 2
                            //    , ScheduleDate = DateTime.Parse("22.10.2017 00:00:00")
                            //    , WorkTimeFrom = TimeSpan.Parse("8:00")
                            //    , WorkTimeTo = TimeSpan.Parse("16:00")
                            //    , WorkTimeDuration = TimeSpan.Parse("8:00") }          
                );
                context.SaveChanges();
            }

            if (!context.Schedule.Any())
            {
                context.Schedule.AddRange( 
                    new Schedule(){
                    Day1 = true
                    ,Day2 = true
                    ,Day3 = true
                    ,Day4 = true
                    ,Day5 = true
                    ,Day6 = false
                    ,Day7 = false
                    , IsAllDay = false
                    , Note = ""
                    , RecordState = RecordState.N
                    , ResourceId = 1
                    , ScheduleTypeId = 1
                    , Duration = TimeSpan.Parse("08:00")
                    , WorkDateFrom = DateTime.Parse("14.02.2018 00:00:00")
                    , WorkDateTo = DateTime.Parse("28.02.2018 00:00:00")
                    ,WorkTimeFrom = DateTime.Parse("14.02.2018 08:00")
                    ,WorkTimeTo = DateTime.Parse("28.02.2018 18:00")
                    }
                //,
                //    new Schedule(){
                //    Day1 = true
                //    ,Day2 = true
                //    ,Day3 = true
                //    ,Day4 = true
                //    ,Day5 = false
                //    ,Day6 = false
                //    ,Day7 = false
                //    , IsAllDay = false
                //    , Note = ""
                //    , RecordState = RecordState.N
                //    , ResourceId = 2
                //    , ScheduleTypeId = 1
                //    , Duration = TimeSpan.Parse("09:00")
                //    , WorkDateFrom = DateTime.Parse("23.10.2017 00:00:00")
                //    , WorkDateTo = DateTime.Parse("01.10.2018 00:00:00")
                //    ,WorkTimeFrom = DateTime.Parse("01.01.2017 08:00")
                //    ,WorkTimeTo = DateTime.Parse("01.01.2107 18:00")
                //    }
                //,
                //    new Schedule(){
                //    Day1 = true
                //    ,Day2 = true
                //    ,Day3 = true
                //    ,Day4 = true
                //    ,Day5 = false
                //    ,Day6 = false
                //    ,Day7 = false
                //    , IsAllDay = true
                //    , Note = "All Day test"
                //    , RecordState = RecordState.N
                //    , ResourceId = 3
                //    , ScheduleTypeId = 2
                //    , Duration = TimeSpan.Parse("09:00")
                //    , WorkDateFrom = DateTime.Parse("23.10.2017 00:00:00")
                //    , WorkDateTo = DateTime.Parse("01.10.2018 00:00:00")
                //    ,WorkTimeFrom = DateTime.Parse("01.01.2017 08:00")
                //    ,WorkTimeTo = DateTime.Parse("01.01.2107 18:00")
                //    }
                    );
                context.SaveChanges();
            }
        }
    }
}

