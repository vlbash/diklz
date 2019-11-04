using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.AppServices;
using App.Business.Services.BranchService;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.Models.ORG;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.PublicHost.Areas.BRN.Controllers
{
    [Area("BRN")]
    public class BranchController: CommonController<BranchRegisterListDTO, Branch>
    {
        private readonly IBranchService _branchService;
        public BranchController(ICommonDataService dataService, IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService, IBranchService branchService) : base(dataService,
            configuration, searchFilterSettingsService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public override async Task<IActionResult> List(IDictionary<string, string> paramList,ActionListOption<BranchRegisterListDTO> options)
        {
            ViewBag.AppType = (await DataService.GetDtoAsync<AppStateDTO>(x => x.Id == new Guid(paramList["ApplicationId"]))).Select(x => x.AppType).FirstOrDefault(); ;
            var orderBy = options.pg_SortExpression ?? ListSortExpressionDefault;
            return await base.List(paramList, options, async (x, y) =>
            {
                var models = (await DataService.GetDtoAsync<BranchRegisterListDTO>(orderBy, z => z.ApplicationId == new Guid(paramList["ApplicationId"]), parameters: paramList,
                    skip: (options.pg_Page - 1) * PageRowCount.Value, take: PageRowCount.Value)).ToList();
                models.ForEach(z => z.OperationListForm = _branchService.GetOperationList(z.OperationListForm));
                return models;
            });
        }
    }
}
