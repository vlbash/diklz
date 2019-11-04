using System;
using System.Collections.Generic;
using Astum.Core.Data.Entities;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.CPL
{
    public class CplCalendarPlanStage : BaseEntity
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public double Price { get; set; }

        //+Календарний план
        public Guid CalendarPlanId { get; set; }
        public CplCalendarPlan CalendarPlan { get; set; }

        //1 ко многим работам календарного плана
        public List<CplCalendarPlanWork> CalendarPlanWorks { get; set; }
    }
}