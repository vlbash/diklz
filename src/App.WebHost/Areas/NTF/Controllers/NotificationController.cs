using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.NTF;
using App.Data.Models.NTF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using App.Business.Services.NotificationServices;
using App.Business.Services.RptServices;
using App.Business.Services.Common;
using System;
using App.Business.Attributes;

namespace App.Host.Areas.NTF.Controllers
{
    // using INotification = IBaseService<NotificationListDTO, Notification>;

    [Area("Ntf")]
    [Authorize(Policy = "Registered")]

    public class NotificationController: CommonController<NotificationListDTO, NotificationDetailsDTO, Notification>
    {
        private readonly ICommonDataService _dataService;
        //private readonly INotification _notification;
        private readonly UserInfo _userInfo;
        private readonly IFeedBackService _feedBackService;
        private readonly INotificationService _notificationService;
        private readonly IMgsReportService _mgsReportService;

        public NotificationController(IUserInfoService userInfoService, IConfiguration configuration, ICommonDataService dataService, IMgsReportService mgsReportService,
                                        ISearchFilterSettingsService filterSettingsService, INotificationService notificationService, IFeedBackService feedBackService) //INotification notification, 
            : base(dataService, configuration, filterSettingsService)
        {
            // _notification = notification;
            _userInfo = userInfoService.GetCurrentUserInfo();
            _dataService = dataService;
            _mgsReportService = mgsReportService;
            _notificationService = notificationService;
            _feedBackService = feedBackService;
        }

        [BreadCrumb(Title = "Реєстр сповіщеннь", Order = 1)]
        public override Task<IActionResult> Index()
        {
            // Прийнято рішення по заяві
            // var boo1 = _notificationService.PrlCreateNotificationAppResolve(Guid.Parse("0c2037ca-ac7e-4717-9a5b-c25d062866f1"), "Заява на отримання", "11 жовтня 1981 року", Guid.Parse("3dc61071-5bf8-4a45-a260-842574d545b9"), true, false);

            // Заяву відправлено до ДЛС
            // var boo2 = _notificationService.PrlCreateNotificationAppSend(Guid.Parse("0c2037ca-ac7e-4717-9a5b-c25d062866f1"), "Заява на отримання", "12 жовтня 1981 року", Guid.Parse("432a47ce-f396-45d5-9af8-0423cb5d365d"), true);

            // Заяву зарєєстровано у ДЛС
            //var boo3 = _notificationService.PrlCreateNotificationAppRegister(Guid.Parse("5b713563-9deb-45dd-ae69-939cfbafd7a6"), "Заява на отримання", "12 жовтня 1981 року", Guid.Parse("3dc61071-5bf8-4a45-a260-842574d545b9"), true, "11/2-2019", "05 лютого 2019");

            // Прийнято рішення по заяві. Очікується сплата.
            // var boo4 = _notificationService.PrlCreateNotificationAppResolve(Guid.Parse("0c2037ca-ac7e-4717-9a5b-c25d062866f1"), "GetLicenseApplication", "11 жовтня 1981 року", Guid.Parse("3dc61071-5bf8-4a45-a260-842574d545b9"), true, true);

            // Повідомлення відправлено
            // var boo5 = _notificationService.PrlCreateNotificationMsgSend(Guid.Parse("0c2037ca-ac7e-4717-9a5b-c25d062866f1"), "AnotherEvent", "12 жовтня 1981 року", Guid.Parse("3dc61071-5bf8-4a45-a260-842574d545b9"), true);

            // Прийнято рішення по повідомленню
            // var boo6 = _notificationService.PrlCreateNotificationMsgResolve(Guid.Parse("021842b8-2e7c-4903-9b2f-de58cf7b1476"), "PharmacyHeadReplacement", "12 жовтня 1981 року", Guid.Parse("3dc61071-5bf8-4a45-a260-842574d545b9"), true, true);

            /// _mgsReportService.MsgToPDFSgdChiefNameChange(Guid.Parse("021842b8-2e7c-4903-9b2f-de58cf7b1476"));

            // var hh = _feedBackService.GetFeedback(Guid.Parse("513cdbef-0fd5-4e13-8f48-15193b96b3f0"));
            // _feedBackService.CloseFeedback(Guid.Parse("513cdbef-0fd5-4e13-8f48-15193b96b3f0"), null, 2);

            //var orgInfo = _dataService.GetDto<OrgInnEdrpouRptMinDetail>(x => x.Id == Guid.Parse("750ef1dc-cf70-4fc7-86d8-bb6d3ee24e3e")).FirstOrDefault();

            //if (orgInfo == null)
            //{
            //    throw new Exception("Не вдалося отримати інформацію про організацію при формуванні сповіщення");
            //}

            return base.Index();
        }

        [BreadCrumb(Title = "Детальна форма нотифікації", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            // TODO добавить в крошки тип оповещения, пример - App.WebHost\Areas\PRL\Controllers\PrlAppAltController.cs            
            return await base.Details(id);
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<NotificationListDTO> options)
        {
            options.pg_SortExpression = !string.IsNullOrEmpty(options.pg_SortExpression)
                ? options.pg_SortExpression
                : "-DateOfCreate";
            return await base.PartialList<NotificationListDTO>(paramList, options);
        }
    }
}
