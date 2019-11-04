using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;
using App.Core.Common.Extensions;
using App.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay("Персона")]
    [Display(Name = "Персона")]
    public sealed class Person : CoreEntity, IPerson
    {
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

        [DisplayName("Місце проживання")]
        public string Location { get; set; }
        
        [DisplayName("Дата народження")]
        public DateTime Birthday { get; set; }

        [DisplayName("Стать")]
        public string GenderEnum { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Електронна адреса")]
        public string Email { get; set; }

        [DisplayName("Системний користувач")]
        public string UserId { get; set; }

        [MaxLength(200)]
        public string UserName { get; set; }

        [DisplayName("Фото")]
        public string Photo { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(50)]
        [DisplayName("РНОКПП (Індивідуальний податковий номер)")]
        public string IPN { get; set; }

        [DisplayName("РНОКПП (Індивідуальний податковий номер) відсутній")]
        public bool NoIPN { get; set; }

        [MaxLength(50)]
        [DisplayName("Документ що посвідчує особу")]
        public string IdentityDocumentTypeEnum { get; set; }

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

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIO => $"{LastName} {(string.IsNullOrEmpty(Name) ? " " : Name)} {(string.IsNullOrEmpty(MiddleName) ? " " : MiddleName)}";

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIOShort => string.Format("{0} {1}. {2}.", LastName, (string.IsNullOrEmpty(Name) ? ' ' : Name[0]), (string.IsNullOrEmpty(MiddleName) ? ' ' : MiddleName[0]));

        [NotMapped]
        [DisplayName("Профайл")]
        public string FIOnVik =>
            $"{LastName} {Name} {MiddleName}, {FullYears}";

        [NotMapped]
        [DisplayName("Вік")]                //число, розраховується в залежності від дати народження на момент заповненвя анкети
        public string FullYears //=> Birthday.HasValue ? ((Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) - Int32.Parse(Birthday.Value.ToString("yyyyMMdd"))) / 10000).ToString() + " років" : "";    //TODO: Need DateTime helper
        {
            get
            {
                if (Birthday!=null)
                {
                    long res = ((int.Parse(DateTime.Today.ToString("yyyyMMdd")) - int.Parse(Birthday.ToString("yyyyMMdd"))) / 10000);
                    return res.ToString() + res.Pluralize("рік", "роки", "років");
                }
                return "";
            }
            set { }
        }

        [NotMapped]
        [DisplayName("Профайл")]
        public string Title
        {
            get { return $"{LastName} {Name} {MiddleName}, {(Birthday.ToString("dd.MM.yyyy"))}"; }
            set { }
        }
    }
}
