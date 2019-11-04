using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Business.Services.TrlServices;
using App.Core.Business.Services;
using App.Data.Models.PRL;

namespace App.Business.Services.AppServices
{
    public class LicenseService
    {
        private readonly IUserInfoService _infoService;
        private readonly IPrlLicenseService _prlLicenseService;
        private readonly IImlLicenseService _imlLicenseService;
        private readonly TrlLicenseService _trlLicenseService;

        public LicenseService(IUserInfoService infoService, IPrlLicenseService prlLicenseService, IImlLicenseService imlLicenseService, TrlLicenseService trlLicenseService)
        {
            _infoService = infoService;
            _prlLicenseService = prlLicenseService;
            _imlLicenseService = imlLicenseService;
            _trlLicenseService = trlLicenseService;
        }


        public async Task<List<(string type, bool isActive)>> GetActiveLicenses()
        {
            var list = new List<(string, bool)>
            {
                _trlLicenseService.GetLicenseGuid() == null ? ("TRL", false) : ("TRL", true),
                _prlLicenseService.GetLicenseGuid() == null ? ("PRL", false) : ("PRL", true),
                _imlLicenseService.GetLicenseGuid() == null ? ("IML", false) : ("IML", true)
            };
            return list;
        }

        public async Task<List<(string type, bool isActive)>> GetActiveLicenses(Guid? orgId)
        {
            var list = new List<(string, bool)>
            {
                _trlLicenseService.GetLicenseGuid(orgId) == null ? ("TRL", false) : ("TRL", true),
                _prlLicenseService.GetLicenseGuid(orgId) == null ? ("PRL", false) : ("PRL", true),
                _imlLicenseService.GetLicenseGuid(orgId) == null ? ("IML", false) : ("IML", true)
            };
            return list;
        }
    }
}
