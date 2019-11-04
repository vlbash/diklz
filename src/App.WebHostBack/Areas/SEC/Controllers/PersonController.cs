using App.Core.Business.Services;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.SEC.Controllers
{
    [Area("SEC")]
    [Authorize]
    public class PersonController: CommonController<PersonListDTO, PersonDetailDTO, Person>
    {
        public PersonController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService) 
            : base(dataService, configuration, searchFilterSettingsService)
        {
        }
    }
}
