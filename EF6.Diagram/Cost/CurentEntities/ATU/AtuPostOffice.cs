using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.ATU
{
    [Table("AtuPostOffice")]
    public class AtuPostOffice: BaseEntity
    {
        [DisplayName("Код")]
        public int Code { get; set; }

        [MaxLength(256)]
        [DisplayName("Назва відділення")]
        public string Name { get; set; }

        [MaxLength(5)]
        [DisplayName("Індекс")]
        public string PostIndex { get; set; }

        [MaxLength(1000)]
        [DisplayName("Адреса")]
        public string Address { get; set; }

        [DisplayName("Райони м. Києва")]
        public List<AtuDistrict> Dictricts { get; set; }

        [MaxLength(1000)]
        [DisplayName("Контактні дані")]
        public string Contacts { get; set; }

        [MaxLength(2000)]
        [DisplayName("Коментар")]
        public string Comment { get; set; }

        [DisplayName("Дія")]
        [DefaultValue(true)]
        public bool IsWorking { get; set; }

        [NotMapped]
        public string NameAndIndex => String.Format("{0} {1}", (String.IsNullOrEmpty(Name) ? " " : Name), (String.IsNullOrEmpty(PostIndex) ? " " : PostIndex));
    }
}
