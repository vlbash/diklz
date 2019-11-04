using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.APP;
using App.Data.Models.ORG;

namespace App.Data.DTO.APP
{
    [RightsCheckList(nameof(AppDecision))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class AppDecisionDTO: BaseDTO
    {
        public bool IsClosed { get; set; }
        public Guid AppId { get; set; }
        public Guid OrgUnitId { get; set; }
        public string ExpertiseResultEnum { get; set; }

        [DisplayName("Тип рішення")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string DecisionType { get; set; }

        [DisplayName("Підстава")]
        public string DecisionReasons { get; set; }

        [DisplayName("Підстава")]
        [NotMapped]
        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        public List<string> ListOfDecisionReason { get; set; } = new List<string>();

        [DisplayName("Дата початку дії")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfStart { get; set; } = DateTime.Now;

        [DisplayName("№ і дата протоколу")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid ProtocolId { get; set; }

        [DisplayName("Номер протоколу")]
        public string ProtocolNumber { get; set; }

        [DisplayName("Дата протоколу")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfProtocol { get; set; }

        [DisplayName("№ і дата протоколу")]
        public string ProtocolDetails => $"№{ProtocolNumber} - {DateOfProtocol:dd.MM.yyyy}";

        [DisplayName("Текст рішення")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string DecisionDescription { get; set; }

        [DisplayName("Сплачено (грн)")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public decimal PaidMoney { get; set; }

        [DisplayName("Нотатки")]
        public string Notes { get; set; }
    }
}
