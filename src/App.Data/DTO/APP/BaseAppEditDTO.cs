using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Core.Data.DTO.Common;
using App.Data.Enums;

namespace App.Data.DTO.APP
{
    public class BaseAppDetailDTO: BaseAppEditDTO
    {
        public string AppType { get; set; }

        [DisplayName("Серія та номер пасспорту \\ ID картки")]
        public string PassportFull =>
            !string.IsNullOrEmpty(PassportSerial) ? PassportSerial + " " + PassportNumber : PassportNumber;

        [DisplayName("Організаційно-правова форма")]
        [StringLength(50, ErrorMessage = "Максимальна кількість символів - 50")]
        public string LegalFormName { get; set; }

        [DisplayName("Форма власності")]
        public string OwnershipTypeName { get; set; }

        [DisplayName("Код економічної класифікації")]
        public string EconomicClassificationTypeName { get; set; }

        [DisplayName("Вид діяльності")]
        public string ActivityTypeName { get; set; }

        [DisplayName("Прошу за місцем/місцями провадження господарської діяльності" +
                     " провести перевірку матеріально-технічної бази, кваліфікованого персоналу, а " +
                     "також умов щодо контролю якості лікарських засобів, що вироблятимуться")]
        public bool IsCheckMpd { get; set; }

        [DisplayName("Додатково до електронної форми бажаю отримати ліцензію на паперовому носії")]
        public bool IsPaperLicense { get; set; }

        [DisplayName("Нарочно")]
        public bool IsCourierDelivery { get; set; }

        [DisplayName("Поштовим відправленням за місцезнаходженням/місцем проживання")]
        public bool IsPostDelivery { get; set; }

        [DisplayName("З порядком отримання ліцензії ознайомлений. Ліцензійним умовам провадження" +
                     " господарської діяльності з виробництва лікарських засобів, оптової та " +
                     "роздрібної торгівлі лікарськими засобами, імпорту лікарських засобів(крім " +
                     "активних фармацевтичних інгредієнтів) відповідаю і зобов’язуюсь їх виконувати.")]
        public bool IsAgreeLicenseTerms { get; set; }

        [DisplayName("Згоден на обробку персональних даних з метою забезпечення виконання" +
                     " вимог Закону України “Про ліцензування видів господарської " +
                     "діяльності” (для фізичної особи - підприємця)")]
        public bool IsAgreeProcessingData { get; set; }

        [DisplayName("На виконання вимог Закону України \"Про ліцензування видів господарської діяльності\"" +
                     " інформуємо про відсутність контролю (у значенні наведеному в статті 1 Закону України \"Про " +
                     "захист економічної конкуренції\") за діяльністю осіб - резидентів інших держав, що здійснюють" +
                     " збройну агресію проти України (у значенні, наведеному у статті 1 Закону України \"Про оборону" +
                     " України\") та\\або дії яких створюють умови для виникнення воєнного конфлікту " +
                     "та застосування воєнної сили проти України.")]
        public bool IsProtectionFromAggressors { get; set; }

        [DisplayName("Нарочно")]
        public bool IsCourierResults { get; set; }

        [DisplayName("Поштовим відправленням за місцезнаходженням/місцем проживання")]
        public bool IsPostResults { get; set; }

        [DisplayName("В електронному вигляді")]
        public bool IsElectricFormResults { get; set; }

        [DisplayName("Розгляд заяви")]
        [StringLength(30, ErrorMessage = "Максимальна кількість символів - 100")]
        public string AppState { get; set; }

        [DisplayName("Розгляд заяви")]
        public string BackOfficeAppState { get; set; }

        [DisplayName("Розгляд заяви")]
        public string BackOfficeAppStateString { get; set; }

        [DisplayName("Примітки/Коментар до заяви:")]
        public string Comment { get; set; }

        [DisplayName("Тип рішення")]
        public string DecisionType { get; set; }

        [DisplayName("Виконавець заяви")]
        public Guid? PerformerId { get; set; }

        [DisplayName("Номер реєстрації")]
        public string RegNumber { get; set; }

        [DisplayName("Дата реєстрації")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }

        [DisplayName("Результат експертизи")]
        public string ExpertiseResultEnum { get; set; }

        [DisplayName("Результат експертизи")]
        public string ExpertiseResult { get; set; }

        [DisplayName("Дата експертизи")]
        public DateTime? ExpertiseDate { get; set; }

        [DisplayName("Виконавець експертизи")]
        public Guid? PerformerOfExpertise { get; set; }

        [DisplayName("Підстава")]
        public bool IsCreatedOnPortal { get; set; }

        public string ErrorProcessingLicense { get; set; }

        [DisplayName("Виробництво (виготовлення) лікарських засобів в умовах аптеки")]
        public bool PrlInPharmacies { get; set; }
        [DisplayName("Оптова торгівля лікарськими засобами")]
        public bool WholesaleOfMedicines { get; set; }
        [DisplayName("Роздрібна торгівля лікарськими засобами")]
        public bool RetailOfMedicines { get; set; }

        #region iml

        [DisplayName("Імпорт зареєстрованих готових лікарських засобів")]
        public bool IMLIsImportingFinished { get; set; }

        [DisplayName("Імпорт зареєстрованих лікарських засобів у формі “in bulk” (продукції “in bulk”)")]
        public bool IMLIsImportingInBulk { get; set; }

        [DisplayName("Інша діяльність з імпорту лікарських засобів (будь-яка інша діяльність, не зазначена вище, зазначити за наявності):")]
        [MaxLength(255)]
        public string IMLAnotherActivity { get; set; }

