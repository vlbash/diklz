using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;

namespace App.Data.Models
{
    public abstract class BaseLicense: BaseEntity
    {
        //enum ActivityType
        [MaxLength(30)]
        public string LicType { get; set; }

        [MaxLength(40)]
        public string LicSort { get; set; }

        [MaxLength(30)]
        public string LicState { get; set; }

        #region Реєстраційні дані

        [DisplayName("Номер ліцензії")]
        public string LicenseNumber { get; set; }

        //в карточке решения
        [DisplayName("Дата початку дії ліцензії")]
        public DateTime LicenseDate { get; set; }

        //протокол
        [DisplayName("Номер наказу")]
        public string OrderNumber { get; set; }

        //протокол
        [DisplayName("Дата наказу")]
        public DateTime OrderDate { get; set; }

        // property for checking license on relevance information
        // false - license has new version
        // true - last version of license
        public bool IsRelevant { get; set; }

        #endregion

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
