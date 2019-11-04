using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.APP;

namespace App.Data.DTO.APP
{
    [RightsCheckList(nameof(AppLicenseMessage))]
    public class AppLicenseMessageDTO : BaseDTO
    {
        public Guid AppId { get; set; }
        public string ExpertiseResultEnum { get; set; }
        public string DecisionType { get; set; }

        [DisplayName("№ листа-повідомлення")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string MessageNumber { get; set; }

        [DisplayName("Дата листа повідомлення")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DateOfMessage { get; set; }

        [DisplayName("Підписав (посада)")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string SignedJobPosition { get; set; }

        [DisplayName("Підписав (ПІБ)")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string SignedFullName { get; set; }

        [DisplayName("Виконавець")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public Guid PerformerId { get; set; }

        [DisplayName("Виконавець")]
        [NotMapped]
        public string PerformerName { get; set; }

        [DisplayName("Стан повідомлення")]
        public string State { get; set; }

        [DisplayName("Приєднаний файл")]
        public string AttachedFile { get; set; }
    }
}
