using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using App.Business.Helpers;
using App.Business.ViewModels;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Data.DTO.Common;
using App.Data.Models.APP;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.PRL;
using Newtonsoft.Json;
using Serilog;

namespace App.Business.Services.AppServices
{
    public class ApplicationService<T> where T: BaseApplication
    {
        private readonly ICommonDataService _commonDataService;
        private readonly IObjectMapper _objectMapper;

        public ApplicationService(ICommonDataService commonDataService, IObjectMapper objectMapper)
        {
            _commonDataService = commonDataService;
            _objectMapper = objectMapper;
        }

        public async Task<bool> ChangeCheckBox(string checkBoxId, Guid appId)
        {
            var application = _commonDataService.GetEntity<T>(x => x.Id == appId && x.RecordState != RecordState.D).SingleOrDefault();
            if (application == null)
            {
                return false;
            }

            switch (checkBoxId)
            {
                case "IsCheckMPD":
                    application.IsCheckMpd = !application.IsCheckMpd;
                    break;
                case "IsPaperLicense":
                    application.IsPaperLicense = !application.IsPaperLicense;
                    break;
                case "IsCourierDelivery":
                    application.IsCourierDelivery = !application.IsCourierDelivery;
                    break;
                case "IsPostDelivery":
                    application.IsPostDelivery = !application.IsPostDelivery;
                    break;
                case "IsCourierResults":
                    application.IsCourierResults = !application.IsCourierResults;
                    break;
                case "IsPostResults":
                    application.IsPostResults = !application.IsPostResults;
                    break;
                case "IsElectricFormResults":
                    application.IsElectricFormResults = !application.IsElectricFormResults;
                    break;
                case "IsAgreeLicenseTerms":
                    application.IsAgreeLicenseTerms = !application.IsAgreeLicenseTerms;
                    break;
                case "IsAgreeProcesingData":
                    application.IsAgreeProcessingData = !application.IsAgreeProcessingData;
                    break;
                case "IsProtectionFromAggressors":
                    application.IsProtectionFromAggressors = !application.IsProtectionFromAggressors;
                    break;
            }

            try
            {
                await _commonDataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SaveComment(string text, Guid appId)
        {
            var application = _commonDataService.GetEntity<T>(x => x.Id == appId && x.RecordState != RecordState.D).SingleOrDefault();
            if (application == null)
            {
                return false;
            }

            if (application.Comment == text)
            {
                return true;
            }

            application.ModifiedOn = DateTime.Now;
            application.Comment = text;
            try
            {
                await _commonDataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #region Payment

        public bool ChangePaymentStatus(Guid appId, string status, string comment = null)
        {
            var edoc = _commonDataService.GetEntity<EDocument>(p => p.EDocumentType == "PaymentDocument" && p.EntityId == appId);

            EDocument activeEdoc;
            switch (status)
            {
                case "WaitingForConfirmation":
                    activeEdoc = edoc.SingleOrDefault(p => p.EDocumentStatus == "RequiresPayment");

                    var files = _commonDataService.GetDto<FileStoreDTO>(p => p.EntityId == activeEdoc.Id).ToList();
                    if (files.Count <= 0)
                        return false;
                    break;
                case "PaymentNotVerified":
                case "PaymentConfirmed":
                    activeEdoc = edoc.SingleOrDefault(p => p.EDocumentStatus == "WaitingForConfirmation");
                    break;
                default:
                    return false;
            }

            if (activeEdoc == null)
            {
                return false;
            }

            activeEdoc.EDocumentStatus = status;
            activeEdoc.Comment = comment;
            _commonDataService.SaveChanges();

            return true;
        }

        #endregion

        public async Task<FilesViewModel> GetEDocumentJsonFile(Guid id)
        {
            var applicationBranchesIds = _commonDataService
                .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == id && x.RecordState != RecordState.D)
                .Select(x => x.BranchId)
                .Distinct()
                .ToList();
            var applicationEdocumentBranches =
                _commonDataService.GetEntity<BranchEDocument>(x => applicationBranchesIds.Contains(x.BranchId) && x.RecordState != RecordState.D);
            var applicationEdocumentIds = applicationEdocumentBranches.Select(x => x.EDocumentId).Distinct().ToList();
            var applicationEdocuments = _commonDataService
                .GetEntity<EDocument>(x => applicationEdocumentIds.Contains(x.Id) && x.RecordState != RecordState.D).ToList();
            var listEDocJson = new List<EDocumentMD5Model>();
            foreach (var applicationEdocument in applicationEdocuments)
            {
                var fileStore = _commonDataService
                    .GetEntity<FileStore>(x => x.EntityName == "EDocument" && x.EntityId == applicationEdocument.Id && x.RecordState != RecordState.D)
                    .ToList();
                try
                {
                    foreach (var file in fileStore)
                    {
                        var dto = (await _commonDataService.GetDtoAsync<FileStoreDTO>(x => x.Id == file.Id)).FirstOrDefault();
                        // С новым кором переделать FileStoreDTO
                        var dtoCore = new Core.Data.DTO.Common.FileStoreDTO();
                        _objectMapper.Map(dto, dtoCore);
                        if (FileStoreHelper.LoadFile(dtoCore, out var stream, out var contentType))
                        {
                            var plainTextBytes = Encoding.UTF8.GetBytes(dto?.OrigFileName);
                            var base64Name = Convert.ToBase64String(plainTextBytes);
                            listEDocJson.Add(new EDocumentMD5Model() { Id = file.Id, Name = base64Name, File = FileSignHelper.CalculateMd5(stream) });
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(".igor error");
                    throw e;
                }

            }

            var jsonFile = JsonConvert.SerializeObject(listEDocJson);
            return new FilesViewModel()
            {
                id = Guid.Empty,
                idFileStore = Guid.Empty,
                name = "Підпис_досьє.txt",
                file = jsonFile,
                isSystemFile = true
            };
        }

        
    }
}
