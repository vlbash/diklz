using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.IML;
using App.Data.Models.ORG;

namespace App.Data.DTO.IML
{
    [RightsCheckList(nameof(ImlMedicine))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ImlMedicineDetailDTO: BaseDTO
    {
        public Guid ApplicationId { get; set; }

        public Guid OrgUnitId { get; set; }

        public bool IsFromLicense { get; set; }

        [Required, MaxLength(200), DisplayName("Торговельна назва")]
        public string MedicineName { get; set; }

        [Required, DisplayName("Форма випуску")]
        public string FormName { get; set; }

        [Required, MaxLength(200), DisplayName("Доза діючої речовини в кожній одиниці")]
        public string DoseInUnit { get; set; }

        [Required, MaxLength(100), DisplayName("Кількість одиниць в упаковці")]
        public string NumberOfUnits { get; set; }

        [Required, DisplayName("Міжнародна непатентована назва(МНН)")]
        public string MedicineNameEng { get; set; }

        [Required, MaxLength(200), DisplayName("Номер реєстраційного посвідчення в Україні")]
        public string RegisterNumber { get; set; }

        [Required, MaxLength(100), DisplayName("Код АТС")]
        public string AtcCode { get; set; }

        [Required, MaxLength(200), DisplayName("Найменування виробника")]
        public string ProducerName { get; set; }

        [DisplayName("Країна виробника")]
        public string ProducerCountry { get; set; }

        [Required, DisplayName("Найменування постачальника"), MaxLength(200)]
        public string SupplierName { get; set; }

        [DisplayName("Країна постачальника")]
        public string SupplierCountry { get; set; }

        public Guid LimsRpId { get; set; }

        [Required, DisplayName("Адреса постачальника")]
        public string SupplierAddress { get; set; }

        [DisplayName("Примітки"), MaxLength(1000)]
        public string Notes { get; set; }
    }

    [RightsCheckList(nameof(ImlMedicine))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ImlMedicineListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }


        [PredicateCase(PredicateOperation.Equals)]
        public Guid ApplicationId { get; set; }

        public Guid OrgUnitId { get; set; }

        [Required, MaxLength(200), DisplayName("Торговельна назва")]
        public string MedicineName { get; set; }

        [Required, MaxLength(200), DisplayName("№ РП ЛЗ")]
        public string RegisterNumber { get; set; }

        [Required, DisplayName("МНН")]
        public string MedicineNameEng { get; set; }

        [Required, MaxLength(200), DisplayName("Виробник")]
        public string ProducerName { get; set; }

        [Required, DisplayName("Постачальник"), MaxLength(200)]
        public string SupplierName { get; set; }

        public bool IsFromLicense { get; set; }
    }

    [RightsCheckList(nameof(ImlMedicine))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ImlMedicineListMsgDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }


        [PredicateCase(PredicateOperation.Equals)]
        public Guid ApplicationId { get; set; }

        public Guid OrgUnitId { get; set; }

        [Required, MaxLength(200), DisplayName("Торговельна назва"), PredicateCase(PredicateOperation.Contains)]
        public string MedicineName { get; set; }

        [Required, MaxLength(200), DisplayName("№ РП ЛЗ"), PredicateCase(PredicateOperation.Contains)]
        public string RegisterNumber { get; set; }

        [Required, DisplayName("МНН"), PredicateCase(PredicateOperation.Contains)]
        public string MedicineNameEng { get; set; }

        [Required, MaxLength(200), DisplayName("Виробник"), PredicateCase(PredicateOperation.Contains)]
        public string ProducerName { get; set; }

        [Required, DisplayName("Постачальник"), MaxLength(200), PredicateCase(PredicateOperation.Contains)]
        public string SupplierName { get; set; }

        public bool IsFromLicense { get; set; }

        public string MessageState { get; set; }

        [DisplayName("Старий постачальник"), NotMapped]
        public string OldName { get; set; }

        [DisplayName("Новий постачальник"), NotMapped]
        public string NewName { get; set; }

        public Guid? ParentId { get; set; }
    }

    [RightsCheckList(nameof(ImlMedicine))]
    public class ImlMedicineMinDTO: BaseDTO
    {
        [PredicateCase(PredicateOperation.Equals)]
        public Guid ApplicationId { get; set; }
    }

    public class ImlMedicineRegisterListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        
        [PredicateCase(PredicateOperation.Equals)]
        public Guid ApplicationId { get; set; }
        
        [Required, MaxLength(200), DisplayName("Торговельна назва")]
        public string MedicineName { get; set; }

        [Required, MaxLength(200), DisplayName("№ РП ЛЗ")]
        public string RegisterNumber { get; set; }

        [Required, DisplayName("МНН")]
        public string MedicineNameEng { get; set; }

        [Required, MaxLength(200), DisplayName("Виробник")]
        public string ProducerName { get; set; }

        [Required, DisplayName("Постачальник"), MaxLength(200)]
        public string SupplierName { get; set; }
    }
}
