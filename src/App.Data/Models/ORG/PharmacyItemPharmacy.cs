using System;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.ORG
{
    // Развязка между МПД(Аптечный пункт) и МПД(Аптека)
    public class PharmacyItemPharmacy: BaseEntity
    {
        // Branch whith type "Pharmacy" 
        public Guid PharmacyId { get; set; }

        // Branch whith type "PharmacyItem" 
        public Guid PharmacyItemId { get; set; }

        public Branch PharmacyItem { get; set; }

        public Branch Pharmacy { get; set; }
    }
}
