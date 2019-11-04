using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник поштових відділень")]
    [Display(Name = "Довідник поштових відділень")]
    [Table("AtuPostOffice")]
    public abstract class BasePostOffice: BaseEntity
    {
        [Display(Name = "Код")]
        public virtual int Code { get; set; }

        [MaxLength(256)]
        [Display(Name = "Назва відділення")]
        public virtual string Name { get; set; }

        [MaxLength(5)]
        [Display(Name = "Індекс")]
        public virtual string PostIndex { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Адреса")]
        public virtual string Address { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Контактні дані")]
        public virtual string Contacts { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Коментар")]
        public virtual string Comment { get; set; }

        [Display(Name = "Дія")]
        [DefaultValue(true)]
        public virtual bool IsWorking { get; set; }

        // TODO: This logic should be implemented in caption
        [NotMapped]
        public virtual string NameAndIndex =>
            $"{(string.IsNullOrEmpty(Name) ? " " : Name)} {(string.IsNullOrEmpty(PostIndex) ? " " : PostIndex)}";
    }
}
