using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.IML;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.IML.Controllers
{
    [Area("Iml")]
    public class ImlMedicineController: CommonController<ImlMedicineRegisterListDTO, ImlMedicine>
    {
        public ImlMedicineController(IConfiguration configuration, ISearchFilterSettingsService filterSettingsService, ImlMedicineService medicineService)
            : base(medicineService.DataService, configuration, filterSettingsService)
        {
        }

        [NonAction]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return NotFound();
        }

        [HttpGet]
        public override async Task<IActionResult> List(IDictionary<string, string> paramList,
            ActionListOption<ImlMedicineRegisterListDTO> options)
        {
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<ImlMedicineRegisterListDTO>(
                orderBy: options.pg_SortExpression ?? "-MedicineName",
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value));
        }
    }
}
