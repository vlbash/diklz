using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Business.Services;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;
using App.Core.Data.Interfaces;
using App.Core.Mvc.Extensions;

namespace App.Core.Mvc.Controllers
{
    public class CommonController<TDTO, TEntity>: CommonController<TDTO, TDTO, TEntity>
        where TDTO : CoreDTO
        where TEntity : CoreEntity
    {
        public CommonController(ICommonDataService dataService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService) : base(dataService, configuration, searchFilterSettingsService)
        {
        }
    }

    public class CommonController<TListDTO, TDetailDTO, TEntity>: Controller
        where TListDTO : CoreDTO
        where TDetailDTO : CoreDTO
        where TEntity : CoreEntity
    {
        #region FieldsAndProperties
        protected ICommonDataService DataService { get; }
        private readonly IConfiguration _configuration;
        private readonly ISearchFilterSettingsService _searchFilterSettingsService;

        private static int? _pageRowCount;
        protected int? PageRowCount =>
            _pageRowCount ?? (_pageRowCount = int.Parse(_configuration["Presentation:Paging:RowCount"]));

        protected string ListSortExpressionDefault = "Id";
        #endregion

        #region Constructors
        public CommonController(ICommonDataService dataService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService)
        {
            DataService = dataService;
            _configuration = configuration;
            _searchFilterSettingsService = searchFilterSettingsService;
        }
        #endregion Constructors

        #region Methods
        #region Actions
        public virtual async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public virtual async Task<IActionResult> List(IDictionary<string, string> paramList,
            ActionListOption<TListDTO> options)
        {
            return await PartialList(paramList, options, null);
        }

        public async Task<IActionResult> PartialList<T>(IDictionary<string, string> paramList,
            ActionListOption<T> options) where T : CoreDTO
        {
            return await PartialList<T>(paramList, options, null);
        }

        public virtual async Task<IActionResult> Details(Guid id)
        {
            return await Details(id, null);
        }

        public virtual async Task<IActionResult> Edit(Guid? id)
        {
            return await Edit(id, null, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(Guid id, TDetailDTO model)
        {
            return await Edit(id, model, null);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return await Delete(id, softDeleting, null);
        }
        #endregion Actions

        #region NonActions
        [NonAction]
        public async Task<IActionResult> List(IDictionary<string, string> paramList,
            ActionListOption<TListDTO> options,
            Func<IDictionary<string, string>, ActionListOption<TListDTO>, Task<IEnumerable<TListDTO>>> listFunction)
        {
            return await PartialList<TListDTO>(paramList, options, listFunction);
        }

        [NonAction]
        public async Task<IActionResult> PartialList<T>(IDictionary<string, string> paramList,
            ActionListOption<T> options,
            Func<IDictionary<string, string>, ActionListOption<T>, Task<IEnumerable<T>>> listFunction) where T : CoreDTO
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
                list = await DataService.GetDtoAsync<T>(orderBy, parameters: paramList, skip: (options.pg_Page - 1) * PageRowCount.Value, take: PageRowCount.Value);
            }
            pagingList = PagingList.Create(list,
            PageRowCount.Value,
            options.pg_Page,
            orderBy,
            "Id",
            x => (x as IPagingCounted)?.TotalRecordCount,
            options.pg_PartialActionName,
            true);

            return PartialView(options.pg_PartialViewName, pagingList);
        }

        [NonAction]
        public async Task<IActionResult> Details(Guid id, Func<Guid, Task<TDetailDTO>> detailFunction)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            TDetailDTO model;
            if (detailFunction != null)
            {
                model = await detailFunction(id);
            }
            else
            {
                model = (await DataService.GetDtoAsync<TDetailDTO>(x => x.Id == id)).SingleOrDefault();
            }

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [NonAction]
        public async Task<IActionResult> Edit(Guid? id, IDictionary<string, string> paramList, Func<Guid?, IDictionary<string, string>, Task<TDetailDTO>> editFunction)
        {
            TDetailDTO model;
            if (editFunction != null)
            {
                model = await editFunction(id, paramList);
            }
            else if (id == null)
            {
                model = Activator.CreateInstance<TDetailDTO>();
            }
            else
            {
                model = (await DataService.GetDtoAsync<TDetailDTO>(x => x.Id == id.Value)).SingleOrDefault();
            }

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [NonAction]
        public async Task<IActionResult> Edit(Guid id, TDetailDTO model, Func<TDetailDTO, Task<Dictionary<string, string>>> editFunction)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (editFunction != null)
                {
                    var errors = await editFunction(model);
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }
                    if (errors.Any())
                    {
                        return View(model);
                    }
                }
                else
                {
                    model.Id = DataService.Add<TEntity>(model);
                    await DataService.SaveChangesAsync();
                }

                return this.RedirectBack(nameof(Details), new { model.Id });
            }

            return View(model);
        }

        [NonAction]
        public async Task<IActionResult> Delete(Guid id, bool softDeleting, Func<Guid, bool, Task> deleteAction)
        {
            if (deleteAction != null)
            {
                await deleteAction(id, softDeleting);
            }
            else
            {
                if (softDeleting)
                {
                    DataService.Disable<TEntity>(id);
                }
                else
                {
                    DataService.Remove<TEntity>(id);
                }
                await DataService.SaveChangesAsync();
            }

            return await Task.FromResult(Json(new { success = true }));
        }
        #endregion NonActions

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
            return _searchFilterSettingsService.GenerateInputConfig(typeof(TListDTO));
        }
        #endregion SearchForm
        #endregion Methods
    }
}
