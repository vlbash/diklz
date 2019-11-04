using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Interfaces;
using App.Core.Mvc.Controllers;
using App.Data.DTO.MSG;
using App.Data.Models.MSG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;

namespace App.Host.Areas.MSG.Controllers
{
    [Area("Msg")]
    [Authorize(Policy = "Registered")]
    public class MessageController : CommonController<MessageListDTO, MessageListDTO, Message>
    {
        public MessageController(ICommonDataService DataService, IConfiguration configuration, ISearchFilterSettingsService filterSettingsService)
            : base(DataService, configuration, filterSettingsService)
        {
        }

        [BreadCrumb(Title = "Журнал повідомлень", Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<MessageListDTO> options)
        {
            return await Partial<MessageListDTO>(paramList, options, (dictionary, option) => DataService.GetDtoAsync<MessageListDTO>(
                orderBy: options.pg_SortExpression ?? "-CreatedOn",
                predicate: p => (p.IsCreatedOnPortal || p.MessageStateEnum != "Project") && (p.MessageHierarchyTypeEnum == "Single" || p.MessageHierarchyTypeEnum == "Parent"),
                parameters:dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value));
        }

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
    }
}
