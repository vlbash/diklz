using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.AppServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.PublicHost.Controllers
{
    public class HomeController: Controller
    {
        public async Task<IActionResult> Index()
        {
            //var userInfo = _userInfoService.GetCurrentUserInfo();
            //var orgName = userInfo?.LoginData.FirstOrDefault(x => x.Key == "OrganizationName").Value ?? "Ви не авторизовані";
           
            return View();
        }
    }
}
