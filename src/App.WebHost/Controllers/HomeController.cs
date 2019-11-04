using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.AppServices;
using App.Business.Services.PrlServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Controllers
{
    [Authorize(Policy = "Registered")]
    [BreadCrumb(Title = "Головна сторінка", UseDefaultRouteUrl = true, ClearStack = true, Order = 0)]
    public class HomeController: Controller
    {
        private readonly LicenseService _licenseService;

        private readonly IConfiguration _configuration;

        public HomeController(LicenseService licenseService, IConfiguration configuration)
        {
            _licenseService = licenseService;
            _configuration = configuration;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var userInfo = _userInfoService.GetCurrentUserInfo();
            //var orgName = userInfo?.LoginData.FirstOrDefault(x => x.Key == "OrganizationName").Value ?? "Ви не авторизовані";
            ViewBag.OrganizationName = "Test"; /*orgName;*/
            var model = await _licenseService.GetActiveLicenses();
            return View(model);
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [AllowAnonymous]
        public IActionResult Info()
        {
            ViewBag.PortalLink = _configuration.GetSection("OpenPortal").Value;
            return View();
        }
    }
}
