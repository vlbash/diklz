using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Common.Attributes;
using App.Core.Data.Attributes;

namespace App.Core.Data.DTO.Common
{
    public class BaseDocumentDTO: BaseDTO
    {
        [DisplayName("Номер")]
        [Required]
        [PredicateCase(PredicateOperation.Contains)]
        public virtual string RegNumber { get; set; }

        [DisplayName("Дата")]
        [DocumentDate]
        [Required]
        [PredicateCase(PredicateOperation.ValueRange)]
        public virtual DateTime? RegDate { get; set; }

        [DisplayName("Дата")]
        [PredicateCase(PredicateOperation.Contains)]
        public virtual string RegDateCaption { get { return RegDate?.ToString("d"); } }

        public override string Title
        {
            get
            {
                if (string.IsNullOrEmpty(Caption)) {
                    var dateString = string.IsNullOrEmpty(RegDateCaption) ? "" : "від " + RegDateCaption;
                    return string.IsNullOrEmpty(RegNumber) ? dateString : RegNumber + " " + dateString;
                }
                else {
                    return Caption;
                }
            }
        }
    }
}
