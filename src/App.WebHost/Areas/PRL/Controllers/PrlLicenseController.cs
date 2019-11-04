using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.PRL;
using App.Data.Models.PRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.PRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Prl")]
    public class PrlLicenseController: CommonController<PrlLicenseDetailDTO, PrlLicense>
    {
        private readonly IPrlLicenseService _service;
        private readonly ICommonDataService _dataService;

        public PrlLicenseController(IPrlLicenseService service, IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService, ICommonDataService dataService)
            : base(service._commonDataService, configuration, searchFilterSettingsService)
        {
            _service = service;
            _dataService = dataService;
        }

        [BreadCrumb(Title = "Ліцензія з виробництва", Order = 1)]
        public override async Task<IActionResult> Index()
        {
            var licenseId = _service.GetLicenseGuid();
            var model = (await _service._commonDataService.GetDtoAsync<PrlLicenseDetailDTO>(dto => dto.Id == licenseId.Value)).FirstOrDefault();
            if (model == null)
                return await Task.Run(() => NotFound());
            model.OrgName = !string.IsNullOrEmpty(model.EDRPOU) ? model.OrgDirector : model.Name;

            return await (licenseId == null
                ? Task.FromResult<IActionResult>(View())
                : Task.FromResult<IActionResult>(View(nameof(Details),model)));
        }

        [HttpGet]
        public IActionResult CheckLicense()
        {
            var licenseId = _service.GetLicenseGuid();
            return StatusCode(licenseId == null ? 500 : 200);
        }

        [NonAction]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await base.Details(id);
        }
    }
}
