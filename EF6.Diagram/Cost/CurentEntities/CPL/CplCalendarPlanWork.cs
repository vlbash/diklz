using App.Data.Entities.PRJ;
using App.Data.Entities.RES;
using Astum.Core.Data.Entities;
using System;
using System.Collections.Generic;
using Astum.Core.Data.Entities.Common;
using App.Data.Entities.CDN;

namespace App.Data.Entities.CPL
{
    public class CplCalendarPlanWork: BaseEntity
    {
        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }
        
        //кількість одиниць
        public double NumberOfUnits { get; set; }

        //цільовий показник
        public double TargetValue { get; set; }

        //+Календарний план етап
        public Guid CalendarPlanStageId { get; set; }
        public CplCalendarPlanStage CalendarPlanStage { get; set; }

        //+Вид робіт
        public Guid WorkTypeId { get; set; }
        public CdnWorkType WorkType { get; set; }

        /*//Розпорядний документ 1:1( mvp версии нет присоединения к конкретной работе договора)
        public Guid ContractId { get; set; }
        public PrjContract Contract { get; set; }*/

        //1 ко многим с ресурсами
        public List<ResPlannedResource> PlannedResources { get; set; }
    }
}
