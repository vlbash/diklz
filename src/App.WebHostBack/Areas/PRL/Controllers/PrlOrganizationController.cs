﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.AtuService;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Core.Mvc.Controllers;
using App.Data.DTO.ORG;
using App.Data.DTO.PRL;
using App.Data.Models.ORG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.PRL.Controllers
{
    [Area("PRL")]
    [Authorize]
    public class PrlOrganizationController: CommonController<OrganizationExtListDTO, PrlOrganizationExtFullDTO, OrganizationExt>
    {
        private readonly IPrlOrganizationService _service;
        private readonly IAtuAddressService _atuAddressService;

        public PrlOrganizationController(IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, IPrlOrganizationService service, IAtuAddressService atuAddressService)
            : base(service.DataService, configuration, searchFilterSettingsService)
        {
            _service = service;
            _atuAddressService = atuAddressService;
        }

        [BreadCrumb(Title = "Вибір ліцензіата", Order = 2)]
        [HttpGet]
        public IActionResult ChooseOrganization(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return View($"{nameof(ChooseOrganization)}Error");
            }

            ViewBag.returnUrl = returnUrl;
            return View(new PrlOrganizationExtMediumDTO());
        }

        [HttpPost]
        public async Task<IActionResult> ChooseOrganization(string returnUrl, PrlOrganizationExtMediumDTO model)
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
            var result1 = DataService.GetDto<PrlOrganizationExtShortDTO>(
                    predicate: dto =>
                        string.IsNullOrEmpty(term)
                        || dto.EDRPOU != null && dto.EDRPOU.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || dto.INN != null && dto.INN.Contains(term, StringComparison.InvariantCultureIgnoreCase),
                    orderBy: "Name",
                    take: 10);

                var result = result1
                .Select(dto => new
                {
                    label = $"{dto.edrpouOrInn} {dto.Name}",
                    value = dto.edrpouOrInn,
                    option = dto.Id
                })
                .ToList();

            return Json(result);
        }

        public JsonResult OnChangeOrganization(IDictionary<string, string> paramList)
        {
            PrlOrganizationExtFullDTO organization;

            if (paramList.TryGetValue("Id", out var idStr) && Guid.TryParse(idStr, out var id))
            {
                organization = DataService.GetDto<PrlOrganizationExtFullDTO>(dto => dto.Id == id).SingleOrDefault();
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

        public async Task<IActionResult> EditSgd(Guid? id, string returnUrl)
        {
            if ((id != Guid.Empty && id != null)
                || string.IsNullOrEmpty(returnUrl))
            {
                return View($"{nameof(ChooseOrganization)}Error");
            }

            ViewBag.returnUrl = returnUrl;
            return View(new PrlOrganizationExtFullDTO());
        }

        [HttpPost]
        public async Task<IActionResult> EditSgd(Guid id, string returnUrl, PrlOrganizationExtFullDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(model.EDRPOU))
            {
                ModelState.Clear();
                ModelState.AddModelError("EDRPOU", "Поле необхідне для заповнення");
                return View(model);
            }

            if (model.StreetId != Guid.Empty && model.CityId != Guid.Empty)
            {
                // ATU
                var newSubjAddress = new SubjectAddress
                {
                    StreetId = model.StreetId,
                    PostIndex = model.PostIndex,
                    Building = model.Building,
                };
                if (!_atuAddressService.SaveAddress(newSubjAddress))
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Вулиця не знайдена у довіднику");
                    return View(model);
                }
                else
                {
                    model.AddressId = newSubjAddress.Id;
                }
            }

            var errors = await _service.SaveNewOrg(model);

            if (errors.Any())
            {
                foreach (var (key, value) in errors)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(key, value);
                }

                return View(model);
            }

            return await Task.Run(() =>
            {
                return Redirect(returnUrl.Contains('?')
                    ? $"{returnUrl}&orgId={model.Id}"
                    : $"{returnUrl}?orgId={model.Id}");
            });

        }

        public async Task<IActionResult> EditFop(Guid? id, string returnUrl)
        {
            if ((id != Guid.Empty && id != null)
                || string.IsNullOrEmpty(returnUrl))
            {
                return View($"{nameof(ChooseOrganization)}Error");
            }

            ViewBag.returnUrl = returnUrl;
            return View(new PrlOrganizationExtFullDTO());
        }

        [HttpPost]
        public async Task<IActionResult> EditFop(Guid id, string returnUrl, PrlOrganizationExtFullDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(model.INN))
            {
                ModelState.Clear();
                ModelState.AddModelError("INN","Поле необхідне для заповнення");
                return View(model);
            }

            if (model.StreetId != Guid.Empty && model.CityId != Guid.Empty)
            {
                // ATU
                var newSubjAddress = new SubjectAddress
                {
                    StreetId = model.StreetId,
                    PostIndex = model.PostIndex,
                    Building = model.Building,
                };
                if (!_atuAddressService.SaveAddress(newSubjAddress))
                {
                    ModelState.AddModelError("", "Вулиця не знайдена у довіднику");
                    return View(model);
                }
                else
                {
                    model.AddressId = newSubjAddress.Id;
                }
            }
            
            var errors = await _service.SaveNewOrg(model);

            if (errors.Any())
            {
                foreach (var (key, value) in errors)
                {
                    ModelState.AddModelError(key, value);
                }

                return View(model);
            }

            return await Task.Run(() =>
            {
                return Redirect(returnUrl.Contains('?')
                    ? $"{returnUrl}&orgId={model.Id}"
                    : $"{returnUrl}?orgId={model.Id}");
            });
        }

        //private Dictionary<string, string> CheckFields(PrlOrganizationExtFullDTO model)
        //{
        //    var errors = new Dictionary<string, string>();

        //    if (string.IsNullOrEmpty(model.EDRPOU))
        //    {
        //        errors.Add("EDRPOU","Це поле має бути заповненим");
        //    }

        //    return errors;
        //}

        #region non-action overloads

        [NonAction]
        public override async Task<IActionResult> Index()
        {
            return await Task.Run(() => NotFound());
        }

        [NonAction]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return await Task.Run(() => NotFound());
        }

        [NonAction]
        public override async Task<IActionResult> Edit(Guid id, PrlOrganizationExtFullDTO model)
        {
            return await Task.Run(() => NotFound());
        }

        #endregion
    }
}