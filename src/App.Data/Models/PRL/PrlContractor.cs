using App.Core.Data.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Models.PRL
{
    public class PrlContractor: BaseEntity
    {
        [StringLength(100, ErrorMessage = "Максимальна кількість символів - 100")]
        public string Name { get; set; }

        [StringLength(30, ErrorMessage = "Максимальна кількість символів - 30")]
        public string ContractorType { get; set; } //enum

        [StringLength(15, ErrorMessage = "Максимальна кількість символів - 9")]
        public string Edrpou { get; set; }

        [StringLength(200, ErrorMessage = "Максимальна кількість символів - 200")]
        public string Address { get; set; }
        public Guid AddressId { get; set; }

        public Guid? LicenseContractorId { get; set; }

        public bool? LicenseDeleteCheck { get; set; }

        public bool? IsFromLicense { get; set; }
    }
}
