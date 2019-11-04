using App.Core.Data.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Attributes;
using App.Core.Data.Entities.Common;
using App.Core.Security;

namespace App.Core.Data.DTO.Common
{
    [RightsCheckList(nameof(Person))]
    public class PersonDetailDTO : BaseDTO
    {
        public string FIO { get; set; }

        [MaxLength(200)]
        [DisplayName("По батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string MiddleName { get; set; }

        [MaxLength(200)]
        [DisplayName("Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string LastName { get; set; }

        [MaxLength(100)]
        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }

        [DisplayName("Стать")]
        public string GenderEnum { get; set; }

        public string GenderName { get; set; }

        [DisplayName("Дата народження")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Електронна адреса")]
        public string Email { get; set; }

        [MaxLength(50)]
        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        public string IPN { get; set; }

        [DisplayName("РНОКПП (Індивідуальний податковий номер) відсутній")]
        public bool NoIPN { get; set; }

        [MaxLength(50)]
        [DisplayName("Документ що посвідчує особу")]
        public string IdentityDocumentTypeEnum { get; set; }

        public string IdentityDocumentTypeName { get; set; }

        [DisplayName("Серія документу")]
        public string DocPrefix { get; set; }

        [MaxLength(20)]
        [DisplayName("Номер документу")]
        public string PersonalDocumentNumber { get; set; }

        [MaxLength(100)]
        [DisplayName("Ким виданий")]
        public string PersonalDocumentAuthority { get; set; }

        [DisplayName("Дата видачі")]
        public DateTime? DocumentIssueDate { get; set; }

        [DisplayName("Діє до")]
        public DateTime? ExpirationDate { get; set; }

    }

    [RightsCheckList(nameof(Person))]
    public class PersonListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("ПІБ")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }
    }
}
