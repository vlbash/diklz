using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.LimsService;
using App.Business.Services.TrlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.TRL;
using App.Data.Models.TRL;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.TRL.Controllers
{
    [Authorize]
    [Area("Trl")]
    public class TrlLicenseController : CommonController<TrlLicenseListDTO, TrlLicenseDetailDTO, TrlLicense>
    {
        private readonly ITrlLicenseService _trlLicenseService;
        public TrlLicenseController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, ITrlLicenseService trlLicenseService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _trlLicenseService = trlLicenseService;
        }

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.EndLicCheck })]
        [BreadCrumb(Title = "Реєстр ліцензій з виробництва", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<TrlLicenseListDTO> options)
        {
            return await PartialList(paramList, options);
        }

        [BreadCrumb(Title = "Ліцензія з виробництва", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id, _trlLicenseService.LicenseDetail);
        }
    }
}
