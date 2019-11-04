using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;

namespace App.Data.DTO.LOG
{
    public class LogAuditListOfChangesDTO : BaseDTO
    {
        public int AuditEntryId { get; set; }

        [DisplayName("Назва властивості")]
        public string PropertyName { get; set; }

        [NotMapped]
        public string PropertyDisplayName { get; set; }

        [DisplayName("Нове значення")]
        public string NewValueFormatted { get; set; }

        [DisplayName("Старе значення")]
        public string OldValueFormatted { get; set; }

        public string EntityName { get; set; }
    }
}
