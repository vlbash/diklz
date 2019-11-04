using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Data.Models.ORG
{
    [AuditInclude]
    [AuditDisplay("Інформація про організацію")]
    [Table("Org" + nameof(OrganizationInfo))]
    public class OrganizationInfo: BaseEntity
    {
        public Guid OrganizationId { get; set; }

        public OrganizationExt Organization { get; set; }

        [DisplayName("Назва")]
        public string Name { get; set; }

        [MaxLength(10)]
        public string Type { get; set; }

        [MaxLength(250)]
        public string OrgDirector { get; set; }

        //Організаційно-правова форма
        [MaxLength(255)]
        public string LegalFormType { get; set; }

        //Код економічної класифікації
        [MaxLength(255)]
        public string EconomicClassificationType { get; set; }

        [MaxLength(255)]
        public string OwnershipType { get; set; }

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string FaxNumber { get; set; }

        public Guid AddressId { get; set; }

        [MaxLength(50)]
        public string NationalAccount { get; set; }

        [MaxLength(50)]
        public string InternationalAccount { get; set; }

        [MaxLength(255)]
        public string NationalBankRequisites { get; set; }

        [MaxLength(255)]
        public string InternationalBankRequisites { get; set; }

        [MaxLength(2)]
        public string PassportSerial { get; set; }

        [MaxLength(12)]
        public string PassportNumber { get; set; }

        public DateTime? PassportDate { get; set; }

        [MaxLength(200)]
        public string PassportIssueUnit { get; set; }

        public bool IsActualInfo { get; set; }

        public bool IsPendingLicenseUpdate { get; set; }

        [MaxLength(100)]
        public string EMail { get; set; }
    }
}
