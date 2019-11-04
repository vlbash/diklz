using System;
using System.Collections.Generic;
using System.Text;

namespace App.Data.DTO.LIMS
{
    public class LimsOldRP
    {
        public int DocId { get; set; }
        public string RegNum { get; set; }
        public string RegProcCode { get; set; }
        public int? StateId { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OrdRegNum { get; set; }
        public DateTime? OrdRegDate { get; set; }
        public string DrugNameUkr { get; set; }
        public string DrugNameEng { get; set; }
        public int? FormTypeId { get; set; }
        public string FormtypeDesc { get; set; }
        public string FormName { get; set; }
        public string FarmGroup { get; set; }
        public string SideName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string ProducerName { get; set; }
        public string ProdCountryName { get; set; }
        public bool IsResident { get; set; }
        public string RegProcedure { get; set; }
        public int? RegprocId { get; set; }
        public string RegprocName { get; set; }
        public string RegprocCode { get; set; }
        public int? DrugtypeId { get; set; }
        public string DrugtypeName { get; set; }
        public int? RpOrderId { get; set; }
        public string OffOrderNum { get; set; }
        public DateTime? OffOrderDate { get; set; }
        public string OffReason { get; set; }
        public int? DrugClassId { get; set; }
        public string DrugClassName { get; set; }
        public string AtcCode { get; set; }
        public string ActiveSubstances { get; set; }
        public string SaleTerms { get; set; }
        public string PublicityInfo { get; set; }
        public string Notes { get; set; }
    }
}
