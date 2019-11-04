using Microsoft.AspNetCore.Mvc;
using System.Linq;
using App.Core.Business.Services;
using App.Data.Contexts;
using App.Data.DTO.LOG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Host.Areas.SEC.Controllers
{
    [Area("Sec")]
    public class LoginController : Controller
    {
        private readonly IUserInfoService _userService;
        private readonly ApplicationDbContext _context;
        //private readonly IBaseService<ProfileEmployeeListDTO, EmployeeProfile> _profileEmployeeService;

        public LoginController(IUserInfoService userService,
            //IBaseService<ProfileEmployeeListDTO, EmployeeProfile> profileEmployeeService,
            ApplicationDbContext context)
        {
            _userService = userService;
            //_profileEmployeeService = profileEmployeeService;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginPage()
        {
            var user = _userService.GetCurrentUserInfo();
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
                                 ProfileName = profile.Caption
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

                _userService.UpdateUserInfo(userInfo); // теперь начитка прав происходит сама
          
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
