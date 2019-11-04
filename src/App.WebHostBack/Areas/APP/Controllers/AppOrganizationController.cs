using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Business.Services;
using App.Data.DTO.APP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.HostBack.Areas.APP.Controllers
{
    [Authorize]
    [Area("App")]
    public class AppOrganizationController : Controller
    {
        public AppOrganizationController(ICommonDataService dataService)
        {
            DataService = dataService;
        }

        private ICommonDataService DataService { get; }

        [BreadCrumb(Title = "Вибір ліцензіата", Order = 2)]
        [HttpGet]
        public IActionResult ChooseOrganization(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return View($"{nameof(ChooseOrganization)}Error");
            }

            ViewBag.returnUrl = returnUrl;
            return View(new AppOrganizationExtMediumDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ChooseOrganization(string returnUrl, AppOrganizationExtMediumDTO model)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return View($"{nameof(ChooseOrganization)}Error");
            }

            if (model.Id == Guid.Empty)
            {
                ModelState.AddModelError("Id", "Необхідно обрати організацію");
                return View(model);
            }

            return await Task.Run(() =>
            {
                return Redirect(returnUrl.Contains('?')
                    ? $"{returnUrl}&orgId={model.Id}"
                    : $"{returnUrl}?orgId={model.Id}");
            });
        }

        [HttpPost]
        public JsonResult AutoCompleteOrganization(string term)
        {
            var result1 = DataService.GetDto<AppOrganizationExtShortDTO>(
                    predicate: dto =>
                        string.IsNullOrEmpty(term)
                        || dto.EDRPOU != null && dto.EDRPOU.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || dto.INN != null && dto.INN.Contains(term, StringComparison.InvariantCultureIgnoreCase),
                    orderBy: "Name",
                    take: 10);

            var result = result1
            .Select(dto => new
            {
                label = $"{dto.edrpouOrInn} {dto.Name} ({dto.License})",
                value = dto.edrpouOrInn,
                option = $"{dto.Id} {dto.LicType}"
            })
            .ToList();

            return Json(result);
        }

        public JsonResult OnChangeOrganization(IDictionary<string, string> paramList)
        {
            AppOrganizationExtFullDTO organization;

            if (paramList.TryGetValue("Id", out var idStr) && Guid.TryParse(idStr.Split(' ')[0], out var id))
            {
                var licType = idStr.Split(' ');
                organization = DataService.GetDto<AppOrganizationExtFullDTO>(dto => dto.Id == id && dto.LicType == licType[1]).SingleOrDefault();
            }
            else
            {
                return new JsonResult(NotFound());
            }

            var result = new Dictionary<string, string>
            {
                {"Id", organization?.Id.ToString()},
                {"Name", organization?.Name},
                {"OrgDirector", organization?.OrgDirector },
                {"Address", organization?.Address },
                {"EMail", organization?.EMail }
            };

            return new JsonResult(result);
        }
    }
}
