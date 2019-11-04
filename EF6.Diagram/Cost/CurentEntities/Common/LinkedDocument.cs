using Astum.Core.Data.Entities.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities.Common
{
    public class LinkedDocument: BaseEntity
    {
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }

        [DisplayName("Номер документу")]
        public string RegNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата")]
        public DateTime? RegDate { get; set; }

        [MaxLength(1024)]
        [DisplayName("Опис")]
        public string Description { get; set; }

        [DisplayName("Тип документу")]
        public string LinkedDocTypeEnum { get; set; }

        [DisplayName("Стан")]
        public string CardStatusEnum { get; set; }
    }
}
