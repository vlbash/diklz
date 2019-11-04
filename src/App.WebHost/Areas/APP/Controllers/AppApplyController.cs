using System.Collections.Generic;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Business.Services;
using App.Data.DTO.APP;
using App.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Areas.APP.Controllers
{
    using IApplication = IBaseService<ApplicationListDTO, LimsDoc>;

    [Authorize(Policy = "Registered")]
    [Area("App")]
    public class AppApplyController: BaseController<ApplicationListDTO, ApplicationListDTO, LimsDoc>
    {
        private readonly IApplication applicationService;

        public AppApplyController(IApplication applicationService)
        {
            this.applicationService = applicationService;
        }

        [BreadCrumb(Title = "Подання заяви", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<ApplicationListDTO> options)
        {
            if (!paramList.TryGetValue("AppStateEnum", out var val) || val != "Project")
            {
                paramList.Add("AppStateEnum", "Project");
            }
            options.pg_SortExpression = !string.IsNullOrEmpty(options.pg_SortExpression) ? options.pg_SortExpression : "-ModifiedOn";
            return await base.List(paramList, options);
        }

    }
}
