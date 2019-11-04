using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;

namespace App.Data.Models.NTF
{
    public class Notification: BaseEntity
    {
        public Guid? ParentId { get; set; }

        [DisplayName("Дата створення сповіщення")]
        public DateTime? DateOfCreate { get; set; }

        [DisplayName("Тема сповіщення")]
        [MaxLength(300)]
        public string NotificationSubject { get; set; }

        [DisplayName("Вид сповіщення")]
        [MaxLength(10)]
        public string NotificationType { get; set; }

        [DisplayName("Текст сповіщення")]
        [MaxLength(10000)]
        public string NotificationText { get; set; }

        [DisplayName("Перелік отримувачів")]
        [MaxLength(10000)]
        public string RecipientJsonList { get; set; }

        [DisplayName("Тип сповіщення")]
        [MaxLength(100)]
        public string NotificationSort { get; set; }

        public bool? IsSend { get; set; }
    }
}
