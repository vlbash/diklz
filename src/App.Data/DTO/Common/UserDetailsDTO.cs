using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.ORG;

namespace App.Data.DTO.Common
{
    [RightsCheckList(nameof(EmployeeExt))]
    public class UserDetailsDTO: BaseDTO
    {
        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        [DisplayName("ПІБ")]
        public string FIO => $"{LastName} {(string.IsNullOrEmpty(Name) ? " " : Name)} {(string.IsNullOrEmpty(MiddleName) ? " " : MiddleName)}";

        [DisplayName("Структурний підрозділ")]
        public string OrgUnitName { get; set; }

        [DisplayName("Посада")]
        public string Position { get; set; }

        [DisplayName("E-mail")]
        public string EMail { get; set; }

        [DisplayName("ЕДРПОУ")]
        public string Edrpou { get; set; }

        //[DisplayName("Електронно цифрофий підпис")]
        //public string DigSign { get; set; }
    }
}
