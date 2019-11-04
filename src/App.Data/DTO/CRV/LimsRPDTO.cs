using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.CRV;

namespace App.Data.DTO.CRV
{
    [RightsCheckList(nameof(LimsRP))]
    public class LimsListRPDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        public int DocId { get; set; }

        [DisplayName("№ РП")]
        [PredicateCase(PredicateOperation.Contains)]
        public string RegNum { get; set; }

        [DisplayName("Тип")]
        public string RegProcCode { get; set; }

        [PredicateCase]
        public int? StateId { get; set; }

        [DisplayName("Стан дії РП")]
        public string StateName
        {
            get
            {
                switch (StateId)
                {
                    case 1:
                        return "Діє";
                    case 2:
                        return "Термін закінчився";
                    case 3:
                        return "Припинено дію";
                    default: return "";
                }
            }
        }

        [DisplayName("Дата початку дії РП")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [PredicateCase(PredicateOperation.ValueRange)]
        public DateTime? RegDate { get; set; }

        [DisplayName("Термін дії РП")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EndDate { get; set; }

        [DisplayName("№ наказу")]
        [PredicateCase(PredicateOperation.Contains)]
        public string OrdRegNum { get; set; }

        [DisplayName("Дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [PredicateCase(PredicateOperation.ValueRange)]
        public DateTime? OrdRegDate { get; set; }

        [DisplayName("Назва ЛЗ")]
        [PredicateCase(PredicateOperation.Contains)]
        public string DrugNameUkr { get; set; }

        [DisplayName("МНН")]
        [PredicateCase(PredicateOperation.Contains)]
        public string DrugNameEng { get; set; }

        [DisplayName("Форма випуску")]
        [PredicateCase(PredicateOperation.Contains)]
        public string FormTypeDesc { get; set; }

        [DisplayName("Клініко-фарм. група")]
        [PredicateCase(PredicateOperation.Contains)]
        public string FarmGroup { get; set; }

        [DisplayName("Виробник")]
        [PredicateCase(PredicateOperation.Contains)]
        public string ProducerName { get; set; }

        [DisplayName("Країна виробника")]
        public string CountryName { get; set; }

        [DisplayName("№ наказу про припинення дії РП")]
        public string OffOrderNum { get; set; }

        [DisplayName("Дата про припинення дії РП")]
        public DateTime? OffOrderDate { get; set; }

        [DisplayName("Код АТС")]
        [PredicateCase(PredicateOperation.Contains)]
        public string AtcCode { get; set; }
    }

    [RightsCheckList(nameof(LimsRP))]
    public class LimsRPMinDTO: BaseDTO
    {

        public string RegNum { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? OffOrderDate { get; set; }

    }

    [RightsCheckList(nameof(LimsRP))]
    public class LimsDetailsRPDTO: BaseDTO
    {
        public int DocId { get; set; }

        [DisplayName("№ РП")]
        public string RegNum { get; set; }

        [DisplayName("Тип")]
        public string RegProcCode { get; set; }

        public int? StateId { get; set; }

        [DisplayName("Стан дії РП")]
        public string StateName
        {
            get
            {
                switch (StateId)
                {
                    case 1:
                        return "Діє";
                    case 2:
                        return "Термін закінчився";
                    case 3:
                        return "Припинено дію";
                    default: return "";
                }
            }
        }

        [DisplayName("Дата початку дії РП")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? RegDate { get; set; }

        [DisplayName("Дата закінчення дії РП")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EndDate { get; set; }

        [DisplayName("№ наказу МОЗ")]
        public string OrdRegNum { get; set; }

        [DisplayName("Дата наказу МОЗ")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? OrdRegDate { get; set; }

        [DisplayName("Назва ЛЗ")]
        public string DrugNameUkr { get; set; }

        [DisplayName("МНН")]
        public string DrugNameEng { get; set; }

        [DisplayName("Тип препарату")]
        public string DrugClassName { get; set; }

        [DisplayName("Тип ЛЗ")]
        public string DrugTypeName { get; set; }

        [DisplayName("Тип форми випуску")]
        public string FormName { get; set; }

        [DisplayName("Форма випуску")]
        public string FormTypeDesc { get; set; }

        [DisplayName("Клініко-фарм. група")]
        public string FarmGroup { get; set; }

        [DisplayName("Виробник")]
        public string ProducerName { get; set; }

        [DisplayName("Заявник")]
        public string SideName { get; set; }

        [DisplayName("Країна виробника")]
        public string ProdCountryName { get; set; }

        [DisplayName("Виробник - резидент")]
        public bool IsResident { get; set; }

        [DisplayName("Тип реєстраційної процедури")]
        public string RegProcName { get; set; }

        [DisplayName("Реєстраційна процедура")]
        public string RegProcedure { get; set; }

        [DisplayName("Країна виробника")]
        public string CountryName { get; set; }

        [DisplayName("№ наказу про припинення дії РП")]
        public string OffOrderNum { get; set; }

        [DisplayName("Причина припинення дії РП")]
        public string OffReason { get; set; }

        [DisplayName("Дата наказу про припинення дії РП")]
        public DateTime? OffOrderDate { get; set; }

        [DisplayName("Склад діючих речовин")]
        public string ActiveSubstances { get; set; }

        [DisplayName("Умови відпуску")]
        public string SaleTerms { get; set; }

        [DisplayName("Рекламування")]
        public string PublicityInfo { get; set; }

        [DisplayName("Примітки")]
        public string Notes { get; set; }

        [DisplayName("Код АТС")]
        public string AtcCode { get; set; }
    }
}
