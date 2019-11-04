using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.RptServices;
using App.Business.Services.TrlServices;
using App.Business.Services.UserServices;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.Common;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;
using App.Data.Enums;
using App.Data.Models.TRL;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;
using Serilog;

namespace App.HostBack.Areas.TRL.Controllers
{
    [Authorize]
    [Area("TRL")]
    public class ApplicationController: CommonController<TrlAppListDTO, TrlAppDetailDTO, TrlApplication>
    {
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly BackOfficeUserService _backOfficeUserService;
        private readonly IAtuAddressService _atuAddressService;
        public readonly IConfiguration _configuration;
        private readonly TrlLicenseService _trlLicenseService;
        private readonly TrlApplicationService _trlApplicationService;
        private readonly TrlApplicationProcessService _trlApplicationProcessService;
        private readonly ApplicationService<TrlApplication> _applicationService;
        private readonly TrlApplicationService _trlAppService;
        private readonly IConverter _converter;

        public ApplicationController(ICommonDataService dataService, 
                                     IAtuAddressService atuAddressService, 
                                     ISearchFilterSettingsService searchFilterSettingsService, 
                                     IEntityStateHelper entityStateHelper,
                                     TrlApplicationProcessService trlApplicationProcessService,
                                     BackOfficeUserService backOfficeUserService, 
                                     TrlLicenseService trlLicenseService,
                                     TrlApplicationService trlApplicationService,
                                     ApplicationService<TrlApplication> applicationService,
                                     IConfiguration configuration, IConverter converter, TrlApplicationService trlAppService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _atuAddressService = atuAddressService;
            _entityStateHelper = entityStateHelper;
            _trlApplicationProcessService = trlApplicationProcessService;
            _backOfficeUserService = backOfficeUserService;
            _trlLicenseService = trlLicenseService;
            _configuration = configuration;
            _converter = converter;
            _trlAppService = trlAppService;
            _trlApplicationService = trlApplicationService;
            _applicationService = applicationService;
        }

