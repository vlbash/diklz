using System;
using System.Collections.Generic;
using System.Text;
using App.Core.Business.Services;

namespace App.Business.Services.P902Services
{
    public class AppConclusionServise
    {
        private readonly ICommonDataService DataService;
        private readonly IUserInfoService _userInfoService;

        public AppConclusionServise(ICommonDataService dataService, IUserInfoService userInfoService)
        {
            DataService = dataService;
            _userInfoService = userInfoService;
        }

        
    }
}
