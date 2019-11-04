using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.Common;
using App.Data.Models.APP;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.App.Controllers
{
    [Authorize]
    [Area("App")]
    public class ProtocolController: CommonController<ProtocolListDTO, ProtocolListDTO, AppProtocol>
    {
        public ProtocolController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService) : base(dataService, configuration, searchFilterSettingsService)
        {
        }

        [NonAction]
        public async override Task<IActionResult> Index() { return NotFound();}

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        [BreadCrumb(Order = 1)]
        public async Task<IActionResult> Index(string appType)
        {
            if (string.IsNullOrEmpty(appType))
                return  NotFound();
            ViewBag.appType = appType;

            HttpContext.ModifyCurrentBreadCrumb(p => p.Name = "Протоколи засідань " + (appType == "PRL" ? "(Виробництво)" : appType == "IML" ? "(Імпорт)" : "(Торгівля)"));

            return View();
        }

        [BreadCrumb(Title = "Деталі протоколу", Order = 2)]
        public override Task<IActionResult> Details(Guid id)
        {
            var referer = Request.Headers["Referer"].ToString();
            var type = referer.Substring(referer.Length - 3);
            ViewBag.appType = type;
            return base.Details(id);
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<ProtocolListDTO> options)
        {
            var referer = Request.Headers["Referer"].ToString();
            var type = referer.Substring(referer.Length - 3);
            options.pg_SortExpression = !string.IsNullOrEmpty(options.pg_SortExpression)
                ? options.pg_SortExpression
                : "-ProtocolDate";

            return await PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<ProtocolListDTO>(
                orderBy: options.pg_SortExpression ?? "-MessageDate",
                predicate: p => p.Type.Contains(type),
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value));
        }

        public override Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
    }
}
