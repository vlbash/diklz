using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.CustomAudit;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.ORG;
using Z.EF.Plus.BatchUpdate.Shared.Extensions;
using Z.EntityFramework.Plus;


namespace App.Data.DTO.LOG
{
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class LogAuditEntryListDTO: BaseDTO, IPagingCounted
    {
        public Guid OrgUnitId { get; set; }
        [DisplayName("ID")]
        public int AuditEntryId { get; set; }

        public int TotalRecordCount { get; set; }

        [DisplayName("Id"), PredicateCase]
        public string EntityId { get; set; }

        [DisplayName("Об'єкт")]
        [PredicateCase(PredicateOperation.Equals)]
        public string EntityTypeNameCode { get; set; }

        [DisplayName("Назва")]
        public string EntityTypeName { get; set; }

        [DisplayName("Користувач")]
        [PredicateCase(PredicateOperation.Contains)]
        public string CreatedBy { get; set; }

        [DisplayName("Дата події")]
        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Стан")]
        [PredicateCase(PredicateOperation.Equals)]
        public AuditEntryState State { get; set; }

        [DisplayName("Подія")]
        [NotMapped]
        public string StateNameUa { get { return AuditHelper.AuditEntryStateUa.GetValueOrNull(this.State); } }

    }
}
