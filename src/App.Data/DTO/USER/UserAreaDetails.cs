using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models.ORG;

namespace App.Data.DTO.USER
{
    [RightsCheckList(nameof(EmployeeExt))]
    public class UserAreaDetailDTO: BaseDTO
    {
        public Guid? OrgEmployeeId { get; set; }

        public Guid? PersonId { get; set; }

        public string PIB { get; set; }

        [DisplayName("Посада")]
        public string Position { get; set; }

        [DisplayName("Електронна пошта користувача")]
        public string UserEmail { get; set; }

        [DisplayName("Електронна пошта компанії")]
        public string OrgEmail { get; set; }

        [DisplayName("Контактний телефон")]
        public string Phone { get; set; }

        [DisplayName("Індивідуальний податковий номер")]
        public string INN { get; set; }

        [DisplayName("Код ЕДРПОУ компанії")]
        public string EDRPOU { get; set; }
        
        [DisplayName("Отримувати сповіщення про зміну стану всіх заяв, поданих співробітниками компанії")]
        public bool ReceiveOnChangeAllApplication { get; set; }

        [DisplayName("Отримувати сповіщення про зміну стану повідомленнь, поданих співробітниками компанії")]
        public bool ReceiveOnChangeAllMessage { get; set; }                
        
        [DisplayName("Отримувати сповіщення про зміну стану всіх заяв, поданих власноруч")]
        public bool ReceiveOnChangeOwnApplication { get; set; }

        [DisplayName("Отримувати сповіщення про зміну стану всіх повідомленнь, поданих власноруч")]
        public bool ReceiveOnChangeOwnMessage { get; set; }

        //TODO изменить на ReceiveOnAutoDeleteOwnProjects
        [DisplayName("Отримувати сповіщення про автоматичне видалення всіх проектів заяв та повідомленнь")]
        public bool PersonalCabinetStatus { get; set; }

        [DisplayName("Отримувати сповіщення про зміну даних організації")]
        public bool ReceiveOnChangeOrgInfo { get; set; } 

        [DisplayName("Отримувати сповіщення про необхідність сплати ДЛС для заяв, поданих співробітниками компанії")]
        public bool ReceiveOnOverduePayment { get; set; }
    }

    [RightsCheckList(nameof(EmployeeExt))]
    public class UserAreaEmployeeDetailDTO: BaseDTO
    {
        public Guid? OrgId { get; set; }

        public Guid? PersonId { get; set; }

        public string Position { get; set; }

        public string UserEmail { get; set; }

        public bool ReceiveOnChangeAllApplication { get; set; }

        public bool ReceiveOnChangeAllMessage { get; set; }

        public bool ReceiveOnChangeOwnApplication { get; set; }

        public bool ReceiveOnChangeOwnMessage { get; set; }

        public bool PersonalCabinetStatus { get; set; }

        public bool ReceiveOnChangeOrgInfo { get; set; }

        public bool ReceiveOnOverduePayment { get; set; }
    }

    [RightsCheckList(nameof(EmployeeExt))]
    public class UserAreaOrgDetailDTO: BaseDTO
    {
        public string Name { get; set; }

        public string Edrpou { get; set; }

        public string INN { get; set; }

        public string Email { get; set; }
    }

    [RightsCheckList(nameof(EmployeeExt))]
    public class UserAreaPersonDetailDTO: BaseDTO
    {
        public string Name { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string IPN { get; set; }        
    }
}
