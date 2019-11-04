using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Eq.Entities.Schedule;

namespace App.Core.Eq.Business
{
    public class ScheduleSettings
    {
        public void CheckSettings(Schedule settings)
        {
            if (settings.Duration == TimeSpan.Parse("00:00:00") || (settings.Duration.Value.Minutes < 5 && settings.Duration.Value.Hours < 1))
            {
                throw new Exception("Некоректна тривалість прийому!");
            }

            if (settings.WorkDateTo.Value.CompareTo(settings.WorkDateFrom.Value) < 0)
            {
                throw new Exception("Некоректно вказаний період дії рокладу!");
            }

            if (settings.WorkTimeTo.Value.CompareTo(settings.WorkTimeFrom.Value) < 0)
            {
                throw new Exception("Некоректно вказано час прийому");
            }
        }
    }
}
