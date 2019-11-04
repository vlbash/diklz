using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.TrlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.TRL;
using App.Data.Models.TRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.TRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Trl")]
    public class TrlLicenseController: CommonController<TrlLicenseDetailDTO, TrlLicense>
    {
        private ITrlLicenseService _trlLicenseService { get; }

        public TrlLicenseController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, ITrlLicenseService trlLicenseService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _trlLicenseService = trlLicenseService;
        }


        [BreadCrumb(Title = "Ліцензія з торгівлі", Order = 1)]
        public override async Task<IActionResult> Index()
        {
            var licenseId = _trlLicenseService.GetLicenseGuid();
            var model = (await _trlLicenseService._commonDataService.GetDtoAsync<TrlLicenseDetailDTO>(dto => dto.Id == licenseId.Value)).FirstOrDefault();
            if (model == null)
                return await Task.Run(() => NotFound());
            model.OrgName = !string.IsNullOrEmpty(model.EDRPOU) ? model.OrgDirector : model.Name;

            return await (licenseId == null
                ? Task.FromResult<IActionResult>(View())
                : Task.FromResult<IActionResult>(View(nameof(Details), model)));
        }

        [HttpGet]
        public IActionResult CheckLicense()
        {
            var licenseId = _trlLicenseService.GetLicenseGuid();
            return StatusCode(licenseId == null ? 500 : 200);
        }

        [NonAction]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id);
        }

    }
}
