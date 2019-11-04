using System.Collections.Generic;
using System.Linq;
using App.Core.Business.Services;
using App.Data.DTO.Common;
using Microsoft.Extensions.Configuration;

namespace App.Business.Services.UserServices
{
    public class BackOfficeUserService
    {
        private IConfiguration Configuration { get; }
        private ICommonDataService _commonDataService { get; }

        private string StateMedicalService { get; }
        public BackOfficeUserService(IConfiguration configuration, ICommonDataService commonDataService)
        {
            Configuration = configuration;
            _commonDataService = commonDataService;
            StateMedicalService = Configuration.GetValue<string>("StateMedicalService");
        }

        public List<UserDetailsDTO> GetLimsEmployee() => 
            _commonDataService.GetDto<UserDetailsDTO>(p => p.Edrpou == StateMedicalService).OrderBy(p => p.FIO).ToList();
    }
}
