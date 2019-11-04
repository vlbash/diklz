using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.PRL;
using App.Data.Models.LIC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.PublicHost.Areas.REG.Controllers
{
    [Area("REG")]
    [AllowAnonymous]
    public class PrlLicenseController: CommonController<PrlLicenseRegisterListDTO, PrlLicenseRegisterDetailDTO, License>
    {
        public PrlLicenseController(ICommonDataService dataService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService)
            : base(dataService, configuration, searchFilterSettingsService)
        {
        }

        public override Task<IActionResult> List(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<PrlLicenseRegisterListDTO> options)
        {
           // options.pg_SortExpression = options.pg_SortExpression ?? "-ModifiedOn";
            return PartialList(paramList, options);
        }
    }
}
