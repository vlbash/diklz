using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.OperationFormList;
using App.Business.Services.RptServices;
using App.Core.Business.Services;
using App.Data.DTO.HelperDTOs;
using App.Data.DTO.IML;
using App.Data.DTO.TRL;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.TRL;
using Newtonsoft.Json;

namespace App.Business.Services.TrlServices
{
    public class TrlLicenseService : ITrlLicenseService
    {
        public ICommonDataService _commonDataService { get; set; }
        private readonly IUserInfoService _infoService;
        private readonly IOperationFormListService _formListService;

        public TrlLicenseService(ICommonDataService commonDataService, IUserInfoService infoService)
        {
            _commonDataService = commonDataService;
            _infoService = infoService;
        }

        public Guid? GetLicenseGuid()
        {
            Guid orgId;
            try
            {
                var user = _infoService.GetCurrentUserInfo();
                orgId = new Guid(user.OrganizationId());
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при обробці данних користувача", e);
            }

            return GetLicenseGuid(orgId);
        }

        public Guid? GetLicenseGuid(Guid? orgId)
        {
            var licenseId = _commonDataService.GetEntity<TrlLicense>(license =>
                license.OrgUnitId == orgId && license.LicType == "TRL" && license.IsRelevant && license.LicState == "Active").ToList();

            switch (licenseId.Count)
            {
                case 0:
                    return null;
                case 1:
                    return licenseId.FirstOrDefault()?.Id;
                default:
                    throw new Exception("Виникла помилка при спробі отримати інформацію про ліцензію");
            }
        }
        public async Task<TrlLicenseDetailDTO> LicenseDetail(Guid id)
        {
            var model = (await _commonDataService.GetDtoAsync<TrlLicenseDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (model == null)
            {
                throw new Exception("Ліцензію не знайдено!");
            }

            if (string.IsNullOrEmpty(model.EDRPOU))
            {
                model.OrgName = model.OrgDirector;
            }
            var branchIds = _commonDataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == model.BaseApplicationId).Select(x => x.BranchId).ToList();
            var branches = _commonDataService.GetEntity<Branch>(x => branchIds.Contains(x.Id)).ToList();
            //var listMedicinalForms = new Dictionary<string, string>();
            //var listActiveIngredients = new Dictionary<string, string>();
            //var listStorageForms = new Dictionary<string, string>();
            //var listProdResearchDrugs = new Dictionary<string, string>();
            //var operationListDTO = _formListService.GetOperationListDTO();
            
            var limsDocs = _commonDataService.GetEntity<LimsDoc>(limsDoc => limsDoc.OrgUnitId == model.OrgUnitId).ToList();

            Guid? limsDocId = null;
            var appIds = new List<Guid>();
            do
            {
                limsDocId = limsDocId ?? model.Id;
                var limsDoc = limsDocs.FirstOrDefault(lic => lic.Id == limsDocId);
                if (limsDoc != null)
                {
                    if (limsDoc.DerivedClass == "TrlLicense")
                    {
                        if (limsDoc.ParentId != null)
                        {
                            limsDocId = limsDoc.ParentId.Value;
                        }
                        else
                            break;
                    }
                    else if (limsDoc.DerivedClass == "TrlApplication")
                    {
                        appIds.Add(limsDoc.Id);
                        if (limsDoc.ParentId != null)
                        {
                            limsDocId = limsDoc.ParentId.Value;
                        }
                        else
                            break;
                    }
                    else
                        throw new Exception("Виникла помилка");
                }
                else
                    throw new Exception("Виникла помилка");

            } while (true);

            model.ApplicationList =
                (await _commonDataService.GetDtoAsync<TrlAppListDTO>(app => appIds.Contains(app.Id)))
                .OrderBy(z => z.OrderDate);

            model.FirstAppId = model.ApplicationList.FirstOrDefault(x => x.AppSortEnum == "GetLicenseApplication")?.Id
                               ?? Guid.Empty;
            return model;
        }
    }
}
