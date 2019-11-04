using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.ImlServices;
using App.Business.Services.RptServices;
using App.Business.Services.UserServices;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.Common;
using App.Data.DTO.DOS;
using App.Data.DTO.IML;
using App.Data.Enums;
using App.Data.Models.APP;
using App.Data.Models.IML;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;
using Serilog;

namespace App.HostBack.Areas.IML.Controllers
{
    [Authorize]
    [Area("IML")]
    public class ApplicationController : CommonController<ImlAppListDTO, ImlAppDetailDTO, ImlApplication>
    {
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly ImlApplicationService _imlApplicationService;
        private readonly ImlApplicationProcessService _imlApplicationProcessService;
        private readonly ApplicationService<ImlApplication> _applicationService;
        private readonly BackOfficeUserService _backOfficeUserService;
        private readonly IAtuAddressService _atuAddressService;
        public readonly IConfiguration _configuration;
        private readonly IImlLicenseService _imlLicenseService;
        private readonly IConverter _converter;
        private readonly ImlApplicationService _imlAppService;

        public ApplicationController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, IEntityStateHelper entityStateHelper, ImlApplicationService imlApplicationService, ApplicationService<ImlApplication> applicationService, IAtuAddressService atuAddressService, BackOfficeUserService backOfficeUserService, ImlApplicationProcessService imlApplicationProcessService, IImlLicenseService imlLicenseService, IConverter converter, ImlApplicationService imlAppService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _configuration = configuration;
            _entityStateHelper = entityStateHelper;
            _imlApplicationService = imlApplicationService;
            _applicationService = applicationService;
            _atuAddressService = atuAddressService;
            _backOfficeUserService = backOfficeUserService;
            _imlApplicationProcessService = imlApplicationProcessService;
            _imlLicenseService = imlLicenseService;
            _converter = converter;
            _imlAppService = imlAppService;
        }

        [BreadCrumb(Title = "Журнал заяв щодо ліцензування імпорту", UseDefaultRouteUrl = true, Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        [BreadCrumb(Title = "Журнал заяв щодо ліцензування імпорту", UseDefaultRouteUrl = true, Order = 1)]
        public IActionResult IndexAppState(string appState)
        {
            ViewBag.AppState = appState;
            return View("Index");
        }
        public async Task<IActionResult> AppTypes(Guid? orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                return await Task.Run(() => View($"{nameof(AppTypes)}Error"));
            }

            ImlOrganizationExtShortDTO org;
            try
            {
                org = DataService.GetDto<ImlOrganizationExtShortDTO>(dto => dto.Id == orgId).Single();
            }
            catch (Exception e)
            {
                throw new Exception("Невірно вказан id організації", e);
            }

            ViewBag.OrgId = orgId;
            ViewBag.OrgName = org.Name;
            ViewBag.Number = org.edrpouOrInn;

            ViewBag.ImlLicense = _imlLicenseService.GetLicenseGuid(orgId);

            return await Task.Run(() => View());
        }

        [BreadCrumb(Title = "Заява про отримання ліцензії на провадження діяльності", Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            var app = (await _imlApplicationService.DataService.GetDtoAsync<AppShortDTO>(x => x.Id == id)).FirstOrDefault();
            var appSort = app.AppSort;
            if (string.IsNullOrEmpty(appSort))
                return await Task.Run(() => NotFound());
            if (appSort != "GetLicenseApplication" && appSort != "AdditionalInfoToLicense" && appSort != "IncreaseToIMLApplication")
            {
                return RedirectToAction("AltAppDetails", "ImlAppAlt", new { Area = "IML", id = id, sort = appSort });
            }
            if (appSort == "GetLicenseApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");
            if (appSort == "AdditionalInfoToLicense")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Доповнення інформації по наявній ліцензії");
            if (appSort == "IncreaseToIMLApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності " +
                                                                  "- Розширення до торгівлі лікарськими засобами");
            if (_entityStateHelper.IsEditableApp(id) == null)
                return NotFound();

            ViewBag.IsEditable = _entityStateHelper.IsEditableApp(id);

            var model = (await _imlApplicationService.DataService.GetDtoAsync<ImlAppDetailDTO>(x => x.Id == id)).SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.PerformerName = _imlApplicationService.DataService.GetDto<UserDetailsDTO>(p => p.Id == model.PerformerId).Select(p => p.FIO).SingleOrDefault();
            return View(model);
        }

        [HttpPost]
        public IActionResult ReturnApplication(AppShortDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = _imlApplicationProcessService.ReturnApplication(model);
            return Json(new { success = result });
        }

        [HttpPost]
        public IActionResult RegisterApplication(AppShortDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }

            var result = _imlApplicationProcessService.RegisterApplication(model);
            return Json(new { success = result });
        }

        public IActionResult ModalRegisterApplication(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var appShortDto = _imlApplicationService.DataService.GetDto<AppShortDTO>(p => p.Id == id).SingleOrDefault();

            if (appShortDto == null)
            {
                return NotFound();
            }

            if (appShortDto.PerformerId != Guid.Empty)
            {
                appShortDto.PerformerName = _imlApplicationService.DataService.GetDto<UserDetailsDTO>(p => p.Id == appShortDto.PerformerId).Select(p => p.FIO).SingleOrDefault();
            }

            return PartialView("Modals/_AppShortEdit", appShortDto);

        }

        public IActionResult ModalReturnApplication(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var appShortDto = _imlAppService.DataService.GetDto<AppShortDTO>(p => p.Id == id).SingleOrDefault();

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
            var model = new ImlAppDetailDTO();
            if (id == null)
                await _imlApplicationService.BackCreateApplication(model, orgId, appSort);
            else
                return await base.Edit(id);
            return View(model);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, ImlAppDetailDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(model.EDRPOU) && string.IsNullOrEmpty(model.INN))
            {
                ModelState.Clear();
                ModelState.AddModelError("EDRPOU", "Поле необхідне для заповнення");
                ModelState.AddModelError("INN", "Поле необхідне для заповнення");
                return View(model);
            }

            if (model.OrgType == OrgType.FOP)
            {
                if (model.PassportNumber.Length != 6 && model.PassportNumber.Length != 9)
                {
                    ModelState.AddModelError("PassportNumber", "Довжина номеру паспорту має бути 6, або 9 (при викориристанні ID-картки)");
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.PassportSerial))
                {
                    if (model.PassportNumber.Length != 9)
                    {
                        ModelState.AddModelError("PassportSerial", "Поле необхідне для заповнення (при використанні паспорту)");
                        return View(model);
                    }
                }
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


            var appId = await _imlApplicationService.SaveApplication(model, true);
            return RedirectToAction("Details", new { id = appId });


        }

