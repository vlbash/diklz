using System;
using System.Threading.Tasks;

namespace App.Business.Services.PrlServices
{
    public interface IPrlApplicationAltService
    {
        Task<Guid> CreateOnOpen(string sort, Guid? id = null, bool isBackOffice = false);

        Task<bool> ChangeBranchDeleteCheck(Guid branchId);

        Task Delete(Guid id, bool softDeleting);

        Task<Guid> ProcessApplicationToApplication(Guid appId, Guid? licenseId = null, string sort = "", bool isBackOffice = false);
    }
}
