using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Business.Services;
using App.Data.DTO.APP;
using App.Data.DTO.P902;
using App.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Business.Services.P902Services;

namespace App.Host.Areas.P902.Controllers
{
    using IApplication = IBaseService<AppConclusionListDTO, LimsDoc>;
 

    [Authorize(Policy = "Registered")]
    [Area("P902")]
    public class AppConclusionController: BaseController<AppConclusionListDTO, AppConclusionListDTO, LimsDoc>
    {
        private readonly IApplication applicationService;
        private readonly AppConclusionServise _service;

        public AppConclusionController(IApplication applicationService, AppConclusionServise service)
        {
            this.applicationService = applicationService;
            _service = service;
        }

        [BreadCrumb(Title = "Подання заяви", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<AppConclusionListDTO> options)
        {
            options.pg_SortExpression = !string.IsNullOrEmpty(options.pg_SortExpression) ? options.pg_SortExpression : "-ModifiedOn";
            return await base.List(paramList, options);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            AppConclusionDetailDTO model = null;
            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(Guid? id)
        //{
         
        //}
    }
}
