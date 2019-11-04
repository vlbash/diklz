using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.DTO.APP;
using App.Data.Enums;
using App.Data.Models;
using App.Data.Models.ORG;
using App.Data.Models.PRL;

namespace App.Data.DTO.PRL
{
    [RightsCheckList(nameof(PrlApplication))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlAppDetailDTO: BaseAppDetailDTO
    {
        [DisplayName("Коментар")]
        public string ReturnComment { get; set; }

        public bool ReturnCheck { get; set; }
    }

    [RightsCheckList(nameof(PrlApplication))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlAppEditDTO: BaseAppEditDTO
    {
    }

    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlAppListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("Дата модифікації")]
        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Дата створення")]
        public string ModifiedOnStr =>
            ModifiedOn.ToString("«dd» MMMM yyyy HH:mm", CultureInfo.CreateSpecificCulture("uk"));

        [DisplayName("Тип заяви")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppType { get; set; }

        [DisplayName("Вид заяви")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppSort { get; set; }

        public string AppSortFull { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppTypeEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppSortEnum { get; set; }

        [DisplayName("Статус")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppState { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppStateEnum { get; set; }

        [DisplayName("Тип рішення і стан заяви")]
        [PredicateCase(PredicateOperation.Contains)]
        public string BackOfficeAppState { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string BackOfficeAppStateEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Тип рішення")]
        public string DecisionType { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Тип рішення")]
        public string DecisionTypeEnum { get; set; }

        [DisplayName("Місце створення заяви")]
        public bool IsCreatedOnPortal { get; set; }

        [NotMapped]
        [DisplayName("Місце створення заяви")]
        public string IsCreatedOnPortalString => IsCreatedOnPortal ? "Портал" : "ДЛС";

        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Номер і дата реєстрації")]
        public DateTime? RegDate { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Номер реєстрації")]
        public string RegNumber { get; set; }

        [NotMapped]
        public string RegDateNumber => concatDate(RegDate, RegNumber);

        [NotMapped]
        [DisplayName("Місце і час створення заяви")]
        public string IsCreatedOnPortalDate => concat(ModifiedOnStr, IsCreatedOnPortalString);

        [NotMapped]
        public string NameOrgIPN=> concat(NameOrg, IPN);

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Найменування заявника і код ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        public string NameOrg { get; set; }

        [DisplayName("ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        [PredicateCase(PredicateOperation.Contains)]
        public string IPN { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Місто заявника")]
        public string CityName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Виконавець")]
        public string PerformerName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Експертиза")]
        public string ExpertiseResult { get; set; }
        
        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Експертиза")]
        public string ExpertiseResultEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Предліцензійна перевірка")]
        public int? ResultOfCheck { get; set; }
        public Guid? ResultOfCheckId { get; set; }

        public string ResultOfCheckString => ResultOfCheckId == null ? "" : ResultOfCheck == 0 || ResultOfCheck == null ? "Без порушень" : $"{ResultOfCheck} порушень";

        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayName("№ і дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDate { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string OrderNumber { get; set; }

        public string OrderDateNumber => concatDate(OrderDate, OrderNumber);

        public Guid? ProtocolId { get; set; }

        [PredicateCase]
        public string Koatuu { get; set; }

        [PredicateCase]
        public string EdocumentStatus { get; set; }

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

        private string concat(string first, string second) =>
            !string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second) ? first + " / " + second :
            string.IsNullOrEmpty(first) ? second :
            string.IsNullOrEmpty(second) ? first : "";
        private string concatDate(DateTime? first, string second) =>
            first != null && !string.IsNullOrEmpty(second) ? first.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + " / " + second :
            first == null ? second :
            string.IsNullOrEmpty(second) ? first.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) : "";
    }

    [RightsCheckList(nameof(PrlApplication))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlAppExpertiseDTO : BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        public string ExpertiseResult { get; set; }

        [DisplayName("Коментар")]
        public string ExpertiseComment { get; set; }

        [DisplayName("Результат експертизи")]
        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string ExpertiseResultEnum { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата експертизи")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public DateTime? ExpertiseDate { get; set; } = DateTime.Now;

        [DisplayName("Виконавець експертизи")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public Guid? PerformerOfExpertiseId { get; set; }

        [DisplayName("Виконавець експертизи")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [NotMapped]
        public string PerformerOfExpertiseName { get; set; }

        [DisplayName("Виконавець експертизи")]
        [NotMapped]
        public string PerformerName { get; set; }
    }
}
