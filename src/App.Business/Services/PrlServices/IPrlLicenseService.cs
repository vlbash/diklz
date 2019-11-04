using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.PRL;

namespace App.Business.Services.PrlServices
{
    public interface IPrlLicenseService
    {
        ICommonDataService _commonDataService { get; set; }

        Guid? GetLicenseGuid(Guid? orgId);
        Guid? GetLicenseGuid();

        Task<PrlLicenseDetailDTO> LicenseDetail(Guid id);
    }
}
