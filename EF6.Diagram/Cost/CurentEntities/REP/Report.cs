using System;
using App.Data.Entities.Common;
using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Entities.ORG;

namespace App.Data.Entities.REP
{
    public class Report : BaseEntity
    {
        //проект
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public string WeatherConditions { get; set; }

        public string RegNumber { get; set; }

        public DateTime RegDate { get; set; }

        //статус документа з "Класифікатор станів документів" DocStatus
        public string DocStatusEnum { get; set; } //enum

        //Об’єкт
        public string Object { get; set; }

        public string ReportType { get; set; }

        public string ReportPeriod { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public string YesterdayCharacteristic { get; set; }

        public string TodayCharacteristic { get; set; }

        public string Description { get; set; }

        public double ScheduleDelay { get; set; }

        public double Progress { get; set; }

        public Guid? ContractorId { get; set; }

        public OrgEmployee Contractor { get; set; }

        public Guid? SupervisorId { get; set; }

        public OrgEmployee Supervisor { get; set; }

        //public Guid ConstructionObjectId { get; set; }

        //public ConstructionObject ConstructionObject { get; set; }
    }
}
