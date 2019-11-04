using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.CRV;
using App.Data.Models.CRV;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.CRV.Controllers
{
    [Area("CRV")]
    [Authorize]
    public class LimsRPController : CommonController<LimsListRPDTO, LimsDetailsRPDTO, LimsRP>
    {
        public LimsRPController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService) : base(dataService, configuration, searchFilterSettingsService)
        {
        }

        [BreadCrumb(Title = "Реєстр РП і АНД (МКЯ)", Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<LimsListRPDTO> actionListOption)
        {
            actionListOption.pg_SortExpression = !string.IsNullOrEmpty(actionListOption.pg_SortExpression)
                ? actionListOption.pg_SortExpression
                : "-DocId";
            return await base.List(paramList, actionListOption);
        }

        [BreadCrumb(Title = "Деталі РП ЛЗ", Order = 2)]
        public override Task<IActionResult> Details(Guid id)
        {
            return base.Details(id);
        }

        public override Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }
    }
}
