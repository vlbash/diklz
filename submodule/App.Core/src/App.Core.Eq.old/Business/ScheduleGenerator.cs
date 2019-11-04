using App.Core.Eq.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Eq.Entities.Schedule;
using App.Core.Eq.Entities.Schedule.SchedulePropertyModels;

namespace App.Core.Eq.Business
{
    public class ScheduleGenerator
    {
        private EqContext _appDbExtContext;
        private readonly Schedule _scheduleSettings;

        public ScheduleGenerator()
        {
            _scheduleSettings = new Schedule();
            InitDBContext();
        }

        public ScheduleGenerator(Schedule settings)
        {
            _scheduleSettings = new Schedule();
            InitDBContext();
            _scheduleSettings = settings;
        }

        private void InitDBContext()
        {
            try
            {
                var dbDicomParams = new string[] { };
                _appDbExtContext = new EqContextFactory().CreateDbContext(dbDicomParams);
            }
            catch (Exception ex)
            {
                throw new Exception($"Incorrect database connection settings.\n{ex.Message}");
            }
        }

        public List<ScheduleSlots> GenerateSlots(long schedId)
        {
            var schedulesToProcess = new List<Schedule>();
            List<ScheduleSlots> slotsResult = new List<ScheduleSlots>();

            schedulesToProcess = GetSchedulesToProcess();
  
            var processingSchedulesIdList = schedId;

            var listOfHolidays = _appDbExtContext.HolidaysDictionary.Select(x => x.HolidayDate).ToList();
            //Generate TimeTable
            TimeTableGenerator ttg = new TimeTableGenerator(schedulesToProcess, _scheduleSettings, listOfHolidays);
            var genResult = ttg.Generate();

            if (genResult != null && genResult.Count > 0)
            {
                _appDbExtContext.ScheduleTime.AddRange(genResult);
                _appDbExtContext.SaveChanges();
            }

            //Generate Slots
            #region CreateTestSchedulesPropertyData            

            #endregion

            List<ScheduleSlots> existSlots = GetExistedScheduledPropertySlots(schedId); //From schedules properties

            var timeTableData = GetScheduleTimes(processingSchedulesIdList);
            if (timeTableData.Count == 0)
            {
                return slotsResult;
            }

            SlotGenerator sg = new SlotGenerator(timeTableData, _scheduleSettings, existSlots);
            slotsResult = sg.Generate();

            slotsResult = slotsResult.Where(x => x.Type == SlotType.Shedule).ToList();

            _appDbExtContext.SaveChanges();


            using (var context = new EqContextFactory().CreateDbContext(new string[0]))
            {
                context.Slots.AddRange(slotsResult);
                context.SaveChanges();
           }


            return slotsResult;
        }

        private List<Schedule> GetSchedulesToProcess()
        {
            List<Schedule> schReturn = _appDbExtContext.Schedule.Where(x => x.RecordState == RecordState.N).ToList();

            if (schReturn.Any())
            {
                schReturn = schReturn?.Where(x => x.ResourceId == _scheduleSettings.ResourceId).ToList();
            }
            return schReturn;
        }

        private List<ScheduleSlots> GetExistedScheduledPropertySlots(long schedId)
        {
            var probRes = _appDbExtContext.ScheduleProperties.Where(x=> x.ScheduleId == schedId).ToList();
            List<ScheduleSlots> existSlots = new List<ScheduleSlots>();

            foreach (var propItem in probRes)
            {
                var propResourceId = _appDbExtContext.Schedule.FirstOrDefault(x => x.Id == propItem.ScheduleId && x.RecordState == RecordState.N).ResourceId;

                switch ((SchedulePropertyTypeModel)propItem.ModelEnumId)
                {
                    case SchedulePropertyTypeModel.Break:
                        var tmpBM = new BreakModel();
                        tmpBM.FromJson(propItem.Value);
                        tmpBM.ResourceId = propResourceId;
                        tmpBM.ScheduleTimeId = propItem.ScheduleId;

                        existSlots.Add(tmpBM.ToSlot());
                        break;
                    case SchedulePropertyTypeModel.Vocation:
                        var tmpVM = new VacationModel();
                        tmpVM.FromJson(propItem.Value);
                        tmpVM.ScheduleTimeId = propItem.ScheduleId;
                        tmpVM.ResourceId = propResourceId;
                        existSlots.Add(tmpVM.ToSlot());
                        break;
                    default: break;
                }
            }
            return existSlots;
        }

        private List<ScheduleTime> GetScheduleTimes(long processingSchedulesIdList)
        {
            return _appDbExtContext.ScheduleTime.Where(x => x.ScheduleId == processingSchedulesIdList).ToList();
        }

    }
}
