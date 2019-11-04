using System;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.IML;

namespace App.Business.Services.ImlServices
{
    public interface IImlLicenseService
    {
        ICommonDataService _commonDataService { get; set; }

        Guid? GetLicenseGuid(Guid? orgId);
        Guid? GetLicenseGuid();

        Task<ImlLicenseDetailDTO> LicenseDetail(Guid id);
    }
}
