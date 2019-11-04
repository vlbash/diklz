using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.APP
{
    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ApplicationListDTO : BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid OrgUnitId { get; set; }

        [DisplayName("Дата модифікації")]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Вид діяльності")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppType { get; set; }

        [DisplayName("Тип заяви")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppSort { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppTypeEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppSortEnum { get; set; }

        [DisplayName("Статус")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AppState { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string AppStateEnum { get; set; }

        [DisplayName("Виробництво (виготовлення) лікарських засобів в умовах аптеки")]
        public bool PrlInPharmacies { get; set; }
        [DisplayName("Оптова торгівля лікарськими засобами")]
        public bool WholesaleOfMedicines { get; set; }
        [DisplayName("Роздрібна торгівля лікарськими засобами")]
        public bool RetailOfMedicines { get; set; }
    }
}
