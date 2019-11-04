using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.ORG;
using App.Data.Models.P902;

namespace App.Data.DTO.P902
{
    public class AppConclusionDetailDTO
    {
        [DisplayName("№ заяви")]
        public string DocNum { get; set; }

        [DisplayName("Дата заяви")]
        public string RegDate { get; set; }

        [DisplayName("Уповноважена особа")]
        public string Assigne { get; set; }

        [DisplayName("ТДС в яку подано звіт")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string TeritorialService { get; set; }

        [DisplayName("ТДС в яку подано звіт")]
        public string TeritorialServiceEnumName { get; set; }
    }

    [RightsCheckList(nameof(AppConclusion))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))] 
    public class AppConclusionListDTO: BaseDTO, IPagingCounted
    {
           //public Guid Id { get; set; }
           public string Caption { get; set; }

           [PredicateCase(PredicateOperation.InputRange)]
           [DataType(DataType.Date)]
           [DisplayName("Дата створення")]
           [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
           public DateTime CreatedOn { get; set; }

           [DisplayName("Дата модифікації")]
           [PredicateCase(PredicateOperation.ValueRange)]
           [DataType(DataType.Date)]
           [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
           public DateTime? ModifiedOn { get; set; }

           public Guid OrgUnitId { get; set; }

           [DisplayName("№ заяви")]
           public string DocNum { get; set; }


           [DisplayName("Дата заяви")]
           public string RegDate { get; set; }

           [DisplayName ("Статус заяви")]
           public string AppConclusionStatus { get; set; }

           public string Name { get; set; }
           public int TotalRecordCount { get; set; }


    }
}
