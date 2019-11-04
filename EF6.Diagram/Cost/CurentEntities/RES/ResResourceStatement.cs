using System;
using System.Collections.Generic;
using App.Data.Entities.CPL;
using Astum.Core.Data.Entities;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.RES
{
    public class ResResourceStatement : BaseEntity
    {
        //статус документа(Ресурсна відомість) з "Класифікатор станів документів" DocStatus
        public string DocStatusEnum { get; set; } //enum

        //+Календарний план 1:1
        public Guid CalendarPlanId { get; set; }
        public CplCalendarPlan CalendarPlan { get; set; }
        
        public string Number { get; set; }

        public DateTime ResourceStatementDate { get; set; }

        public string Comment { get; set; }

        public bool AgreedWithCreditorBank { get; set; }

        //1 ко многим с запланованими ресурсами
        public List<ResPlannedResource> PlannedResources { get; set; }
    }
}