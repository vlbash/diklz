using System;
using System.Threading.Tasks;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Data.DTO.PRL;
using Microsoft.Extensions.Configuration;

namespace App.Business.Services.PrlServices
{
    public interface IPrlApplicationService
    {
        ICommonDataService DataService { get; }
        Task<Guid> SaveApplication(PrlAppDetailDTO model, bool isBackOffice = false);
        Task<FilesSignViewModel> GetFilesForSign(Guid id);
        Task SubmitApplication(IConfiguration configuration, FilesSignViewModel model);
        Task SubmitBackOfficeApplication(IConfiguration config, Guid appId);
        Task BackCreateApplication(PrlAppDetailDTO model, Guid? orgId, string appSort);
        Task<byte[]> GetApplicationFile(Guid appId, string sort);
        Task SubmitAdditionalInfoToLicense(Guid appId, string text);
        Task<FilesSignViewModel> GetFilesForVerification(Guid appId);
    }
}
