using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Business.Services.ImlServices;
using App.Business.Services.OperationFormList;
using App.Business.Services.RptServices;
using App.Core.Business.Services;
using App.Data.DTO.HelperDTOs;
using App.Data.DTO.PRL;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using Newtonsoft.Json;

namespace App.Business.Services.PrlServices
{
    public class PrlLicenseService: IPrlLicenseService
    {
        public ICommonDataService _commonDataService { get; set; }
        private readonly IUserInfoService _infoService;
        private readonly IOperationFormListService _formListService;

        public PrlLicenseService(ICommonDataService commonDataService, IUserInfoService infoService, IOperationFormListService formListService)
        {
            _commonDataService = commonDataService;
            _infoService = infoService;
            _formListService = formListService;
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
            var licenseId = _commonDataService.GetEntity<PrlLicense>(license =>
                license.OrgUnitId == orgId && license.LicType == "PRL" && license.IsRelevant && license.LicState == "Active").ToList();

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

        public async Task<PrlLicenseDetailDTO> LicenseDetail(Guid id)
        {
            var model = (await _commonDataService.GetDtoAsync<PrlLicenseDetailDTO>(x => x.Id == id)).FirstOrDefault();
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
            var listMedicinalForms = new Dictionary<string, string>();
            var listActiveIngredients = new Dictionary<string, string>();
            var listStorageForms = new Dictionary<string, string>();
            var listProdResearchDrugs = new Dictionary<string, string>();
            var operationListDTO = _formListService.GetOperationListDTO();
            foreach (var branch in branches)
            {
                var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListForm);
                var result = new Item();
                if (list != null)
                    foreach (var root in list)
                    {
                        foreach (var i in root)
                        {
                            if (i.Key == "id")
                            {
                                result = PrlReportService.Find(operationListDTO.FirstLevels, i.Value);
                                if (result != null)
                                {
                                    if (result.Code.StartsWith('1'))
                                        listMedicinalForms.TryAdd(result.Code, result.Name);
                                    if (result.Code.StartsWith('2'))
                                        listActiveIngredients.TryAdd(result.Code, result.Name);
                                    if (result.Code.StartsWith('3'))
                                        listStorageForms.TryAdd(result.Code, result.Name);
                                    if (result.Code.StartsWith('4'))
                                        listProdResearchDrugs.TryAdd(result.Code, result.Name);
                                }
                            }
                            else
                            {
                                if (i.Value != "true" && i.Value != "false")
                                {
                                    if (i.Key.StartsWith('1'))
                                        listMedicinalForms.Add(i.Key, i.Value);
                                    if (i.Key.StartsWith('2'))
                                        listActiveIngredients.TryAdd(i.Key, i.Value);
                                    if (i.Key.StartsWith('3'))
                                        listStorageForms.TryAdd(i.Key, i.Value);
                                    if (i.Key.StartsWith('4'))
                                        listProdResearchDrugs.TryAdd(i.Key, i.Value);
                                }
                            }
                        }
                    }
            }

            foreach (var form in listMedicinalForms)
                model.MedicinalForms += $"{form.Key} {form.Value}</br>";
            foreach (var form in listStorageForms)
                model.StorageForms += $"{form.Key} {form.Value}</br>";
            foreach (var form in listActiveIngredients)
                model.ActiveIngredients += $"{form.Key} {form.Value}</br>";
            foreach (var form in listProdResearchDrugs)
                model.ProdResearchDrugs += $"{form.Key} {form.Value}</br>";



            var limsDocs = _commonDataService.GetEntity<LimsDoc>(limsDoc => limsDoc.OrgUnitId == model.OrgUnitId).ToList();

            Guid? limsDocId = null;
            var appIds = new List<Guid>();
            do
            {
                limsDocId = limsDocId ?? model.Id;
                var limsDoc = limsDocs.FirstOrDefault(lic => lic.Id == limsDocId);
                if (limsDoc != null)
                {
                    if (limsDoc.DerivedClass == "PrlLicense")
                    {
                        if (limsDoc.ParentId != null)
                        {
                            limsDocId = limsDoc.ParentId.Value;
                        }
                        else
                            break;
                    }
                    else if (limsDoc.DerivedClass == "PrlApplication")
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
                (await _commonDataService.GetDtoAsync<PrlAppListDTO>(app => appIds.Contains(app.Id)))
                .OrderBy(z => z.OrderDate);

            model.FirstAppId = model.ApplicationList.FirstOrDefault(x => x.AppSortEnum == "GetLicenseApplication")?.Id 
                               ?? Guid.Empty;
            return model;
        }

    }
}
