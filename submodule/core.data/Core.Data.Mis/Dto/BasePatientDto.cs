using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;

namespace Core.Data.Mis.Dto
{
    public abstract class BasePatientListDto: BaseDto
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
    }

    public abstract class BasePatientDetailDto: BaseDto
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
    }
}
