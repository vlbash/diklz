using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using App.Core.Data.Interfaces;
using App.PublicHost.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReflectionIT.Mvc.Paging;

// ReSharper disable Mvc.ViewNotResolved
// View implementation is not needed in base controllers

namespace App.PublicHost.Controllers
{
    public class ActionListOption<TDTO>
    {
        public string pg_SortExpression { get; set; }
        public int pg_Page { get; set; } = 1;
        public string pg_PartialViewName { get; set; } = "List";
        public IQueryable<TDTO> pg_QueryList { get; set; }
    }

    public class BaseController<TDTO, TEntity>: BaseController<TDTO, TDTO, TEntity>
        where TDTO : BaseDTO
        where TEntity : BaseEntity
    {
        public BaseController(bool disableInsteadOfDelete = false) : base(disableInsteadOfDelete)
        {
        }
    }

    public class BaseController<TListDTO, TDetailDTO, TEntity>: Controller
        where TListDTO : BaseDTO
        where TDetailDTO : BaseDTO
        where TEntity : BaseEntity
    {
        private IBaseService<TListDTO, TDetailDTO, TEntity> _service;
        public IBaseService<TListDTO, TDetailDTO, TEntity> Service
        {
            get
            {
                if (_service == null)
                {
                    _service = HttpContext.RequestServices.GetService<IBaseService<TListDTO, TDetailDTO, TEntity>>();
                };
                return _service;
            }
            set
            {
                _service = value;
            }
        }

        private IConfiguration _configuration;
        public IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = HttpContext.RequestServices.GetService<IConfiguration>();
                };
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        private static int? _pageRowCount;
        protected int? PageRowCount =>
            _pageRowCount ?? (_pageRowCount = int.Parse(Configuration["Presentation:Paging:RowCount"]));

        protected bool DisableInsteadOfDelete;
        protected bool TransactedDBOperations;
        protected string ListSortExpressionDefault = "Id";

        public delegate void ListQueryHandler(ref IQueryable<TListDTO> query);
        public event ListQueryHandler OnList;

        public delegate void DetailDTOHandler(TDetailDTO model);
        public event DetailDTOHandler OnDetails;
        public event DetailDTOHandler OnGetEdit;
        public event DetailDTOHandler BeforePostEdit;
        public event DetailDTOHandler OnPostEdit;

        public delegate void EntityHandler(TEntity model);
        public event EntityHandler OnDelete;

        public BaseController(bool disableInsteadOfDelete = false)
        {
            this.DisableInsteadOfDelete = disableInsteadOfDelete;
        }

        public virtual async Task<IActionResult> Index() => await Task.FromResult(View());

        public virtual async Task<IActionResult> List(IDictionary<string, string> paramList, ActionListOption<TListDTO> options)
        {
            if (options == null)
                options = new ActionListOption<TListDTO>();

            ViewBag.FormParamList = string.Join("&", paramList.Select(x => string.Format("{0}={1}", x.Key, x.Value)));

            paramList = paramList
                .Where(x => !string.IsNullOrEmpty(x.Value) &&
                            x.Key != "__RequestVerificationToken" &&
                            x.Key != "X-Requested-With" &&
                            !x.Key.StartsWith("pg_"))
                .ToDictionary(x => x.Key, x => x.Value);

            var iquerList = options.pg_QueryList ?? Service.GetListDTO(paramList);
            OnList?.Invoke(ref iquerList);

            var pagingList = await PagingList.CreateAsync(iquerList,
                PageRowCount.Value,
                options.pg_Page,
                options.pg_SortExpression ?? ListSortExpressionDefault,
                "Id",
                x => (x as IPagingCounted)?.TotalRecordCount,
                options.pg_PartialViewName ?? "List");

            return PartialView(options.pg_PartialViewName, pagingList);
        }

        public async Task<IActionResult> PartialList<T>(IDictionary<string, string> paramList, ActionListOption<T> options) where T : class
        {
            ViewBag.FormParamList = string.Join("&", paramList.Select(x => string.Format("{0}={1}", x.Key, x.Value)));

            var pagingList = await PagingList.CreateAsync(options.pg_QueryList,
                PageRowCount.Value,
                options.pg_Page,
                options.pg_SortExpression ?? ListSortExpressionDefault,
                "Id",
                x => (x as IPagingCounted)?.TotalRecordCount,
                options.pg_PartialViewName);

            return PartialView(options.pg_PartialViewName, pagingList);
        }


        public virtual async Task<IActionResult> Details(Guid? id)
        {
            var model = await Service.GetDetailDTO().SingleOrDefaultAsync(x => x.Id == id.Value);

            if (model == null)
            {
                return NotFound();
            }

            OnDetails?.Invoke(model);

            return View(model);
        }

        public virtual async Task<IActionResult> Edit(Guid? id)
        {
            TDetailDTO model = null;

            if (id == null)
                model = Activator.CreateInstance<TDetailDTO>();
            else
            {
                model = await Service.GetDetailDTO().SingleOrDefaultAsync(x => x.Id == id.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }

            OnGetEdit?.Invoke(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(Guid id, TDetailDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    BeforePostEdit?.Invoke(model);
                    await Service.SaveAsync(model, TransactedDBOperations);
                    OnPostEdit?.Invoke(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await Service.GetListDTO().SingleOrDefaultAsync(x => x.Id == id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return this.RedirectBack(nameof(Details), new { model.Id });
            }
            return View(model);
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                TEntity entity;
                if (DisableInsteadOfDelete)
                    entity = Service.Disable(id, TransactedDBOperations);
                else
                    entity = Service.Remove(id, TransactedDBOperations);

                OnDelete?.Invoke(entity);

                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Помилка видалення. " + (e.InnerException ?? e).Message }));
            }
        }
    }
}
