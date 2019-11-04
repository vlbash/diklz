using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.ImlServices;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.IML;
using App.Data.DTO.PRL;
using App.Data.Models.IML;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.IML.Controllers
{
    [Authorize]
    [Area("Iml")]
    public class ImlLicenseController : CommonController<ImlLicenseListDTO, ImlLicenseDetailDTO, ImlLicense>
    {
        private readonly IImlLicenseService _imlLicenseService;
        public ImlLicenseController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, IImlLicenseService imlLicenseService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _imlLicenseService = imlLicenseService;
        }

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.EndLicCheck })]
        [BreadCrumb(Title = "Реєстр ліцензій з імпорту ЛЗ", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<ImlLicenseListDTO> options)
        {
            return await PartialList(paramList, options);
        }

        [BreadCrumb(Title = "Ліцензія з імпорту", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id, _imlLicenseService.LicenseDetail);
        }
    }
}
