using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Extensions;

namespace Core.Data.Common.Models
{
    [Display(Name = "Персона")]
    [Table("Person")]
    public abstract class BasePerson : CoreEntity, IPerson
    {
        [MaxLength(100)]
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string Name { get; set; }

        [MaxLength(200)]
        [Display(Name = "По батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string MiddleName { get; set; }

        [MaxLength(200)]
        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string LastName { get; set; }

        [Display(Name = "Місце проживання")]
        public virtual string Location { get; set; }
        
        [Display(Name = "Дата народження")]
        public virtual DateTime Birthday { get; set; }

        [Display(Name = "Стать")]
        public virtual string GenderEnum { get; set; }

        [Display(Name = "Телефон")]
        public virtual string Phone { get; set; }

        [Display(Name = "Електронна адреса")]
        public virtual string Email { get; set; }

        [Display(Name = "Системний користувач")]
        public virtual string UserId { get; set; }

        [MaxLength(200)]
        public virtual string UserName { get; set; }

        [Display(Name = "Фото")]
        public virtual string Photo { get; set; }

        [MaxLength(1024)]
        public virtual string Description { get; set; }

        [MaxLength(50)]
        [Display(Name = "ІПН")]
        public virtual string IPN { get; set; }

        [Display(Name = "ІПН відсутній")]
        public virtual bool NoIPN { get; set; }

        [MaxLength(50)]
        [Display(Name = "Документ що посвідчує особу")]
        public virtual string IdentityDocumentTypeEnum { get; set; }

        [Display(Name = "Серія документу")]
        public virtual string DocPrefix { get; set; }

        [MaxLength(20)]
        [Display(Name = "Номер документу")]
        public virtual string PersonalDocumentNumber { get; set; }

        [MaxLength(100)]
        [Display(Name = "Ким виданий")]
        public virtual string PersonalDocumentAuthority { get; set; }

        [Display(Name = "Дата видачі")]
        public virtual DateTime? DocumentIssueDate { get; set; }

        [Display(Name = "Діє до")]
        public virtual DateTime? ExpirationDate { get; set; }

        [NotMapped]
        [Display(Name = "П.I.Б.")]
        public virtual string FIO => $"{LastName} {(string.IsNullOrEmpty(Name) ? " " : Name)} {(string.IsNullOrEmpty(MiddleName) ? " " : MiddleName)}";

        [NotMapped]
        [Display(Name = "П.I.Б.")]
        public virtual string FIOShort => string.Format("{0} {1}. {2}.", LastName, (string.IsNullOrEmpty(Name) ? ' ' : Name[0]), (string.IsNullOrEmpty(MiddleName) ? ' ' : MiddleName[0]));

        [NotMapped]
        [Display(Name = "Профайл")]
        public virtual string FIOnVik =>
            $"{LastName} {Name} {MiddleName}, {FullYears}";

        [NotMapped]
        [Display(Name = "Вік")]                //число, розраховується в залежності від дати народження на момент заповненвя анкети
        public virtual string FullYears //=> Birthday.HasValue ? ((Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) - Int32.Parse(Birthday.Value.ToString("yyyyMMdd"))) / 10000).ToString() + " років" : "";    //TODO: Need DateTime helper
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
        [Display(Name = "Профайл")]
        public virtual string Title
        {
            get { return $"{LastName} {Name} {MiddleName}, {(Birthday.ToString("dd.MM.yyyy"))}"; }
            set { }
        }
    }
}
