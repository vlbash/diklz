using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities;
using System;
using System.Collections.Generic;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.CPL
{
    public class CplCalendarPlan : BaseEntity
    {

        //статус документа(Календарний план) з "Класифікатор станів документів" DocStatus
        public string DocStatusEnum { get; set; } //enum

        public string Number { get; set; }

        public DateTime CalendarPlanDate { get; set; }

        public string Comment { get; set; }

        //Проект 1:1
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        
        //1 ко многим с этапами календарного плана
        public List<CplCalendarPlanStage> CalendarPlanStages { get; set; }
    }
}