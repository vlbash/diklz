using System.Collections.Generic;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.RES
{
    public class Resource:BaseEntity
    {

        //Одиниця виміру з "Класифікатор одиниць виміру" UnitOfMeasurement
        public string UnitOfMeasurementEnum { get; set; } //enum

        //типів ресурсу з "Класифікатор типів ресурсів" ResourceType
        public string ResourceTypeEnum { get; set; } //enum

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double MinPrice { get; set; }

        public double MaxPrice { get; set; }

        public string Characteristic { get; set; }

        //ІТП
        public bool ITP { get; set; }

        //Використовується
        public bool Enabled { get; set; }

        //1 ко многим с запланованими ресурсами
        public List<ResPlannedResource> PlannedResources { get; set; }
    }
}