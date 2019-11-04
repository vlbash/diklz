using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;

namespace App.Data.Entities.PRJ
{
    public class Project : BaseEntity
    {
        //Назва проекту
        [Required]
        public string Name { get; set; }

        //Код проекта
        public string Code { get; set; }

        //Область
        public Guid RegionId { get; set; }

        //Область
        public Guid? RegionDistrictId { get; set; }

        //Статус проекту из класификатора"Класифікатор статусу проекту" ProjectStatus
        public string ProjectStatusEnum { get; set; } //enum

        //Тип фінансування з "Класифікатор типу фінансування" FinansingType
        public string FinansingTypeEnum { get; set; } //enum

        public double Price { get; set; }

        public double PaidSum { get; set; }

        public double WarrantyPeriod { get; set; }

        public double StatusRelativeToExpenditures { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }
        
        //статусу виконання проекту з "Класифікатор статусу виконання проекту" ExecutionProjectStatus
        public string ExecutionProjectStatusEnum { get; set; } //enum

        //Опис проекта
        public string Comment { get; set; }

        //Добавление списка объектов с координатами
        public List<ProjectCObject> ProjectCObject { get; set; }

        //+Учасники
        public List<PrjParticipant> Participants { get; set; }
        
    }
}
