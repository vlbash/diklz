using System;
using System.ComponentModel;

namespace App.Data.DTO.LIMS
{
    public class LimsEndLicCheck
    {
        public int Id { get; set; }

        [DisplayName("Підстава")]
        public string EndReasonText { get; set; }

        [DisplayName("№ Наказу")]
        public string EndOrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        public DateTime? EndOrderDate { get; set; }

        [DisplayName("Текст наказу")]
        public string EndOrderText { get; set; }
    }
}
