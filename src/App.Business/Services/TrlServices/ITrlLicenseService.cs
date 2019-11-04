using System;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.TRL;

namespace App.Business.Services.TrlServices
{
    public interface ITrlLicenseService
    {
        ICommonDataService _commonDataService { get; set; }

        Guid? GetLicenseGuid(Guid? orgId);
        Guid? GetLicenseGuid();

        Task<TrlLicenseDetailDTO> LicenseDetail(Guid id);
    }
}
