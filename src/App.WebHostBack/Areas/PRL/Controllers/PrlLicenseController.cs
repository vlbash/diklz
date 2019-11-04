using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.LimsService;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.PRL;
using App.Data.Models.PRL;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.PRL.Controllers
{
    [Authorize]
    [Area("Prl")]
    public class PrlLicenseController: CommonController<PrlLicenseListDTO, PrlLicenseDetailDTO, PrlLicense>
    {
        private readonly IPrlLicenseService _service;

        public PrlLicenseController(IPrlLicenseService service, IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService)
            : base(service._commonDataService, configuration, searchFilterSettingsService)
        {
            _service = service;
        }

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.EndLicCheck })]
        [BreadCrumb(Title = "Реєстр ліцензій з виробництва ЛЗ (промислового)", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<PrlLicenseListDTO> options)
        {
            return await PartialList(paramList, options);
        }

        [BreadCrumb(Title = "Ліцензія з виробництва", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id,_service.LicenseDetail);
        }
    }
}
