using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.APP;

namespace App.Data.DTO.APP
{
    [RightsCheckList(nameof(AppPreLicenseCheck))]
    public class AppPreLicenseCheckDTO : BaseDTO
    {
        public Guid AppId { get; set; }
        public string ExpertiseResultEnum { get; set; }

        [DisplayName("Планова дата початку")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public DateTime ScheduledStartDate { get; set; } = DateTime.Now;

        [DisplayName("Планова дата кінця перевірки")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public DateTime ScheduledEndDate { get; set; } = DateTime.Now;

        [DisplayName("Перевірку створив")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public Guid CheckCreatedId { get; set; }

        [DisplayName("Перевірку створив")]
        [NotMapped]
        public string CheckCreatedName { get; set; }

        [DisplayName("Дата створення переревірки")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public DateTime CreationDateOfCheck { get; set; } = DateTime.Now;

        [DisplayName("Дата закінчення перевірки")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateOfCheck { get; set; }

        [DisplayName("Результат перевірки")]
        public int? ResultOfCheck { get; set; }
    }
}
