using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.DOS
{
    public class EDocument: BaseEntity
    {
        [MaxLength(30)]
        [DisplayName("Номер картки")]
        public string CardNumber { get; set; }

        [DisplayName("Дата з")]
        public DateTime? DateFrom { get; set; }

        [DisplayName("Дата до")]
        public DateTime? DateTo { get; set; }

        [MaxLength(30)]
        [DisplayName("Версія досьє")]
        public string Version { get; set; }

        [MaxLength(30)]
        [DisplayName("Стан досьє")]
        public string EDocumentStatus { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        public string EDocumentType { get; set; }

        public bool? IsFromLicense { get; set; }

        public Guid? EntityId { get; set; }
        public string EntityName { get; set; }
    }
}
