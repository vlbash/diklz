using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Services.Common;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Interfaces;
using App.Core.Mvc.Controllers;
using App.Data.DTO.LOG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;

namespace App.HostBack.Areas.AUD.Controllers
{
    [Authorize]
    [Area("AUD")]
    public class AuditController: Controller
    {
        #region CommonController
        private readonly IConfiguration _configuration;
        private readonly ISearchFilterSettingsService _searchFilterSettingsService;

        private static int? _pageRowCount;
        protected int? PageRowCount =>
            _pageRowCount ?? (_pageRowCount = int.Parse(_configuration["Presentation:Paging:RowCount"]));

        protected string ListSortExpressionDefault = "Id";

        #region SearchForm
        public string GetPresettings(string journalName)
        {
            return _searchFilterSettingsService.GetUserPresettings(journalName);
        }

        public async Task<JsonResult> SetPresettings(string journalName, string presettingsJson)
        {
            try
            {
                await _searchFilterSettingsService.SetUserPresettings(journalName, presettingsJson);

                return await Task.FromResult(Json(new { setSuccess = true }));

            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { setSuccess = false, ErrorMessage = "Помилка. " + (e.InnerException ?? e).Message }));
            }
        }

        public virtual string GenerateInputConfig()
        {
            return _searchFilterSettingsService.GenerateInputConfig(typeof(LogAuditEntryListDTO));
        }
        #endregion SearchForm

        public async Task<PagingList<T>> PartialList<T>(IDictionary<string, string> paramList,
            ActionListOption<T> options, Func<IDictionary<string, string>, ActionListOption<T>, Task<IEnumerable<T>>> listFunction,
            params object[] extraParameters) where T : CoreDTO
        {
            if (options == null)
            {
                options = new ActionListOption<T>();
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
                list = await _auditService.DataService.GetDtoAsync<T>(orderBy, parameters: paramList, skip: (options.pg_Page - 1) * PageRowCount.Value, take: PageRowCount.Value, extraParameters: extraParameters);
            }

            pagingList = PagingList.Create(list,
                PageRowCount.Value,
                options.pg_Page,
                orderBy,
                "Id",
                x => (x as IPagingCounted)?.TotalRecordCount,
                options.pg_PartialViewName ?? "List",
                true);

            return pagingList;
        }

        #endregion

        private AuditService _auditService { get; }
        public AuditController(IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, AuditService auditService)
        {
            _configuration = configuration;
            _searchFilterSettingsService = searchFilterSettingsService;
            _auditService = auditService;
        }

        [BreadCrumb(Title = "Журнал аудиту", Order = 1)]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<LogAuditEntryListDTO> options)
        {
            options.pg_SortExpression = !string.IsNullOrEmpty(options.pg_SortExpression)
                ? options.pg_SortExpression
                : "-CreatedDate";

            var pagingList = await PartialList(paramList, options, null, _auditService.StrFollowTable);

            ViewBag.CreatedFrom = paramList.FirstOrDefault(p => p.Key == "CreatedDate_From").Value;
            ViewBag.CreatedTo = paramList.FirstOrDefault(p => p.Key == "CreatedDate_To").Value;

            return PartialView(options.pg_PartialViewName, pagingList);
        }

        [BreadCrumb(Title = "Аудит таблиці", Order = 2)]
        public async Task<IActionResult> Details(int auditId, DateTime dateFrom, DateTime dateTo)
        {
            var audit = (await _auditService.DataService.GetDtoAsync<LogAuditEntryListDTO>(p => p.AuditEntryId == auditId,extraParameters: _auditService.StrFollowTable)).SingleOrDefault();

            ViewBag.FilterDate = $"{dateFrom.ToShortDateString()}||{dateTo.ToShortDateString()}";

            return View(audit);
        }

        public async Task<IActionResult> AuditPropertiesList(int auditId,
            IDictionary<string, string> paramList, ActionListOption<LogAuditListOfChangesDTO> options)
        {
            options.pg_PartialViewName = "_AuditProperties";

            var pagingList = await PartialList(paramList, options, async (x, y) => await _auditService.DataService.GetDtoAsync<LogAuditListOfChangesDTO>(aud => aud.AuditEntryId == auditId));
            _auditService.SetDisplayNameEntityProp(pagingList);

            var audHistory = _auditService.GeEntityHistoryById(auditId);
            #region Filter date

            var paramDate = paramList.FirstOrDefault(p => p.Key == "filterDate");
            if (!string.IsNullOrEmpty(paramDate.Value))
            {
                var filterDate = paramDate.Value.Split("||");
                if (filterDate.Length >= 1)
                {
                    if (DateTime.TryParse(filterDate[0], out var dateFrom))
                    {
                        if(dateFrom != DateTime.MinValue)
                            audHistory = audHistory.Where(p => p.CreatedDate.Date >= dateFrom.Date);
                    }
                }
                if (filterDate.Length == 2)
                {
                    if (DateTime.TryParse(filterDate[1], out var dateTo))
                    {
                        if(dateTo != DateTime.MinValue)
                            audHistory = audHistory.Where(p => p.CreatedDate.Date <= dateTo.Date);
                    }
                }
            }

            #endregion

            var audEntityHistory = audHistory.Select(p => new
            {
                Id = p.AuditEntryId,
                Name = p.CreatedDate.ToString("dd-MM-yyyy HH:mm") + " - " + p.CreatedBy
            });
            ViewBag.listOfDate = new SelectList(audEntityHistory, "Id", "Name", auditId);

            return PartialView(options.pg_PartialViewName, pagingList);
        }
    }
}
