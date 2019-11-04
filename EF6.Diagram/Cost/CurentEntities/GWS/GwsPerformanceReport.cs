using App.Data.Entities.CDN;
using Astum.Core.Data.Entities.Common;
using System;

namespace App.Data.Entities.GWS
{
    //2.2.5.5.1	Форма «Картка загального журналу робіт у режимі перегляду, вкладка «Відомість про виконання робіт»
    public class GwsPerformanceReport : BaseEntity
    {
        //загальний журнал робіт
        public Guid GeneralWorksheetId { get; set; }

        public GeneralWorksheet GeneralWorksheet { get; set; }

        public DateTime PerformanceReportDate { get; set; }

        //кількість годин
        public double DurationHour { get; set; }

        //Значення з довідника видів робіт в межах певного виду робіт
        public Guid CdnWorkTypeId { get; set; }

        public CdnWorkType CdnWorkType { get; set; }

        public double NumberOfUnits { get; set; }

        public bool HiddenWorks { get; set; }

        public string ConfirmDocs { get; set; }
    }
}
