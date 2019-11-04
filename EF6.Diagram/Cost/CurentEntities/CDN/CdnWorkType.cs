using System.Collections.Generic;
using Astum.Core.Data.Entities.Common;
using App.Data.Entities.CPL;
using System;

namespace App.Data.Entities.CDN
{
    public class CdnWorkType:BaseDirectory
    {
        //Підпорядкування з довідника CdnUnitedPurchase (ДК 021:2015)
        public Guid? UnitedPurchaseId { get; set; }

        //Вид робіт - «код CPV» та «Назва об’єкту» з запису довідника видів робіт.
        public string WorkType { get; set; }

        //Одиниця виміру з "Класифікатор одиниць виміру" UnitOfMeasurement
        public string UnitOfMeasurementEnum { get; set; } //enum

        //Статус з "Класифікатор статусів записів" RecordStatus
        public string RecordStatusEnum { get; set; } //enum

        //1 ко многим  с работам календарного плана
        public List<CplCalendarPlanWork> CalendarPlanWorks { get; set; }
    }


}