        public override Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<ImlAppListDTO> options)
        {
            options.pg_SortExpression = options.pg_SortExpression ?? "-ModifiedOn";
            return PartialList(paramList, options);
        }

        public async Task<IActionResult> ProtocolPartialList(Guid protocolId,
            IDictionary<string, string> paramList, ActionListOption<ImlAppListDTO> options)
        {
            options.pg_PartialViewName = options.pg_PartialViewName ?? "ProtocolPartialView";
            return await Partial(paramList, options,
                async (x, y) =>
                        await _imlApplicationService.DataService.GetDtoAsync<ImlAppListDTO>(app => app.ProtocolId == protocolId));
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

        public async Task<IActionResult> GetImlApplicationReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            byte[] file;

            var application = (await DataService.GetDtoAsync<AppStateDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }

            var sort = application.AppSort;

            try
            {
                file = await _imlApplicationService.GetApplicationFile(id, sort);
            }

            catch (Exception)
            {
                return await Task.Run(() => NotFound());
            }

            return File(file, "application/pdf");
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
                await _imlApplicationService.SubmitBackOfficeApplication(_configuration, id);
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
            var app = _imlApplicationService.DataService.GetEntity<ImlApplication>(x => x.Id == id).FirstOrDefault();
            app.BackOfficeAppState = "InReview";
            await _imlApplicationService.DataService.SaveChangesAsync();
            return RedirectToAction("Details", "Application", new { id = id }, "applyApp");
        }

        [HttpPost]
        public async Task<IActionResult> AdditionalInfoToLicenseSubmit(Guid id, string type, string text)
        {
            if (string.IsNullOrEmpty(text))
                return NotFound();
            if (type == "Submit")
            {
                try
                {
                    await _imlApplicationService.SubmitAdditionalInfoToLicense(id, text);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return Json(new { success = false, alert = "Виникла помилка серверу, спробуйте пізніше!" });
                }
            }
            else if (type == "Reject")
            {
                var app = _imlApplicationService.DataService.GetEntity<ImlApplication>(x => x.Id == id).FirstOrDefault();
                app.BackOfficeAppState = "Reviewed";
                app.AppState = "Reviewed";
                var decision = new AppDecision { Id = Guid.NewGuid(), AppId = id, DecisionType = "Denied", DecisionDescription = text };
                app.AppDecisionId = decision.Id;
                _imlApplicationService.DataService.Add(decision);
                await _imlApplicationService.DataService.SaveChangesAsync();
            }
            else
                return Json(new { success = false, alert = "Неправильно введені дані, спробуйте ще раз" });
            return Json(new { success = true });
        }

        #region Payment

        public IActionResult ConfirmPayment(Guid appId)
        {
            _applicationService.ChangePaymentStatus(appId, "PaymentConfirmed");

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult CancelPayment(Guid appId)
        {
            var edoc = _imlApplicationService.DataService.GetDto<EDocumentDetailsDTO>(p => p.EntityId == appId && p.EDocumentType == "PaymentDocument" && p.EDocumentStatus == "WaitingForConfirmation",
                extraParameters: new object[] { $"\'{appId}\'" }).SingleOrDefault();

            return PartialView("_ModalPaymentCommentEdit", edoc);
        }

        [HttpPost]
        public IActionResult CancelPayment(EDocumentDetailsDTO edoc)
        {
            if (!ModelState.IsValid || edoc.EntityId == null)
            {
                return PartialView("_ModalPaymentCommentEdit", edoc);
            }

            _applicationService.ChangePaymentStatus(edoc.EntityId.Value, "PaymentNotVerified", edoc.Comment);
            return Json(new { success = true });

        }

        public IActionResult SendPayment(Guid appId)
        {
            var result = _applicationService.ChangePaymentStatus(appId, "WaitingForConfirmation");
            if (!result)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        #endregion

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
