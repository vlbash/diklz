using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.PRL;

namespace App.Data.DTO.NTF
{
    [RightsCheckList(nameof(PrlApplication))]
    // [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]

    public class NotificationDetailsDTO: BaseDTO
    {
        // public Guid OrgUnitId { get; set; }

        [DisplayName("Дата створення сповіщення")]
        public DateTime? DateOfCreate { get; set; }

        [DisplayName("Тема сповіщення")]
        [MaxLength(300)]
        public string NotificationSubject { get; set; }

        [DisplayName("Текст сповіщення")]
        [MaxLength(10000)]
        public string NotificationText { get; set; }

        [DisplayName("Тип сповіщення")]
        [MaxLength(10)]
        public string NotificationType { get; set; }

        [DisplayName("Перелік отримувачів")]
        [MaxLength(10000)]
        public string RecipientJsonList { get; set; }

        [DisplayName("Користувач")]
        [MaxLength(300)]
        public string UserCreate { get; set; }

        [DisplayName("П.І.Б. користувача")]
        [MaxLength(300)]
        public string UserCreatePib { get; set; }

        [DisplayName("Стан відправки сповіщення")]
        public bool? IsSend { get; set; }
    }

    public class NotificationListDTO: BaseDTO, IPagingCounted
    {
        [DisplayName("Дата створення сповіщення")]
        public DateTime? DateOfCreate { get; set; }

        [DisplayName("Тема сповіщення")]
        [MaxLength(300)]
        public string NotificationSubject { get; set; }

        [DisplayName("Тип сповіщення")]
        [MaxLength(10)]
        public string NotificationType { get; set; }

        [DisplayName("Користувач")]
        [MaxLength(300)]
        public string UserCreate { get; set; }

        [DisplayName("П.І.Б. користувача")]
        [MaxLength(300)]
        public string UserCreatePib { get; set; }

        [DisplayName("Стан відправки сповіщення")]
        public bool? IsSend { get; set; }

        public int TotalRecordCount { get; set; }
    }
}
