using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник поштових відділень")]
    [Display(Name = "Довідник поштових відділень")]
    [Table("Atu" + nameof(PostOffices))]
    public class PostOffices: BaseEntity
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

        [DisplayName("Райони міста")]
        public List<CityDistricts> Dictricts { get; set; }

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
        public string NameAndIndex =>
            $"{(string.IsNullOrEmpty(Name) ? " " : Name)} {(string.IsNullOrEmpty(PostIndex) ? " " : PostIndex)}";
    }
}
