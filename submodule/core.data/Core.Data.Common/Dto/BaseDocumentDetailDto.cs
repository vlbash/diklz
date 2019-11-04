using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;

namespace Core.Data.Common.Dto
{
    public abstract class BaseDocumentDetailDto : BaseDto
    {
        [Display(Name = "Номер")]
        public virtual string RegNumber { get; set; }
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? RegDate { get; set; }
        public virtual string Description { get; set; }
    }
}