        [DisplayName("Наявні умови щодо контролю якості лікарських засобів, які будуть ввозитися на територію України")]
        public bool IsConditionsForControl { get; set; }

        [DisplayName("Виробництво лікарських засобів, які планується ввозити на територію України, відповідає вимогам щодо належної виробничої практики лікарських засобів")]
        public bool IsGoodManufacturingPractice { get; set; }
        #endregion
    }

    public class BaseAppEditDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }
        public Guid OrganizationInfoId { get; set; }

        [Display(Name = "Найменування юридичної особи / ПІБ ФОП")]
        public string OrgName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Код згідно з ЄДРПОУ")]
        public string EDRPOU { get; set; }

        [DisplayName("Реєстраційний номер облікової картки платника податків")]
        [StringLength(20)]
        public string INN { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Організаційно-правова форма")]
        [StringLength(255, ErrorMessage = "Максимальна кількість символів - 255")]
        public string LegalFormType { get; set; }

        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Код економічної класифікації")]
        public string EconomicClassificationType { get; set; }

        [DisplayName("Серія паспорта")]
        [StringLength(2, ErrorMessage = "Максимальна кількість символів - 2")]
        public string PassportSerial { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Номер паспорта \\ ID картки")]
        [StringLength(9, ErrorMessage = "Максимальна кількість символів - 9")]
        public string PassportNumber { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Форма власності")]
        [StringLength(255, ErrorMessage = "Максимальна кількість символів - 255")]
        public string OwnershipType { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [DisplayName("Дата видачі")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public DateTime? PassportDate { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Орган, що видав паспорт")]
        [StringLength(200, ErrorMessage = "Максимальна кількість символів - 200")]
        public string PassportIssueUnit { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [StringLength(250, ErrorMessage = "Максимальна кількість символів - 250")]
        public string OrgDirector { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "Максимальна кількість символів - 100")]
        [EmailAddress(ErrorMessage = "Не вірний формат Email")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Phone(ErrorMessage = "Не правильний формат номеру телефону")]
        [StringLength(25, ErrorMessage = "Максимальна кількість символів - 25")]
        [DisplayName("Номер телефону")]
        public string PhoneNumber { get; set; }

        [Phone(ErrorMessage = "Не правильний формат номеру факсу")]
        [DisplayName("Номер факсу")]
        [StringLength(20, ErrorMessage = "Максимальна кількість символів - 20")]
        public string FaxNumber { get; set; }

        #region ATU

        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }

        [DisplayName("Вулиця")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")]
        public string CityName { get; set; }
        public string CityEnum { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поле має мати 5 символів")]
        public string PostIndex { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер будинку, корпус або будівля, номер квартири або офісу")]
        public string Building { get; set; }
        public string AddressType { get; set; }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public Guid AddressId { get; set; }

        public string CityFullName
        {
            get
            {
                var district = DistrictName;
                if (!string.IsNullOrEmpty(DistrictName) &&
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName, StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                return $"{(string.IsNullOrEmpty(RegionName) ? "" : $"{RegionName}, ")}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{CityName}";
            }
        }

        [DisplayName("Код КОАТУУ")]
        public string KoatuuCode { get; set; }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public string Address
        {
            get
            {
                if (AddressId == Guid.Empty)
                    return "";
                var district = !string.IsNullOrEmpty(DistrictName) ? $"{DistrictName} ," : "";
                if (!string.IsNullOrEmpty(DistrictName) &&
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName, StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                var cityType = "";
                switch (CityEnum)
                {
                    case "Village":
                        cityType = "с.";
                        break;
                    case "Hamlet":
                        cityType = "с-ще ";
                        break;
                    case "UrbanTypeVillages":
                        cityType = "смт ";
                        break;
                    case "TownsOfDistrictSubordination":
                        cityType = "м.";
                        break;
                    case "CitiesOfRegionalSubordination":
                        cityType = "м.";
                        break;
                }
                var addressType = "";
                switch (AddressType)
                {
                    case "Street":
                        addressType = "вул.";
                        break;
                    case "Lane":
                        addressType = "пров.";
                        break;
                    case "Boulevard":
                        addressType = "б-р ";
                        break;
                    case "Avenue":
                        addressType = "просп.";
                        break;
                    case "Square":
                        addressType = "площа ";
                        break;
                }

                return $"{RegionName}, {district}{cityType}{CityName}, {addressType} {StreetName}, {Building}";
            }
        }

        #endregion

        [DisplayName("Номер рахунку в національній валюті")]
        [StringLength(50, ErrorMessage = "Максимальна кількість символів - 50")]
        public string NationalAccount { get; set; }

        [DisplayName("Реквізити банку з рахунком в національній валюті")]
        public string NationalBankRequisites { get; set; }

        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Номер рахунку в іноземній валюті")]
        [StringLength(50, ErrorMessage = "Максимальна кількість символів - 50")]
        public string InternationalAccount { get; set; }

        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Реквізити банку з рахунком в іноземній валюті")]
        [StringLength(255, ErrorMessage = "Максимальна кількість символів - 255")]
        public string InternationalBankRequisites { get; set; }

        [DisplayName("D-U-N-S номер (за наявності)")]
        [StringLength(24, ErrorMessage = "Максимальна кількість символів - 24")]
        public string Duns { get; set; }

        public string AppSort { get; set; }

        [NotMapped]
        public OrgType OrgType
        {
            get { return string.IsNullOrEmpty(EDRPOU) ? OrgType.FOP : OrgType.Organization; }
            set { }
        }
    }
}
