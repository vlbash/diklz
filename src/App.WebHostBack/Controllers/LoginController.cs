using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Exceptions;
using App.Core.Business.Services;
using App.Data.Contexts;
using App.Data.DTO.LOG;
using App.Host.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace App.Host.Controllers
{
    public class LoginController: Controller
    {
        private readonly IUserInfoService _userService;
        private readonly ApplicationDbContext _context;

        public LoginController(IUserInfoService userService,
            ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public IActionResult Index()
        {
            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            //var user =  await _userService.GetCurrentUserInfoAsync();
            //var tasks = new Task[]
            //{
            //    HttpContext.SignOutAsync("Cookies"),
            //    HttpContext.SignOutAsync("oidc")
            //};
            //await Task.WhenAll(tasks);
            //try
            //{
            //    logoutTime.Wait();
            //}
            //catch(Exception ex)
            //{

            //}
            //var userAfterLogout = await _userService.GetCurrentUserInfoAsync();
            //if (userAfterLogout==null)
            SetLogout();
        }

        [AllowAnonymous]
        public async Task Login()
        {
            await HttpContext.SignInAsync("Cookies", new System.Security.Claims.ClaimsPrincipal());
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginPage()
        {
            var user = _userService.GetCurrentUserInfo();
            Log.Warning(user.Id);
            ViewBag.userId = user.Id;
            var model = new UserIdentLog
            {
                UserGuid = user.Id,
                LoginData = user.LoginData
            };
            var loginData = (from person in _context.Person.Where(u => u.UserId == user.Id)
                             join employee in _context.Employee on person.Id equals employee.PersonId
                             join empprofile in _context.UserProfiles on employee.Id equals empprofile.UserId
                             join profile in _context.Profiles on empprofile.ProfileId equals profile.Id
                             select new
                             {
                                 PersonId = person.Id,
                                 PersonFIO = person.FIO,
                                 EmployeeId = employee.Id,
                                 ProfileId = profile.Id,
                                 ProfileName = profile.Caption,
                                 Login = person.FIO
                             }).ToList();

            if (loginData.Count > 1)
            {
                model.PersonId = loginData.First().PersonId;
                model.EmployeeId = loginData.First().EmployeeId;
                model.ProfileId = loginData.First().ProfileId;

                ViewBag.EmpList = loginData
                                    .GroupBy(t => t.EmployeeId)
                                    .Select(g => g.First())
                                    .Select(c => new SelectListItem
                                    {
                                        Value = c.EmployeeId.ToString(),
                                        Text = c.PersonFIO
                                    });

                ViewBag.ProfList = loginData
                                    .GroupBy(t => t.ProfileId)
                                    .Select(g => g.First())
                                    .Select(c => new SelectListItem
                                    {
                                        Value = c.ProfileId.ToString(),
                                        Text = c.ProfileName
                                    });
                return View(model);
            }
            else if (loginData.Count == 1)
            {
                model.PersonId = loginData.First().PersonId;
                model.EmployeeId = loginData.First().EmployeeId;
                model.ProfileId = loginData.First().ProfileId;
                model.LoginData["UserLogin"] = loginData.First().Login;
                model.LoginData["ProfileName"] = loginData.First().ProfileName;


                return SetLogin(model);
            }
            else
            {
                ModelState.AddModelError("EmployeeId", "За Вами не закріплено співробітника або профіль, зверніться до адміністратора");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult LoginPage(UserIdentLog model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emplProf = (from person in _context.Person.Where(u => u.Id == model.PersonId)
                            join employee in _context.Employee on person.Id equals employee.PersonId
                            join empprofile in _context.UserProfiles on employee.Id equals empprofile.UserId
                            select new { PersonId = person.Id, EmployeeId = employee.Id, empprofile.ProfileId })
                    .FirstOrDefault(x => x.EmployeeId == model.EmployeeId && x.ProfileId == model.ProfileId);
            if (emplProf == null)
            {
                ModelState.AddModelError("EmployeeId", "За обраним співробітником даний профіль не закріплений, зверніться до адміністратора");
                return View(model);
            }

            return SetLogin(model);
        }

        private IActionResult SetLogin(UserIdentLog model)
        {
            var userInfo = _userService.GetCurrentUserInfo();
            userInfo.ProfileId = model.ProfileId;
            userInfo.UserId = model.EmployeeId;
            userInfo.PersonId = model.PersonId;
            userInfo.LoginData = model.LoginData;


            _userService.UpdateUserInfo(userInfo);

            return RedirectToAction("Index", "Home");
        }

        private void SetLogout()
        {
            _userService.DeleteCurrentUserInfo();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature?.Error;

            switch (exception)
            {
                case BusinessRulesException businessRulesException:
                    return View("BusinessError", businessRulesException);
                    break;
                default:
                    break;
            }

            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
