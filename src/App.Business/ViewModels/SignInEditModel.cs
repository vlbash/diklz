using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Business.ViewModels
{
    public class SignInEditModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування організації користувача")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування посади користувача")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("По-батькові")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Телефон користувача")]
        [Phone(ErrorMessage = "Не правильний формат номеру телефону")]
        public string UserPhone { get; set; }

        //[Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Адреса")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("E-mail Компанії")]
        public string OrgEmail { get; set; }

        [DisplayName("ЕДРПОУ організації")]
        public string EDRPOU { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Серійний номер сертифіката")]
        public string SertCode { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Ім'я")]
        public string UserName { get; set; }

        [DisplayName("РНОКПП (Індивідуальний податковий номер) користувача")]
        public string INN { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("E-mail користувача")]
        public string UserEmail { get; set; }

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
}
