using System;
using System.Threading.Tasks;

namespace App.Business.Services.RptServices
{
    public interface IPrlReportService
    {
        Task<string> PrlGetAppSort(Guid id);

        Task<string> PrlCreateLicenseApp(Guid id);
        Task<string> PrlCancelLicenseApp(Guid id);
        Task<string> PrlRemBranchApp(Guid id);
        Task<string> PrlChangeContrApp(Guid id);
        Task<string> PrlChangeAutPersonApp(Guid id);
        Task<string> PrlAddBranchInfoApp(Guid id);
        Task<string> PrlRemBranchInfoApp(Guid id);
        Task<string> PrlAddBranchApp(Guid id);
        //Task<string> PrlRenewLicenseApp(Guid id);
    }
}
