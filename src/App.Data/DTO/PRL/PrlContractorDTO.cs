using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;
using App.Core.Data.DTO.Common;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using App.Core.Security;

namespace App.Data.DTO.PRL
{
    [RightsCheckList(nameof(PrlContractor))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlContractorDetailDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        [StringLength(100, ErrorMessage = "Максимальна кількість символів - 100")]
        [DisplayName("Найменування суб'єкта господарювання")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string Name { get; set; }

        [StringLength(30, ErrorMessage = "Максимальна кількість символів - 30")]
        [DisplayName("Тип контрагента")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string ContractorType { get; set; } //enum

        [DisplayName("Тип контрагента")]
        public string ContractorTypeName { get; set; }

        [StringLength(10, MinimumLength = 8, ErrorMessage = "Мінімальна кількість знаків - 8, максимальна - 10")]
        [DisplayName("ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string Edrpou { get; set; }

        [StringLength(200, ErrorMessage = "Максимальна кількість символів - 200")]
        [DisplayName("Найменування, місце провадження діяльності")]
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string Address { get; set; }

        public Guid LicenseContractorId { get; set; }

        public bool? LicenseDeleteCheck { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        [Required(ErrorMessage = "Мае бути обрано щонайменьше одне МПД")]
        public List<Guid> ListOfBranches { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames { get; set; }

        [NotMapped]
        public Guid? appId { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public string Sort { get; set; }
    }

    [RightsCheckList(nameof(PrlContractor))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PrlContractorListDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        [DisplayName("Найменування суб'єкта господарювання")]
        public string Name { get; set; }

        [DisplayName("Тип контрагента")]
        public string ContractorType { get; set; }

        [DisplayName("Найменування, місце провадження діяльності")]
        public string Address { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }

        public RecordState RecordState { get; set; }
    }
}
