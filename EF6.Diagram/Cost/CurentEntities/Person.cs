using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay("Персона")]
    public sealed class Person : BaseEntity, IPerson
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
        public string FullName { get; set; }

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
        [DisplayName("ІПН")]
        public string IPN { get; set; }

        [DisplayName("ІПН відсутній")]
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
        public string FIO => String.Format("{0} {1} {2}", FullName, (String.IsNullOrEmpty(Name) ? " " : Name), (String.IsNullOrEmpty(MiddleName) ? " " : MiddleName));

        [NotMapped]
        [DisplayName("П.I.Б.")]
        public string FIOShort => String.Format("{0} {1}. {2}.", FullName, (String.IsNullOrEmpty(Name) ? ' ' : Name[0]), (String.IsNullOrEmpty(MiddleName) ? ' ' : MiddleName[0]));

        [NotMapped]
        [DisplayName("Профайл")]
        public string FIOnVik =>
            $"{FullName} {Name} {MiddleName}, {FullYears}";

        [NotMapped]
        [DisplayName("Вік")]                //число, розраховується в залежності від дати народження на момент заповненвя анкети
        public string FullYears { get; set; }

        [NotMapped]
        [DisplayName("Профайл")]
        public string Title
        {
            get { return $"{FullName} {Name} {MiddleName}, {(Birthday.ToString("dd.MM.yyyy"))}"; }
            set { }
        }
    }
}
