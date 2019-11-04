using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.DTO.Common
{
    public class DocumentDetailDTO : BaseDTO
    {
        [DisplayName("Номер")]
        public string RegNumber { get; set; }
        [DisplayName("Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }
        public string Description { get; set; }
    }
}
