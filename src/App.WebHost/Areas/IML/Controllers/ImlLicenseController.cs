using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.ImlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.IML;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.IML.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Iml")]
    public class ImlLicenseController : CommonController<ImlLicenseDetailDTO, ImlLicense>
    {
        private IImlLicenseService _imlLicenseService { get; }

        public ImlLicenseController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, IImlLicenseService imlLicenseService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _imlLicenseService = imlLicenseService;
        }


        [BreadCrumb(Title = "Ліцензія з імпорту", Order = 1)]
        public override async Task<IActionResult> Index()
        {
            var licenseId = _imlLicenseService.GetLicenseGuid();
            var model = (await _imlLicenseService._commonDataService.GetDtoAsync<ImlLicenseDetailDTO>(dto => dto.Id == licenseId.Value)).FirstOrDefault();
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
            var licenseId = _imlLicenseService.GetLicenseGuid();
            return StatusCode(licenseId == null ? 500 : 200);
        }

        [NonAction]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id);
        }

    }
}
