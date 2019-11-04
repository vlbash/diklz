using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.ORG;
using App.Core.Security.Entities;

namespace App.Data.Models.ORG
{
    public class EmployeeExt : Employee
    {
        [DisplayName("Спеціалізація")]
        public Guid? OrgUnitSpecializationId { get; set; }

        [DisplayName("Область")]
        public Guid? AtuRegionId { get; set; }
        
        [DisplayName("E-mail користувача")]
        public string UserEmail { get; set; }

        public string Position { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану всіх заяв")]
        public bool ReceiveOnChangeAllApplication { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану всіх повідомлень")]
        public bool ReceiveOnChangeAllMessage { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану заяв, розміщених особисто")]
        public bool ReceiveOnChangeOwnApplication { get; set; }

        [DisplayName("Отримувати повідомлення на вказану електронну пошту при зміні стану повідомлень, розміщених особисто")]
        public bool ReceiveOnChangeOwnMessage { get; set; }

        [DisplayName("Отримувати попередження на вказану електронну пошту перед автоматичним видаленням проектів заяв та повідомленнь, розміщенних особисто")]
        public bool PersonalCabinetStatus { get; set; }

        [DisplayName("Отримувати інформацію по зміну даних організації")]
        public bool ReceiveOnChangeOrgInfo { get; set; }

        [DisplayName("Отримувати на вказану електронну пошту інформацію щодо необхідності сплати ДЛС за послуги щодо ліцензування")]
        public bool ReceiveOnOverduePayment { get; set; }

        public int? OldLimsId { get; set; }

        [ForeignKey("UserId")]
        public List<UserDefaultValue> DefaultValues { get; set; }

        [ForeignKey("UserId")]
        public List<UserProfile> Profiles { get; set; }
    }
}
