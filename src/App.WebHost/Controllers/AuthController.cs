using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Web;
using App.Business.Extensions;
using App.Business.Services.Token;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Core.Data.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace App.Host.Controllers
{
    [Authorize]
    public class AuthController: Controller
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpRequest _request => _httpContextAccessor.HttpContext.Request;
        private readonly IUserInfoService _userInfoService;

        public AuthController(IConfiguration config, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IUserInfoService userInfoService)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _userInfoService = userInfoService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var redirectUri = _config["IdGovUaProvider:RedirectUri"];
            var clientId = _config["IdGovUaProvider:ClientId"];
            var authUri = _config["IdGovUaProvider:Url"];

            return Redirect($"{authUri}/?response_type=code&client_id={clientId}&redirect_uri={redirectUri}/auth/authorize");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Authorize()
        {
            var code = HttpUtility.ParseQueryString(_request.QueryString.Value).Get("code");
            var tokenModel = await _tokenService.GetIdGovUaToken(code);
            if (tokenModel == null)
            {
                return RedirectToAction("Denied",
                    new
                    {
                        text =
                            "Сервіс \"Інтегрована система електронної ідентифікації\" https://id.gov.ua/ не відповідає. Доступ на веб - портал СГД Держлікслужби не може бути наданий. Будь - ласка спробуйте пізніше."
                    });
            }

            var userInfo = await _tokenService.GetIdGovUaUserInfo(tokenModel);
            if (userInfo == null)
            {
                return RedirectToAction("Denied",
                    new
                    {
                        text =
                            "Сервіс \"Інтегрована система електронної ідентифікації\" https://id.gov.ua/ не підтвердив Вашу особу. Доступ на веб - портал СГД Держлікслужби не надано."
                    });
            }

            if (!string.IsNullOrEmpty(userInfo.edrpoucode))
            {
                if (userInfo.edrpoucode.All(char.IsDigit))
                {
                    if (userInfo.edrpoucode.Length != 8)
                    {
                        userInfo.drfocode = userInfo.edrpoucode;
                        userInfo.edrpoucode = string.Empty;
                    }
                }
                else
                {
                    userInfo.drfocode = userInfo.edrpoucode;
                    userInfo.edrpoucode = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(userInfo.drfocode))
            {
                return RedirectToAction("Denied", new
                {
                    text = "Згідно інформації, що надійшла від \"Інтегрованой системи електронної ідентифікації\" https://id.gov.ua/ у Вашій ЕЦП не заповнене поле \"Код РНОКПП (Індивідуальний податковий номер) користувача\"." +
                           "В зв'язку з цим, ми не можемо ідентифікувати Вашу особу."
                });
            }

            if (userInfo.edrpoucode == userInfo.drfocode)
                userInfo.edrpoucode = "";

            if (userInfo.auth_type != "dig_sign")
            {
                return RedirectToAction("Denied", new
                {
                    text = "Сервіс \"Інтегрована система електронної ідентифікації\" https://id.gov.ua/ надав інформацію що Ви ідентифікувалися за допомогою MobileID / BankID. " +
                           "Звертаємо Вашу увагу, що доступ до порталу надається тільки для Користувачів з ЕЦП, зареєстрованих на організацію чи ФОП. " +
                           "Доступ на веб - портал СГД Держлікслужби не надано."
                });
            }
            (string organizationId, Guid employeeId, Guid profileId, Guid personId) res;
            string register;
            try
            {
                res = await _tokenService.CheckOrgEmployeeUnit(userInfo);
                register = "1";

            }
            catch (Exception)
            {
                register = "0";
                res = ("", Guid.Empty, Guid.Empty, Guid.Empty);
            }

            var claims = new List<Claim>
            {
                new Claim("fullName", userInfo.subjectcn, ClaimValueTypes.String),
                new Claim("lastname", userInfo.lastname, ClaimValueTypes.String),
                new Claim("drfocode", userInfo.drfocode, ClaimValueTypes.String),
                new Claim("register", register, ClaimValueTypes.String)
            };

            var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            _userInfoService.UpdateUserInfo(new UserInfo()
            {
                Id = userInfo.drfocode + userInfo.lastname,
                UserCultureInfo = new UserCultureInfo(),
                LoginData = new Dictionary<string, string>()
                {
                    {"FullName", userInfo.subjectcn},
                    {"OrganizationName", userInfo.o},
                    {"Position", userInfo.title},
                    {"Name", userInfo.givenname},
                    {"MiddleName", userInfo.middlename},
                    {"LastName", userInfo.lastname},
                    {"Email", userInfo.email},
                    {"Address", userInfo.address},
                    {"Phone", userInfo.phone},
                    {"EDRPOU", userInfo.edrpoucode},
                    {"INN", userInfo.drfocode},
                    {"SerialNumber", userInfo.serial},
                    {"OrganizationId", res.organizationId}
                },
                UserId = res.employeeId,
                ProfileId = res.profileId,
                PersonId = res.personId
            });


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(8),
                    IsPersistent = false,
                    AllowRefresh = false
                });

            if (register == "0")
            {
                return RedirectToAction("SignIn");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            var userInfo = _userInfoService.GetCurrentUserInfo();

            // Добавил Максим. Блокировка загрузки страницы регистрации, если пользватель залогирован
            if ((userInfo != null) && (!string.IsNullOrEmpty(userInfo.OrganizationId())))
            {
                return RedirectToAction("Index", "Home");
            } // конец

            var employee = new SignInEditModel()
            {
                Position = userInfo?.Position(),
                LastName = userInfo?.LastName(),
                MiddleName = userInfo?.MiddleName(),
                UserPhone = userInfo?.Phone(),
                Address = userInfo?.Address(),
                UserEmail = userInfo?.Email(),
                EDRPOU = userInfo?.EDRPOU(),
                SertCode = userInfo?.SerialNumber(),
                UserName = userInfo?.Name(),
                INN = userInfo?.INN()
            };
            employee.Name = string.IsNullOrEmpty(userInfo?.EDRPOU()) ? userInfo?.FullName() : userInfo?.Name();

            return View(employee);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(SignInEditModel model)
        {
            if ((string.IsNullOrEmpty(model.EDRPOU)) && (string.IsNullOrEmpty(model.OrgEmail)))
                model.OrgEmail = model.UserEmail;
            else
                if (string.IsNullOrEmpty(model.OrgEmail))
                    ModelState.AddModelError("OrgEmail", "Поле необхідне для заповнення");
                
            
            if (ModelState.IsValid)
            {
                try
                {
                    var savedData = _tokenService.SaveSignIn(model, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
                    var claims = new List<Claim>()
                    {
                        new Claim("fullName", $"{model.LastName} {model.UserName} {model.MiddleName}", ClaimValueTypes.String),
                        new Claim("lastname", model.LastName, ClaimValueTypes.String),
                        new Claim("drfocode", model.INN, ClaimValueTypes.String),
                        new Claim("register", "1", ClaimValueTypes.String)
                    };
                    HttpContext.SignOutAsync();

                    _userInfoService.UpdateUserInfo(new UserInfo()
                    {
                        Id = model.INN + model.LastName,
                        UserCultureInfo = new UserCultureInfo(),
                        LoginData = new Dictionary<string, string>()
                        {
                            {"FullName", $"{model.LastName} {model.UserName} {model.MiddleName}"},
                            {"OrganizationName", model.Name},
                            {"Position", model.Position},
                            {"Name", model.UserName},
                            {"MiddleName", model.MiddleName},
                            {"LastName", model.LastName},
                            {"Email", model.UserEmail},
                            {"Address", model.Address},
                            {"Phone", model.UserPhone},
                            {"EDRPOU", model.EDRPOU},
                            {"INN", model.INN},
                            {"SerialNumber", model.SertCode},
                            {"OrganizationId", savedData.organizationId.ToString()}
                        },
                        UserId = savedData.employeeId,
                        ProfileId = savedData.profileId,
                        PersonId = savedData.personId
                    });

                    var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddHours(8),
                            IsPersistent = false,
                            AllowRefresh = false
                        });
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    HttpContext.SignOutAsync();
                    return RedirectToAction("Denied", "Auth", new
                    {
                        text = "Нажаль Ви не були зареєстровані на веб-порталі СГД Держлікслужби через помилку зв'язку з сервером. Будь-ласка спробуйте ще раз."
                    });
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Denied(string text)
        {
            ViewBag.DeniedMessage = text;
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            _userInfoService.DeleteCurrentUserInfo();
            return RedirectToAction("Info", "Home");
        }
    }
}
