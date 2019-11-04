using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;
using Astum.Core.Data.Interfaces;
using Z.EntityFramework.Plus;

namespace Astum.Core.Data.Entities.DTM
{
    [AuditInclude]
    [AuditDisplay("DtmTemplate")]
    public class DtmTemplate : BaseEntity
    {
        [DisplayName("Код")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Code { get; set; }

        [DisplayName("Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }
    }
}
