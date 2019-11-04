using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Enums;

namespace App.Core.Eq.Entities
{
	public class EqBaseEntity : IEqBaseEntity
    {
        [Key]
        public long Id { get; set; }
        [DefaultValue(RecordState.N)]
        public RecordState RecordState { get; set; }
        [MaxLength(128)]
        public string Caption { get; set; }
        [MaxLength(128)]
        public string ModifiedBy { get; set; }
        [MaxLength(64)]
        public string ModifiedBy_Id { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(128)]
        public string CreatedBy { get; set; }
        [MaxLength(64)]
        public string CreatedBy_Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class Document : EqBaseEntity
    {
        [DisplayName("Номер документу")]
        public string RegNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата реєстрації")]
        public DateTime? RegDate { get; set; }
        public DocType DocTypes { get; set; }
        public string Discriminator { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }

        [NotMapped]
        [DisplayName("Назва")]
        public string Title => string.Format("№{0} від {1}", RegNumber, RegDate.HasValue ? RegDate.Value.ToString("dd.MM.yyyy") : "-");
    }
}
