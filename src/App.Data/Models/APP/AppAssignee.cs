using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.APP
{
    public class AppAssignee: BaseEntity
    {
        [MaxLength(100)]
        [DisplayName("Ім'я")]
        
        public string Name { get; set; }

        [MaxLength(200)]
        [DisplayName("По батькові")]
        
        public string MiddleName { get; set; }

        [MaxLength(200)]
        [DisplayName("Прізвище")]
        
        public string LastName { get; set; }

        [MaxLength(50)]
        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        public string IPN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата народження")]
        
        public DateTime? Birthday { get; set; }

        [DisplayName("Тип особи")]
        [MaxLength(30)]
        public string OrgPositionType { get; set; }

        [DisplayName("Найменування навчального закладу")]
        [MaxLength(255)]
        public string EducationInstitution { get; set; }

        [DisplayName("Рік закінчення навчального закладу")]
        [MaxLength(10)]
        public string YearOfGraduation { get; set; }

        [DisplayName("Номер диплому")]
        [MaxLength(25)]
        public string NumberOfDiploma { get; set; }

        [DisplayName("Дата видачі диплому")]
        public DateTime? DateOfGraduation { get; set; }

        [DisplayName("Спеціальність")]
        [MaxLength(200)]
        public string Speciality { get; set; }

        [DisplayName("Стаж роботи за фахом (місяців)")]
        [MaxLength(5)]
        public string WorkExperience { get; set; }

        [DisplayName("Номер трудового договору")]
        [MaxLength(20)]
        public string NumberOfContract { get; set; }

        [DisplayName("Номер наказу про призначення на посаду")]
        [MaxLength(20)]
        public string OrderNumber { get; set; }

        [DisplayName("Дата трудового договору")]
        public DateTime? DateOfContract { get; set; }

        [DisplayName("Дата наказу про призначення на посаду")]
        public DateTime? DateOfAppointment { get; set; }

        [DisplayName("Назва посади")]
        [MaxLength(100)]
        public string NameOfPosition { get; set; }

        [DisplayName("Контактна інформація")]
        [MaxLength(255)]
        public string ContactInformation { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        public Guid? LicenseAssigneeId { get; set; }
        public AppAssignee LicenseAssignee { get; set; }

        public bool? LicenseDeleteCheck { get; set; }

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIO =>
            $"{LastName} {(String.IsNullOrEmpty(Name) ? " " : Name)} {(String.IsNullOrEmpty(MiddleName) ? " " : MiddleName)}";

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIOShort =>
            $"{LastName} {(String.IsNullOrEmpty(Name) ? ' ' : Name[0])}. {(String.IsNullOrEmpty(MiddleName) ? ' ' : MiddleName[0])}.";

        public bool? IsFromLicense { get; set; }
    }
}
