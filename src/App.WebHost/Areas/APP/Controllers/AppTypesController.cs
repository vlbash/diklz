using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Core.Business.Services;
using App.Data.Models.ORG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Areas.APP.Controllers
{
    [Area("App")]
    [Authorize(Policy = "Registered")]
    public class AppTypesController: Controller
    {
        private ICommonDataService _dataService;
        private IUserInfoService _userInfoService;
        private LicenseService _licenseService;

        public AppTypesController(ICommonDataService dataService, IUserInfoService userInfoService, LicenseService licenseService)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _licenseService = licenseService;
        }
        public async Task<IActionResult> Index()
        {
            var orgId = (await _userInfoService.GetCurrentUserInfoAsync())?.OrganizationId();
            if (orgId == null)
                return await Task.Run(() => NotFound());
            var orgInfos = _dataService
                .GetEntity<OrganizationInfo>(x => x.OrganizationId == new Guid(orgId) && x.IsActualInfo);
                
            var dick = orgInfos.ToDictionary(x => x.Type, x => x.IsPendingLicenseUpdate);
            var licenses = await _licenseService.GetActiveLicenses();
            licenses.ForEach(p => dick.Add("LIC_"+p.type, p.isActive));
            ViewBag.isFop = false;//string.IsNullOrEmpty((await _userInfoService.GetCurrentUserInfoAsync()).EDRPOU());
            return View(dick);
        }
    }
}
