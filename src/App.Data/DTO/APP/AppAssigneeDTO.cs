using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.DTO.BRN;
using App.Data.Models.APP;
using App.Data.Models.ORG;

namespace App.Data.DTO.APP
{
    [RightsCheckList(nameof(AppAssignee))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class AppAssigneeDetailDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        [NotMapped]
        public Guid? appId { get; set; }

        [NotMapped]
        public string AppType { get; set; }

        [MaxLength(100)]
        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }

        [MaxLength(200)]
        [DisplayName("По батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string MiddleName { get; set; }

        [MaxLength(200)]
        [DisplayName("Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string LastName { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Мінімальна кількість символів - 10")]
        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        [MinLength(10)]
        //[Required(ErrorMessage = "Заповніть поле")]
        [RegularExpression("[\'\",.:0-9а-яА-Я ]+", ErrorMessage = "Поле не має містити символів латиниці")]
        public string IPN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата народження")]
        //[Required(ErrorMessage = "Заповніть поле")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Тип особи")]
        [Required(ErrorMessage = "Заповніть поле")]
        [MaxLength(30)]
        public string OrgPositionType { get; set; }

        [DisplayName("Найменування навчального закладу")]
        [Required(ErrorMessage = "Заповніть поле")]
        [MaxLength(255)]
        public string EducationInstitution { get; set; }

        [DisplayName("Рік закінчення навчального закладу")]
        [Required(ErrorMessage = "Заповніть поле")]
        [MaxLength(10)]
        public string YearOfGraduation { get; set; }

        [DisplayName("Серія і номер диплому")]
        [MaxLength(25)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string NumberOfDiploma { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата видачі диплому")]
        [Required(ErrorMessage = "Заповніть поле")]
        public DateTime? DateOfGraduation { get; set; }

        [DisplayName("Спеціальність")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Speciality { get; set; }

        [DisplayName("Стаж роботи за фахом (місяців)")]
        [MaxLength(5)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string WorkExperience { get; set; }

        [DisplayName("Номер трудового договору")]
        //[Required(ErrorMessage = "Заповніть поле")]
        [MaxLength(20)]
        public string NumberOfContract { get; set; }

        [DisplayName("Номер наказу про покладання обов'язків")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string OrderNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата трудового договору")]
        //[Required(ErrorMessage = "Заповніть поле")]
        public DateTime? DateOfContract { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата наказу про покладання обов'язків")]
        [Required(ErrorMessage = "Заповніть поле")]
        public DateTime? DateOfAppointment { get; set; }

        [DisplayName("Посада")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string NameOfPosition { get; set; }

        [DisplayName("Контактна інформація")]
        [MaxLength(255)]
        [Required(ErrorMessage = "Заповніть поле")]
        public string ContactInformation { get; set; }

        [DisplayName("Коментар")]
        //[Required(ErrorMessage = "Заповніть поле")]
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

        [DisplayName("МПД")]
        [NotMapped]
        public List<Guid> ListOfBranches { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames { get; set; } = new List<string>();

        [DisplayName("МПД")]
        [NotMapped]
        public Guid? BranchId { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public string BranchName { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public string AppSort { get; set; }

        [DisplayName("Тип особи")]
        public string AssigneTypeName { get; set; }
    }

    [RightsCheckList(nameof(AppAssignee))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class AppAssigneeListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("Ім'я")]
        public string Name { get; set; }

        [DisplayName("По батькові")]
        public string MiddleName { get; set; }

        [DisplayName("Прізвище")]
        public string LastName { get; set; }

        [DisplayName("Посада")]
        public string NameOfPosition { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата наказу про призначення на посаду")]
        public DateTime? DateOfAppointment { get; set; }

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIO =>
            $"{LastName} {(String.IsNullOrEmpty(Name) ? " " : Name)} {(String.IsNullOrEmpty(MiddleName) ? " " : MiddleName)}";

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames
        {
            get
            {
                return ListOfBranches.Select(x => new string($"{x.Name}, {x.PhoneNumber}")).ToList();
            }
        }

        [NotMapped]
        public List<BranchListDTO> ListOfBranches { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }

        [DisplayName("Тип особи")]
        public string AssigneTypeName { get; set; }
    }
}
