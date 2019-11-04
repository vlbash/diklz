using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.DOS;
using App.Data.Models.DOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.DOC.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("DOC")]
    public class RegisterController : CommonController<RegisterEDocumentListDTO, EDocumentDetailsDTO, EDocument>
    {
        private readonly ICommonDataService _commonDataService;
        public RegisterController(ICommonDataService commonDataService, IConfiguration configuration,
            ISearchFilterSettingsService filterSettingsService) : base(commonDataService, configuration,
            filterSettingsService)
        {
            _commonDataService = commonDataService;
        }

        public async Task<IActionResult> EDocuments()
        {
            return View();
        }

        public async Task<IActionResult> PrlDossierList(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<RegisterEDocumentListDTO> options)
        {
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<RegisterEDocumentListDTO>
            (
                orderBy: options.pg_SortExpression ?? "-DateFrom",
                predicate: dto => dto.EDocumentType == "ManufactureDossier",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value
            ));
        }

        public async Task<IActionResult> ImlDossierList(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<RegisterEDocumentListDTO> options)
        {
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<RegisterEDocumentListDTO>
            (
                orderBy: options.pg_SortExpression ?? "-DateFrom",
                predicate: dto => dto.EDocumentType == "ImportDossier",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value
            ));
        }

        public async Task<IActionResult> PayrollWholesaleList(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<RegisterEDocumentListDTO> options)
        {
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<RegisterEDocumentListDTO>
            (
                orderBy: options.pg_SortExpression ?? "-DateFrom",
                predicate: dto => dto.EDocumentType == "PayrollWholesale",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value
            ));
        }

        public async Task<IActionResult> PayrollPharmacyProductionList(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<RegisterEDocumentListDTO> options)
        {
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<RegisterEDocumentListDTO>
            (
                orderBy: options.pg_SortExpression ?? "-DateFrom",
                predicate: dto => dto.EDocumentType == "PayrollPharmacyProduction",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value
            ));
        }

        public async Task<IActionResult> PayrollRetailList(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<RegisterEDocumentListDTO> options)
        {
            return await base.PartialList<RegisterEDocumentListDTO>(paramList, options, (dictionary, option) => DataService.GetDtoAsync<RegisterEDocumentListDTO>
            (
                orderBy: options.pg_SortExpression ?? "-DateFrom",
                predicate: dto => dto.EDocumentType == "PayrollRetail",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value
            ));
        }



    }
}
