using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.ORG;
using App.Data.Models.P902;
using App.Data.Models.PRL;

namespace App.Data.DTO.P902
{
    [RightsCheckList(nameof(ResultInputControl))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    [DisplayName("Результат вхідного контролю субстанцій ")]
    public class ResultInputControlDetailsDTO: BaseDTO
    {
        [DisplayName("Стан")]
        public string State { get; set; }

        public string StateLimsId { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }
        
        [DisplayName("Стан")]
        public string StateEnumName{ get; set; }

        [DisplayName("ТДС в яку подано звіт")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string TeritorialService { get; set; }
        
        [DisplayName("ТДС в яку подано звіт")]
        public string TeritorialServiceEnumName { get; set; }

        public Guid LicenseId { get; set; }

        [DisplayName("Ліцензія СГД")]
        public string LicenseString { get; set; }

        public long? OldLimsId { get; set; }

        [DisplayName("ЄДРПОУ СГД")]
        public string Edrpou { get; set; }

        [DisplayName("Назва СГД")]
        public string OrgName { get; set; }

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

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        public string DistrictName { get; set; }


        public string AddressType { get; set; }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public Guid AddressId { get; set; }

        public string Building { get; set; }

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

        [DisplayName("Юридична адреса")]
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

        [DisplayName("Посилання на запис в реєстрі РП")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid LimsRPId { get; set; }

        public long DocId { get; set; }
        
        [DisplayName("№ РП ЛЗ")]
        public string RegisterNumber { get; set; }

        [DisplayName("Термін дії РП")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayName("Назва ЛЗ")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string DrugName { get; set; }

        [DisplayName("Форма випуску")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string DrugForm { get; set; }

        [DisplayName("Виробник")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string ProducerName { get; set; }

        [DisplayName("Країна виробника")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string ProducerCountry { get; set; }

        [DisplayName("№ серії")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string MedicineSeries { get; set; }

        [DisplayName("Термін придатності ЛЗ")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MedicineExpirationDate { get; set; }

        [DisplayName("Розмір серії")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string SizeOfSeries { get; set; }

        [DisplayName("Одиниці вимірювання")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string UnitOfMeasurement { get; set; }

        public string UnitOfMeasurementLimsId { get; set; }
        
        [DisplayName("Одиниці вимірювання")]
        public string UnitOfMeasurementEnumName { get; set; }

        [DisplayName("Кількість ввезеного ЛЗ")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string AmountOfImportedMedicine { get; set; }

        [DisplayName("№ ВМД")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string WinNumber { get; set; }

        [DisplayName("Дата ВМД")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateWin { get; set; }

        [DisplayName("Результат вхідного контролю")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string InputControlResult { get; set; }
        
        [DisplayName("Результат вхідного контролю")]
        public string InputControlResultEnumName { get; set; }

        public string InputControlResultLimsId { get; set; }

        [DisplayName("Назва показника невідповідності АНД")]
        public string NameOfMismatch { get; set; }

        [DisplayName("Примітки")]
        public string Comment { get; set; }
    }

    [RightsCheckList(nameof(ResultInputControl))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ResultInputControlListDTO: BaseDTO, IPagingCounted
    {
        public Guid OrgUnitId { get; set; }

        public int TotalRecordCount { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayName("Дата створення")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Стан")]
        public string State { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Стан")]
        public string StateEnumName { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата ВМД")]
        public DateTime? DateWin { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Назва ЛЗ")]
        public string DrugName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("№ серії")]
        public string MedicineSeries { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Розмір серії")]
        public string SizeOfSeries { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Кількість ввезеного ЛЗ")]
        public string AmountOfImportedMedicine { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Результат вхідного контролю")]
        public string InputControlResult { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Результат вхідного контролю")]
        public string InputControlResultEnumName { get; set; }

        [DisplayName("Відправка результатів")]
        public bool SendCheck { get; set; }
    }
}
