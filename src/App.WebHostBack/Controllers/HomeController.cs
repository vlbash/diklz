using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Business.Services;
using App.Data.Contexts;
using App.Data.DTO.Common.Widget;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.HostBack.Controllers
{
    [Authorize]
    [BreadCrumb(Title = "Головна сторінка", UseDefaultRouteUrl = true, ClearStack = true, Order = 0)]
    public class HomeController: Controller
    {
        private readonly ICommonDataService _dataService;

        public HomeController(ApplicationDbContext context, ICommonDataService dataService)
        {
            _dataService = dataService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model = (await _dataService.GetDtoAsync<WidgetBackDTO>()).FirstOrDefault();
            return View(model);
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
