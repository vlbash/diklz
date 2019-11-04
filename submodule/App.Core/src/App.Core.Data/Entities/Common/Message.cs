using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.Common
{
    [AuditInclude]
    [AuditDisplay(name: "Повідомлення")]
    [Display(Name = "Повідомлення")]
    public class Message : BaseEntity
    {
        public Message()
        {
            Children = new HashSet<Message>();
        }

        [DisplayName("Посилання на пов’язане повідомлення")]
        public Guid? ParentId { get; set; }
        public Message Parent { get; set; }
        public ICollection<Message> Children { get; set; }

        [DisplayName("Message entity Id")]
        public Guid? EntityId { get; set; }
        [DisplayName("Message entity name")]
        public string EntityName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата повідомлення")]
        public DateTime MsgDate { get; set; }
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Статус повідомлення")]
        public string MsgStatus { get; set; } // EnumRecord code - MsgStatus
        
        [DisplayName("Тип повіомлення")]
        public string MsgType { get; set; } // EnumRecord -  MsgType 

        [DisplayName("Повідомлення продивленно (так/ні)")]
        [DefaultValue(false)]
        public bool MsgViewed{ get; set; } 
        
        [DisplayName("Відправвник")]
        public Guid SenderId { get; set; }
        public Person Sender { get; set; } 

        [DisplayName("Отримувач")]
        public Guid? ReceiverId { get; set; }
        public Person Receiver { get; set; }

        [DisplayName("Посада отримувача")]
        public string ReceiverPositionType { get; set; }

        [StringLength(100)]
        [DisplayName("Заголовок")]
        public string Header { get; set; }

        [StringLength(4000)]
        [DisplayName("Текст повідомлення")]
        public string MsgText { get; set; }
    }
}
