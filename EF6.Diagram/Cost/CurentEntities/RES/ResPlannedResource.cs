using System;
using App.Data.Entities.CPL;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.RES
{
    public class ResPlannedResource : BaseEntity
    {
        //+Ресурсна відомість 
        public Guid ResourceStatementId { get; set; }
        public ResResourceStatement ResourceStatement { get; set; }

        //+Календарний план 
        public Guid CalendarPlanWorkId { get; set; }
        public CplCalendarPlanWork CalendarPlanWork { get; set; }

        //Ресурс
        public Guid ResourceId { get; set; }
        public Resource Resource { get; set; }

        //Загальний об'єм
        public double TotalVolume { get; set; }

        
    }
}