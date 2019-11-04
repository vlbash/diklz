using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;

namespace App.Data.DTO.APP
{
    public class AppShortDTO : BaseDTO
    {
        #region LIMS
        [Required]
        [DisplayName("Виконавець заяви")]
        public Guid? PerformerId { get; set; }

        [Required]
        [DisplayName("Виконавець заяви")]
        [NotMapped]
        public string PerformerName { get; set; }

        [Required]
        [DisplayName("Номер реєстрації")]
        public string RegNumber { get; set; }

        [Required]
        [DisplayName("Дата реєстрації")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }

        #endregion
         
        [DisplayName("Тип заяви")]
        public string AppSort { get; set; }

        [DisplayName("Підстава")]
        public bool IsCreatedOnPortal { get; set; }

        [DisplayName("Коментар")]
        //[Required (ErrorMessage = "Заповніть поле")]
        public  string ReturnComment { get; set; }

        public bool ReturnCheck { get; set; }
    }
}
