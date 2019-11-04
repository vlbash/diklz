using System;
using System.Collections.Generic;
using System.Text;
using App.Data.Entities.CPL;
using App.Data.Entities.PRJ;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.ACW
{
    public class ActOfCompletedWork: BaseEntity
    {
        public string Number { get; set; }

        public DateTime ActOfCompletedWorkDate { get; set; }

        //проект
        public Guid ProjectId { get; set; }

        public Project Project { get; set; } 

        //+Календарний план етап
        public Guid CalendarPlanStageId { get; set; }
        public CplCalendarPlanStage CalendarPlanStage { get; set; }

        
        //статус документа з "Класифікатор станів документів" DocStatus
        public string DocStatusEnum { get; set; } //enum

        //статус документа з "Класифікатор типів актів" DocStatus
        public string DocTypeEnum { get; set; } //enum

        public DateTime? ReportDateFrom { get; set; }

        public DateTime? ReportDateTo { get; set; }

        //Разом прями витрати
        public double AllDirectCosts { get; set; }

        //Загальновиробничі витрати
        public double GeneralProductionCosts { get; set; }
        
        //Кошти на зведення та розбирання тимчасових будівель та споруд
        public double ConstAndDistTemporalBuildCosts { get; set; }

        //Додаткові витрати
        public double AditionalCosts { get; set; }

        //Інші супутні витрати
        public double OtherCosts { get; set; }

        //Прибуток
        public double Profit { get; set; }

        //Адмінвитрати
        public double AdminCosts { get; set; }

        //Кошти на покриття ризику
        public double RiskCosts { get; set; }

        //Податки,  збори,  обов'язкові  платежі
        public double TaxesCosts { get; set; }

        //Примітки
        public string Description { get; set; } 
    }
}
