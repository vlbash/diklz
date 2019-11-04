using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.MTR
{
    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MtrAppListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("Дата подання")]
        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Тип заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppType { get; set; }

        [DisplayName("Тип заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppTypeEnum { get; set; }


        [DisplayName("Вид заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppSort { get; set; }

        [DisplayName("Вид заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppSortEnum { get; set; }

        [DisplayName("Стан розгляду заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppState { get; set; }

        [NotMapped]
        public string AppStateString
        {
            get
            {
                if (AppStateEnum == "Reviewed" && !ProtocolStatusNameCheck)
                    return "На розгляді";
                return AppState;
            }
        }

        [DisplayName("Стан розгляду заяви")]
        [PredicateCase(PredicateOperation.Equals)]
        public string AppStateEnum { get; set; }

        [DisplayName("Тип рішення")]
        [PredicateCase]
        public string DecisionEnum { get; set; }

        [DisplayName("Тип рішення")]
        [PredicateCase]
        public string Decision { get; set; }

        public string DecisionDescription { get; set; }

        public string DecisionReason { get; set; }

        [DisplayName("Дата реєстрації")]
        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }

        [DisplayName("Номер реєстрації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string RegNumber { get; set; }

        public string EdocumentStatus { get; set; }

        public string ProtocolStatusName { get; set; }

        [NotMapped]
        public bool ProtocolStatusNameCheck => ProtocolStatusName == "Закрито";

        [DisplayName("Підтвердження оплати")]
        [NotMapped]
        public string PaymentStatus
        {
            get
            {
                if (AppSortEnum != "GetLicenseApplication" && AppSortEnum != "IncreaseToPRLApplication")
                {
                    return "Не потребує";
                }

                if (string.IsNullOrEmpty(EdocumentStatus))
                {
                    return "Потребує оплати";
                }

                switch (EdocumentStatus)
                {
                    case "PaymentConfirmed":
                        return "Оплата підтверджена";
                    case "WaitingForConfirmation":
                        return "Очікує підтвердження";
                    case "DontNeed":
                        return "Не потребує";
                    default:
                        return "Потребує оплати";
                }
            }
        }

        public bool ReturnCheck { get; set; }

        public string ReturnComment { get; set; }
    }
}