        [BreadCrumb(Title = "Журнал заяв щодо ліцензування торгівлі", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        [BreadCrumb(Title = "Журнал заяв щодо ліцензування торгівлі", UseDefaultRouteUrl = true, Order = 1)]
        public IActionResult IndexAppState(string appState)
        {
            ViewBag.AppState = appState;
            return View("Index");
        }

        public override Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<TrlAppListDTO> options)
        {
            options.pg_SortExpression = options.pg_SortExpression ?? "-ModifiedOn";
            return PartialList(paramList, options);
        }

        [BreadCrumb(Title = "Заява про отримання ліцензії на провадження діяльності", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            var app = (await _trlApplicationService.DataService.GetDtoAsync<AppShortDTO>(x => x.Id == id)).FirstOrDefault();
            var appSort = app.AppSort;
            if (string.IsNullOrEmpty(appSort))
                return await Task.Run(() => NotFound());
            if (appSort != "GetLicenseApplication" && appSort != "AdditionalInfoToLicense" && appSort != "IncreaseToTRLApplication")
            {
                return RedirectToAction("AltAppDetails", "TrlAppAlt", new { Area = "TRL", id = id, sort = appSort });
            }
            if (appSort == "GetLicenseApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");
            if (appSort == "AdditionalInfoToLicense")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Доповнення інформації по наявній ліцензії");
            if (appSort == "IncreaseToTRLApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності " +
                                                                  "- Розширення до торгівлі лікарськими засобами");
            if (_entityStateHelper.IsEditableApp(id) == null)
                return NotFound();

            ViewBag.IsEditable = _entityStateHelper.IsEditableApp(id);

            var model = (await _trlApplicationService.DataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.PerformerName = _trlApplicationService.DataService.GetDto<UserDetailsDTO>(p => p.Id == model.PerformerId).Select(p => p.FIO).SingleOrDefault();
            return View(model);
        }


        [HttpPost]
        public IActionResult ReturnApplication(AppShortDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = _trlApplicationProcessService.ReturnApplication(model);
            return Json(new { success = result });
        }

        [HttpPost]
        public IActionResult RegisterApplication(AppShortDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = _trlApplicationProcessService.RegisterApplication(model);
            return Json(new { success = result });
        }

        public IActionResult ModalRegisterApplication(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var appShortDto = _trlApplicationService.DataService.GetDto<AppShortDTO>(p => p.Id == id).SingleOrDefault();

            if (appShortDto == null)
            {
                return NotFound();
            }

            if (appShortDto.PerformerId != Guid.Empty)
            {
                appShortDto.PerformerName = _trlApplicationService.DataService.GetDto<UserDetailsDTO>(p => p.Id == appShortDto.PerformerId).Select(p => p.FIO).SingleOrDefault();
            }

            return PartialView("Modals/_AppShortEdit", appShortDto);

        }

        public IActionResult ModalReturnApplication(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var appShortDto = _trlAppService.DataService.GetDto<AppShortDTO>(p => p.Id == id).SingleOrDefault();

            if (appShortDto == null)
            {
                return NotFound();
            }

            //if (appShortDto.PerformerId != Guid.Empty)
            //{
            //    appShortDto.PerformerName = _prlAppService.DataService.GetDto<UserDetailsDTO>(p => p.Id == appShortDto.PerformerId).Select(p => p.FIO).SingleOrDefault();
            //}

            return PartialView("Modals/_AppReturnEdit", appShortDto);

        }
        [NonAction]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return await Task.Run(() => NotFound());
        }

        public async Task<IActionResult> Edit(Guid? id, Guid? orgId, string appSort = "GetLicenseApplication")
        {
            var enRecordList = DataService.GetEntity<EnumRecord>(x => x.EnumType == "TrlActivityType");

            var model = new TrlAppDetailDTO();
            if (id == null)
                await _trlApplicationService.BackCreateApplication(model, orgId, appSort);
            else
            {
                model = (await DataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id.Value)).FirstOrDefault();
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
                await _trlApplicationService.TrlActivityTypeList(model);
            }

            ViewBag.trlActivityTypeList =
                new MultiSelectList(enRecordList, nameof(EnumRecord.Id), nameof(EnumRecord.Name));

            //return await base.Edit(id);
            return View(model);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, TrlAppDetailDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!model.ListOfTrlActivityType?.Any() ?? true)
            {
                ModelState.AddModelError("ListOfTrlActivityType", "Має бути обрано щонайменьше один вид діяльності");
                var activityTypeList = DataService.GetEntity<EnumRecord>(x => x.EnumType == "TrlActivityType");
                ViewBag.trlActivityTypeList =
                    new MultiSelectList(activityTypeList, nameof(EnumRecord.Id), nameof(EnumRecord.Name));
            }

            if (model.OrgType == OrgType.FOP)
            {
                if (ModelState.ContainsKey("EDRPOU"))
                    ModelState["EDRPOU"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("OrgDirector"))
                    ModelState["OrgDirector"].ValidationState = ModelValidationState.Valid;
                if (model.PassportNumber.Length != 6 && model.PassportNumber.Length != 9)
                {
                    ModelState.AddModelError("PassportNumber", "Довжина номеру паспорту має бути 6, або 9 (при викориристанні ID-картки)");
                }
                if (string.IsNullOrEmpty(model.PassportSerial))
                {
                    if (model.PassportNumber.Length != 9)
                    {
                        ModelState.AddModelError("PassportSerial", "Поле необхідне для заповнення (при використанні паспорту)");
                    }
                }
            }
            else
            {
                if (ModelState.ContainsKey("INN"))
                    ModelState["INN"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportSerial"))
                    ModelState["PassportSerial"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportNumber"))
                    ModelState["PassportNumber"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportDate"))
                    ModelState["PassportDate"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportIssueUnit"))
                    ModelState["PassportIssueUnit"].ValidationState = ModelValidationState.Valid;
            }
            if (ModelState.IsValid)
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
                }
                else
                {
                    model.AddressId = newSubjAddress.Id;
                }
            }

            if (ModelState.IsValid)
            {
                var appId = await _trlApplicationService.SaveApplication(model, true);
                return RedirectToAction("Details", new { id = appId });
            }

            return View(model);
        }
        public async Task<IActionResult> ProtocolPartialList(Guid protocolId,
            IDictionary<string, string> paramList, ActionListOption<TrlAppListDTO> options)
        {
            options.pg_PartialViewName = options.pg_PartialViewName ?? "ProtocolPartialView";
            return await Partial(paramList, options,
                async (x, y) =>
                        await _trlApplicationService.DataService.GetDtoAsync<TrlAppListDTO>(app => app.ProtocolId == protocolId));
        }

        // Скопировал с Common потому что кол. страниц не соответствует действительности. В коре нужно исправить
        public async Task<IActionResult> Partial<T>(IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<T> options,
            Func<IDictionary<string, string>, Core.Mvc.Controllers.ActionListOption<T>, Task<IEnumerable<T>>> listFunction) where T : CoreDTO
        {
            if (options == null)
            {
                options = new Core.Mvc.Controllers.ActionListOption<T>();
            }

            ViewBag.FormparamList = string.Join("&", paramList.Select(x => string.Format("{0}={1}", x.Key, x.Value)));

            paramList = paramList
                .Where(x => !string.IsNullOrEmpty(x.Value) &&
                            x.Key != "__RequestVerificationToken" &&
                            x.Key != "X-Requested-With" &&
                            !x.Key.StartsWith("pg_"))
                .ToDictionary(x => x.Key, x => x.Value);

            var orderBy = options.pg_SortExpression ?? ListSortExpressionDefault;

            PagingList<T> pagingList;
            IEnumerable<T> list;
            if (listFunction != null)
            {
                list = await listFunction(paramList, options);
            }
            else
            {
                list = await DataService.GetDtoAsync<T>(orderBy, parameters: paramList, skip: (options.pg_Page - 1) * PageRowCount.Value, take: PageRowCount.Value);
            }
            pagingList = PagingList.Create(list,
                PageRowCount.Value,
                options.pg_Page,
                orderBy,
                "Id",
                x => list?.Count(),
                options.pg_PartialViewName ?? "List",
                true);

            return PartialView(options.pg_PartialViewName, pagingList);
        }

        public async Task<IActionResult> AppTypes(Guid? orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                return await Task.Run(() => View($"{nameof(AppTypes)}Error"));
            }

            TrlOrganizationExtShortDTO org;
            try
            {
                org = DataService.GetDto<TrlOrganizationExtShortDTO>(dto => dto.Id == orgId).Single();
            }
            catch (Exception e)
            {
                throw new Exception("Невірно вказан id організації", e);
            }

            ViewBag.OrgId = orgId;
            ViewBag.OrgName = org.Name;
            ViewBag.Number = org.edrpouOrInn;

            ViewBag.TrlLicense = _trlLicenseService.GetLicenseGuid(orgId);

            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCheckBox(string checkBoxId, Guid appId)
        {
            var success = await _applicationService.ChangeCheckBox(checkBoxId, appId);
            if (success)
                return Json(new { success = success });
            return StatusCode(500);

        }

        [HttpPost]
        public async Task<IActionResult> SaveComment(string text, Guid appId)
        {
            var success = await _applicationService.SaveComment(text, appId);
            if (success)
                return Json(new { success = success });
            return StatusCode(500);
        }

        public async Task<IActionResult> SubmitApplication(Guid id)
        {
            try
            {
                await _trlApplicationService.SubmitBackOfficeApplication(_configuration, id);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound();
            }
            return RedirectToAction("Details", "Application", new { id = id }, "applyApp");
        }

        public async Task<IActionResult> ReviewApplication(Guid id)
        {
            var app = _trlApplicationService.DataService.GetEntity<TrlApplication>(x => x.Id == id).FirstOrDefault();
            app.BackOfficeAppState = "InReview";
            await _trlApplicationService.DataService.SaveChangesAsync();
            return RedirectToAction("Details", "Application", new { id = id }, "applyApp");
        }

        public async Task<IActionResult> GetTrlApplicationReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] file;

            var application = (await DataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }
            var sort = application.AppSort;
            try
            {
                file = await _trlAppService.GetApplicationFile(id, sort);
            }
            catch (Exception e)
            {
                return await Task.Run(() => NotFound());
            }

            return File(file, "application/pdf");
        }

        #region AutoComplete

        [HttpPost]
        public JsonResult AutoCompletePerformer(string term)
        {
            var result = _backOfficeUserService.GetLimsEmployee().Where(p => p.FIO.Contains(term ?? "", StringComparison.InvariantCultureIgnoreCase)).Select(p => new
            {
                value = $"{p.FIO}",
                option = p.Id
            }).ToList();

            return Json(result);
        }

        #endregion
    }
}
