using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.DOS;
using App.Data.Models.ORG;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace App.Data.DTO.DOS
{
    [RightsCheckList(nameof(EDocument))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class EDocumentDetailsDTO: BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        [MaxLength(30)]
        [DisplayName("Номер картки")]
        public string CardNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата з")]
        public DateTime? DateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата до")]
        public DateTime? DateTo { get; set; }

        [MaxLength(30)]
        [DisplayName("Назва/Версія")]
        public string Version { get; set; }

        [MaxLength(12)]
        [DisplayName("Стан досьє")]
        public string EDocumentStatus { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        public string EDocumentType { get; set; }

        public Guid? EntityId { get; set; }
        public string EntityName { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<Guid> ListOfBranches { get; set; }
        
        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public Guid? BranchId { get; set; }

        [NotMapped]
        public Guid ApplicationId { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public string AppSort { get; set; }

        [NotMapped]
        public string AppType { get; set; }

    }

    [RightsCheckList(nameof(EDocument))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class EDocumentDetailsMsgDTO : BaseDTO
    {
        public Guid OrgUnitId { get; set; }

        [MaxLength(30)]
        [DisplayName("Номер картки")]
        public string CardNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата з")]
        public DateTime? DateFrom { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата до")]
        public DateTime? DateTo { get; set; }

        [MaxLength(30)]
        [DisplayName("Назва/Версія")]
        public string Version { get; set; }

        [MaxLength(12)]
        [DisplayName("Стан досьє")]
        public string EDocumentStatus { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        public string EDocumentType { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<Guid> ListOfBranches { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranchsNames { get; set; }

        [NotMapped]
        public Guid MessageId { get; set; }

    }

    [RightsCheckList(nameof(EDocument))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class EDocumentListDTO: BaseDTO
    {
        [DisplayName("Назва/Версія")]
        public string Version { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranches { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }

        [NotMapped]
        public bool IsFromMessage { get; set; }

        [DisplayName("Опис")]
        public string Description { get; set; }
    }

    [RightsCheckList(nameof(EDocument))]
    public class EDocumentPaymentListDTO: BaseDTO
    {
        public DateTime CreatedOn { get; set; }
        [DisplayName("Коментар до платежу")]
        public string Comment { get; set; }
        public string EdocumentType { get; set; }

        [DisplayName("Статус")]
        public string EdocumentStatus { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }

        [DisplayName("Опис")]
        public string Description { get; set; }

        public Guid? EntityId { get; set; }

        public string EntityName { get; set; }
    }

    [RightsCheckList(nameof(EDocument))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class RegisterEDocumentListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва/Версія")]
        public string Version { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("МПД")]
        [NotMapped]
        public List<string> ListOfBranches { get; set; }

        public bool? IsFromLicense { get; set; }

        [NotMapped]
        public bool IsEditable { get; set; }

        [DisplayName("Номер картки")]
        public string CardNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата з")]
        public DateTime? DateFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата до")]
        public DateTime? DateTo { get; set; }

        [DisplayName("Стан досьє")]
        public string EDocumentStatus { get; set; }

        public string EDocumentType { get; set; }

        public Guid AppId { get; set; }
    }


}
