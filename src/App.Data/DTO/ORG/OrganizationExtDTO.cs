using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.DTO.Org;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.ORG
{
    public class OrganizationExtDetailDTO : BaseDTO/*, IOrgUnitDetailDTO*/
    {
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        public string EDRPOU { get; set; }

        public string INN { get; set; }

        [NotMapped]
        [DisplayName("Код ЄРДПОУ/ІНН Ліцензіата")]
        public string edrpouOrInn => !string.IsNullOrEmpty(EDRPOU) ? EDRPOU : INN;

        ////OrgOrganization
        //public string Parent { get; set; }

        //public Guid? ParentId { get; set; }
        
        //[MaxLength(250)]
        //public string OrgDirector { get; set; }

        ////Організаційно-правова форма
        //[MaxLength(255)]
        //public string LegalFormType { get; set; }

        //[MaxLength(255)]
        //public string OwnershipType { get; set; }
        
        //[DisplayName("Коментар")]
        //public string Description { get; set; }
        
        //[DisplayName("Телефон")]
        //[StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        //[RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        //[MaxLength(255)]
        //public string PhoneNumber { get; set; }

        //[DisplayName("Факс")]
        //[StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        //[RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        //public string FaxNumber { get; set; }

        //[MaxLength(20)]
        //public string PostIndex { get; set; }

        //[MaxLength(50)]
        //public string NationalAccount { get; set; }

        //[MaxLength(50)]
        //public string InternationalAccount { get; set; }

        //[MaxLength(255)]
        //public string NationalBankRequisites { get; set; }

        //[MaxLength(255)]
        //public string InternationalBankRequisites { get; set; }

        //[MaxLength(2)]
        //public string PassportSerial { get; set; }

        //[MaxLength(12)]
        //public string PassportNumber { get; set; }

        //public DateTime? PassportDate { get; set; }

        //[MaxLength(200)]
        //public string PassportIssueUnit { get; set; }
        
        //[DisplayName("Ел.адреса")]
        //[RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неприпустиме значення")]
        //public string EMail { get; set; }
        
        //[NotMapped]
        //public string _ReturnUrl { get; set; }
    }

    public class OrganizationExtListDTO: BaseDTO, IOrgUnitListDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        public string Parent { get; set; }

        [DisplayName("Телефон")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [DisplayName("Факс")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        public string FaxNumber { get; set; }

        [DisplayName("Ел.адреса")]
        [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неприпустиме значення")]
        public string EMail { get; set; }

        [MaxLength(250)]
        public string OrgDirector { get; set; }
    }
}
