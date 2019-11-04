using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Business.Services;

namespace App.Business.Services.NotificationServices
{
    public interface INotificationService
    {
        // Створення сповіщення про прийняття рішення по заяві. 
        // В залежності від параметра isPositiveDecision та типу заяви створюються повідомлення з (або без) нагадуванням про необхідність сплати за послуги
        Task<bool> PrlCreateNotificationAppResolve(Guid id, string appSort, string appDate, Guid orgInfoId, bool isPositiveDecision);

        // Створення сповіщення про відправку заяви до ДЛС
        Task<bool> PrlCreateNotificationAppSend(Guid id, string appSort, string appDate, Guid orgInfoId);

        // Створення сповіщення про реєстрацію заяви у ДЛС, вказуються реєстраційні дані
        Task<bool> PrlCreateNotificationAppRegister(Guid id, string appSort, string appDate, Guid orgInfoId, string regNum, string regDate);

        // Створення даного сповіщення провозиться через PrlCreateNotificationAppResolve
        Task<bool> PrlCreateNotificationAppResolvePay(Guid id, string appSort, string appDate, Guid orgInfoId);

        // Створення сповіщення при затримці сплати за послуги ДЛС після прийняття позитивного рішення по заяві
        Task<bool> PrlCreateNotificationAppResolvePayRepeatedly(Guid id, string appSort, string appDate, Guid orgInfoId);

        // Створення сповіщення при відправці повідомлення до ДЛС
        Task<bool> PrlCreateNotificationMsgSend(Guid id, string msgSort, string msgDate, Guid orgInfoId);

        // Створення сповіщення після прийняття рішення ДЛС по повідомленню
        Task<bool> PrlCreateNotificationMsgResolve(Guid id, string msgSort, string msgDate, Guid orgInfoId, bool isAsseptDecision);
    }
}
