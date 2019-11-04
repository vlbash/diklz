using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.ORG;

namespace App.Data.DTO.ORG
{
    [RightsCheckList(nameof(OrganizationExt))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class EmployeeExtDetailDTO : BaseDTO
    {
        
        public Guid? PersonId { get; set; }

        [DisplayName("П.І.Б персони")]
        public string PersonFIO { get; set; }

        [DisplayName("Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonLastName { get; set; }

        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonName { get; set; }

        [DisplayName("По-батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string PersonMiddleName { get; set; }

        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        public string PersonIPN { get; set; }
        
        [DisplayName("Телефон")]
        public string PersonPhone { get; set; }

        [DisplayName("Організація")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid? OrgUnitId { get; set; }

        [DisplayName("Організація")] 
        public string OrgUnit { get; set; }

        [DisplayName("Посада")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid? OrgUnitPositionId { get; set; }

        [DisplayName("Посада")] 
        public string OrgUnitPosition { get; set; }
        
        [NotMapped]
        public string _ReturnUrl { get; set; }
        
        [DisplayName("E-mail користувача")]
        public string UserEmail { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану всіх заяв")]
        public bool ReceiveOnChangeAllApplication { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану всіх повідомлень")]
        public bool ReceiveOnChangeAllMessage { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану заяв, розміщених особисто")]
        public bool ReceiveOnChangeOwnApplication { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану повідомлень, розміщених особисто")]
        public bool ReceiveOnChangeOwnMessage { get; set; }

        [DisplayName("Отримувати на вказану електронну пошту інформацію по стану особистого кабінету (стан розміщених заяв, " +
                     "розбіжності в даних, перелік файлів, що потребують оновлення та інше)")]
        public bool PersonalCabinetStatus { get; set; }

        public int OldLimsId { get; set; }
    }

    public class EmployeeExtListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("П.І.Б")]
        [PredicateCase(PredicateOperation.Contains)]
        public string PersonFIO { get; set; }

        [DisplayName("Область")]
        [PredicateCase(PredicateOperation.Equals)]
        public Guid? AtuRegionId { get; set; }
        [DisplayName("Область")]
        public string AtuRegion { get; set; }

        [DisplayName("Назва організації")]
        public string OrgUnit { get; set; }

        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        [PredicateCase(PredicateOperation.Contains)]
        public string PersonIPN { get; set; }
        
        [DisplayName("Телефон")]
        public string PersonPhone { get; set; }
        
        [DisplayName("Ел.адреса")]
        public string PersonEmail { get; set; }
    }
    
    public class EmployeeExtMinDTO : BaseDTO
    {
        public string Name { get; set; }
        public Guid OrgUnitId { get; set; }
        public string LastName { get; set; }
    }
}
