using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.IML;
using App.Data.DTO.PRL;
using App.Data.Models.LIC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.REG.Controllers
{
    [Area("REG")]
    [AllowAnonymous]
    public class ImlLicenseController: CommonController<ImlLicenseRegisterListDTO, ImlLicenseRegisterDetailDTO, License>
    {
        public ImlLicenseController(ICommonDataService dataService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService)
            : base(dataService, configuration, searchFilterSettingsService)
        {
        }

        public override Task<IActionResult> List(IDictionary<string, string> paramList, Core.Mvc.Controllers.ActionListOption<ImlLicenseRegisterListDTO> options)
        {
           // options.pg_SortExpression = options.pg_SortExpression ?? "-ModifiedOn";
            return PartialList(paramList, options);
        }
    }
}
