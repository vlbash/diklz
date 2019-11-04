using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.Common;
using App.Data.Models.ORG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Controllers
{
    [Authorize]
    public class UserController: CommonController<UserDetailsDTO, UserDetailsDTO, EmployeeExt>
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _userInfoService;
        public UserController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService filterSettingsService, IUserInfoService userInfoService)
            : base(dataService, configuration, filterSettingsService)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
        }
        public async Task<IActionResult> PersonalCabinet()
        {
            var employeeId = (await _userInfoService.GetCurrentUserInfoAsync()).UserId;
            var model = (await _dataService.GetDtoAsync<UserDetailsDTO>(x=>x.Id == employeeId)).FirstOrDefault();
            return PartialView(model);
        }
    }
}
