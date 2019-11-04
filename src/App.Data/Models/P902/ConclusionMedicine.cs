using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Data.Models.CRV;
using Newtonsoft.Json;

namespace App.Data.Models.P902
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConclusionMedicine: BaseEntity
    {
        [JsonProperty]
        public Guid ApplicationId { get; set; }

        public AppConclusion ConclusionApplication { get; set; }

        [JsonProperty]
        public string MedicineName { get; set; }

        [JsonProperty]
        public string FormName { get; set; }

        [JsonProperty]
        public string DoseInUnit { get; set; }

        [JsonProperty]
        public string NumberOfUnits { get; set; }

        [JsonProperty]
        public string MedicineNameEng { get; set; }

        [JsonProperty]
        public string RegisterNumber { get; set; }

        [JsonProperty]
        public string AtcCode { get; set; }

        [JsonProperty]
        public string ProducerName { get; set; }

        [JsonProperty]
        public string ProducerCountry { get; set; }

        [JsonProperty]
        public string SupplierName { get; set; }

        [JsonProperty]
        public string SupplierCountry { get; set; }

        [JsonProperty]
        public string SupplierAddress { get; set; }

        [JsonProperty]
        public bool IsFromLicense { get; set; }

        public LimsRP LimsRp { get; set; }

        [JsonProperty]
        public Guid LimsRpId { get; set; }

        [JsonProperty]
        public string Notes { get; set; }

        [JsonProperty]
        public long OLdDRugId { get; set; }

        [NotMapped]
        [JsonProperty]
        public Guid CreatedByJson { get; set; }
    }
}
