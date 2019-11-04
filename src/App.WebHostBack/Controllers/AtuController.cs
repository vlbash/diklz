using System;
using System.Linq;
using App.Business.Extensions;
using App.Business.Services.AtuService;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Data.DTO.ATU;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.HostBack.Controllers
{
    public class AtuController: Controller
    {
        private ICommonDataService _commonDataService { get; }
        private IAtuAddressService _atuAddressService { get; }

        public AtuController(ICommonDataService commonDataService, IAtuAddressService atuAddressService)
        {
            _commonDataService = commonDataService;
            _atuAddressService = atuAddressService;
        }

        // public IActionResult AtuAddress()
        //{
        //    return ViewComponent("AtuAddress");
        //}

        public IActionResult AtuModalCity()
        {
            ViewBag.Regions = new SelectList(_commonDataService.GetEntity<Region>(p => p.ParentId == null),
                nameof(Region.Id),
                nameof(Region.Name));
            return PartialView("Atu/_ModalAddCity");
        }

        public IActionResult AtuModalStreet(Guid cityId)
        {
            var city = _commonDataService.GetDto<AtuCityDTO>(p => p.Id == cityId, extraParameters: new object[] { $"AND city.id = '{cityId}'" }).SingleOrDefault();
            var street = new AtuStreetDTO
            {
                CityId = cityId,
                CityName = city?.CityFullName
            };
            return PartialView("Atu/_ModalAddStreet", street);
        }

        [HttpPost]
        public JsonResult AutoCompleteDistrict(string term, string regionId)
        {
            var result = _commonDataService.GetEntity<Region>(
                p => !string.IsNullOrEmpty(term)
                     && p.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                     && (p.ParentId.ToString() == regionId),
                take: 10)
                .Select(p => new
                {
                    label = p.Name,
                    value = p.Name,
                    option = p.Id
                }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult AutoCompleteDistrictFull(string term)
        {
            var result = _commonDataService
                .GetDto<AtuCityDTO>(
                    predicate: p => !string.IsNullOrEmpty(term),
                    orderBy: o => o.OrderBy(x => x.Name),
                    extraParameters: new object[] { $"AND concat(lower(city.name),'',city.code) LIKE lower('%{term}%') ORDER BY \"length\"(city.name) asc, district_name LIMIT 10" })
                .Select(p => new
                {
                    value = $"({p.Code}) {p.CityFullName}",
                    option = p.Id
                }).ToList();
            return Json(result);
        }

        [HttpPost]
        public JsonResult AutoCompleteStreet(string term, string cityId)
        {
            var result = _commonDataService.GetDto<AtuStreetDTO>(
                    p => !string.IsNullOrEmpty(term)
                         && p.Name.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                         && p.CityId.ToString() == cityId,
                    take: 10)
                .Select(p => new { value = p.FullName, option = p.Id }).ToList();
            return Json(result);
        }

        [HttpPost]
        public IActionResult AtuSaveCity(AtuCityDTO model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Atu/_ModalAddCity", model);
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult AtuSaveStreet(AtuStreetDTO model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Atu/_ModalAddStreet", model);
            }
            // Изменить структуру бд. Временно используется caption для streetType
            var exStreet = _commonDataService
                .GetEntity<Street>(p => p.CityId == model.CityId && p.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase) && p.Caption == model.TypeEnum)
                .SingleOrDefault();
            if (exStreet != null)
            {
                return Json(new { success = false, errorMessage = "Вулиця вже існує у системі" });
            }

            var newStreet = new Street { CityId = model.CityId, Name = model.Name.UppercaseWords(), Caption = model.TypeEnum };
            _atuAddressService.InsertStreet(newStreet);

            var addressType = "";
            switch (model.TypeEnum)
            {
                case "Street":
                    addressType = "вул.";
                    break;
                case "Lane":
                    addressType = "пров.";
                    break;
                case "Boulevard":
                    addressType = "б-р ";
                    break;
                case "Avenue":
                    addressType = "просп.";
                    break;
            }

            return Json(new { success = true, newStreetId = newStreet.Id, newStreetName = $"{addressType}{newStreet.Name}" });
        }
    }
}
