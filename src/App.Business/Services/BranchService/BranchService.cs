using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.LimsService;
using App.Business.Services.OperationFormList;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.DTO.HelperDTOs;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using Newtonsoft.Json;

namespace App.Business.Services.BranchService
{
    public class BranchService: IBranchService
    {
        private readonly ICommonDataService _dataService;
        private readonly IOperationFormListService _formListService;
        private readonly LimsExchangeService _limsExchangeService;

        public BranchService(ICommonDataService dataService, IOperationFormListService formListService, LimsExchangeService limsExchangeService)
        {
            _dataService = dataService;
            _formListService = formListService;
            _limsExchangeService = limsExchangeService;
        }

        public async Task<Guid> Save(BranchDetailsDTO model)
        {
            var check = model.Id == Guid.Empty;

            if (check)
                model.IsFromLicense = false;

            var branchId = _dataService.Add<Branch>(model);
            await _dataService.SaveChangesAsync();

            if (check)
            {
                var applicationBranch = new ApplicationBranch
                {
                    LimsDocumentId = model.ApplicationId,
                    BranchId = branchId
                };
                _dataService.Add(applicationBranch);
                await _dataService.SaveChangesAsync();

                if (model.AppType == "TRL")
                {
                    //Вид діяльності обраний в МПД
                    var trlActivityTypeEnum = _dataService.GetEntity<EnumRecord>(x => x.Code == model.TrlActivityType)?.FirstOrDefault();
                    _dataService.Add(new EntityEnumRecords()
                    {
                        EntityId = branchId,
                        EntityType = "BranchApplication",
                        EnumRecordType = trlActivityTypeEnum?.EnumType,
                        EnumRecordCode = trlActivityTypeEnum?.Code
                    });

                    if (model.PharmacyId != null)
                    {
                        _dataService.Add(new PharmacyItemPharmacy()
                        {
                            PharmacyId = model.PharmacyId.Value,
                            PharmacyItemId = branchId
                        });
                    }

                    await _dataService.SaveChangesAsync();
                }
            }
            else
            {
                if (model.AppType == "TRL")
                {
                    var pharmacyItem = _dataService.GetEntity<PharmacyItemPharmacy>(x => x.PharmacyItemId == branchId)?.FirstOrDefault();
                    if (pharmacyItem != null)
                        _dataService.Remove(pharmacyItem);

                    if (model.PharmacyId != null)
                    {
                        _dataService.Add(new PharmacyItemPharmacy()
                        {
                            PharmacyId = model.PharmacyId.Value,
                            PharmacyItemId = branchId
                        });
                    }

                    var entityEnumRecords =
                        _dataService.GetEntity<EntityEnumRecords>(x => x.EntityId == branchId).ToList();

                    if (entityEnumRecords.Count > 0)
                    {
                        entityEnumRecords.ForEach(x => _dataService.Remove(x));
                    }

                    //Види діяльності обрані в МПД
                    var trlActivityTypeEnum = _dataService.GetEntity<EnumRecord>(x => x.Code == model.TrlActivityType)?.FirstOrDefault();
                    _dataService.Add(new EntityEnumRecords()
                    {
                        EntityId = branchId,
                        EntityType = "BranchApplication",
                        EnumRecordType = trlActivityTypeEnum?.EnumType,
                        EnumRecordCode = trlActivityTypeEnum?.Code
                    });

                    await _dataService.SaveChangesAsync();
                }
            }
            return branchId;
        }

        public bool SaveChangingInfoAsync(Guid id, string _string)
        {
            var branch = _dataService.GetEntity<Branch>(x => x.Id == id).SingleOrDefault();
            if (branch == null)
                return false;
            branch.OperationListFormChanging = _string;
            _dataService.SaveChanges();
            return true;
        }

        public async Task Delete(Guid id)
        {
            var branch = _dataService.GetEntity<Branch>(x => x.Id == id).SingleOrDefault();
            branch.RecordState = RecordState.D;
            var branchContractors = _dataService.GetEntity<PrlBranchContractor>(x => x.BranchId == id).ToList();
            var contractors = _dataService
                .GetEntity<PrlContractor>(x => branchContractors.Select(y => y.ContractorId).Contains(x.Id)).ToList();
            contractors.ForEach(contr =>
                {
                    var contrBranches = _dataService.GetEntity<PrlBranchContractor>(x => x.ContractorId == contr.Id).ToList();
                    if (contrBranches.All(z => z.BranchId == id))
                    {
                        contr.RecordState = RecordState.D;
                        contrBranches.ForEach(x => x.RecordState = RecordState.D);
                    }
                }
            );
            var branchAssignees = _dataService.GetEntity<AppAssigneeBranch>(x => x.BranchId == id).ToList();
            var assignees = _dataService
                .GetEntity<AppAssignee>(x => branchAssignees.Select(y => y.AssigneeId).Contains(x.Id)).ToList();
            assignees.ForEach(assignee =>
                {
                    var assigneeBranches = _dataService.GetEntity<AppAssigneeBranch>(x => x.AssigneeId == assignee.Id).ToList();
                    if (assigneeBranches.All(z => z.BranchId == id))
                    {
                        assignee.RecordState = RecordState.D;
                        assigneeBranches.ForEach(x => x.RecordState = RecordState.D);
                    }
                }
            );
            var branchEDocuments = _dataService.GetEntity<BranchEDocument>(x => x.BranchId == id).ToList();
            var eDocuments = _dataService
                .GetEntity<EDocument>(x => branchEDocuments.Select(y => y.EDocumentId).Contains(x.Id)).ToList();
            eDocuments.ForEach(eDoc =>
                {
                    var eDocBranches = _dataService.GetEntity<BranchEDocument>(x => x.EDocumentId == eDoc.Id).ToList();
                    if (eDocBranches.All(z => z.BranchId == id))
                    {
                        eDoc.RecordState = RecordState.D;
                        var files = _dataService.GetEntity<FileStore>(x =>
                            x.EntityId == eDoc.Id && x.EntityName == "EDocument").ToList();
                        files.ForEach(file =>
                        {
                            file.RecordState = RecordState.D;
                            FileStoreHelper.DeleteFileIfExist(file.FilePath);
                        });
                        eDocBranches.ForEach(x => x.RecordState = RecordState.D);
                    }
                }
            );
            await _dataService.SaveChangesAsync();
        }

        public async Task UpdateBranch(Guid appId)
        {
            var branchIdList = _dataService.GetDto<BranchListDTO>(x => x.ApplicationId == appId).Select(x => x.Id);
            var branchList = _dataService.GetEntity<Branch>(x => branchIdList.Contains(x.Id) && x.CreateDls == false && x.RecordState != RecordState.D).ToList();

            if (branchList.Any())
            {
                branchList.ForEach(x => x.CreateTds = true);
                try
                {
                    await _limsExchangeService.CreateTdsAsync(appId);
                }
                catch (Exception e)
                {
                    branchList.ForEach(x => x.CreateTds = false);
                }

                await _dataService.SaveChangesAsync();
            }
        }

        public async Task UpdateBranch(List<Guid> branchIdList)
        {
            var branchList = _dataService.GetEntity<Branch>(x => branchIdList.Contains(x.Id) && x.CreateTds == false).ToList();

            if (branchList.Any())
            {
                branchList.ForEach(x => x.CreateDls = true);
                try
                {
                    await _limsExchangeService.CreateDlsAsync(branchIdList);
                }
                catch (Exception e)
                {
                    branchList.ForEach(x => x.CreateTds = false);
                }

                await _dataService.SaveChangesAsync();
            }
        }

        public string GetOperationList(string operationListJson)
        {
            var operationListDTO = _formListService.GetOperationListDTO();
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(operationListJson);

            var MPDListText = "";

            if (list != null)
            {
                foreach (var root in list)
                {
                    foreach (var i in root)
                    {
                        if (i.Key == "id")
                        {
                            var result = Find(operationListDTO.FirstLevels, i.Value);
                            if (result != null)
                            {
                                MPDListText += result.Code + " " + result.Name + "</br>";
                            }
                        }
                        else
                        {
                            if (i.Value != "true" && i.Value != "false")
                            {
                                MPDListText += "\t" + i.Value + "</br>";
                            }
                        }
                    }
                }
            }

            return MPDListText;
        }

        public static Item Find(List<Item> node, string code)
        {
            foreach (var i in node)
            {
                if (i == null)
                {
                    return null;
                }

                if (i.Code == code)
                {
                    return i;
                }

                if (i.ChildItems != null)
                {
                    foreach (var child in i.ChildItems)
                    {
                        var found = Find(i.ChildItems, code);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }

            return null;
        }

        public List<BranchListDTO> GetPharmacyList(Guid? id, Guid? appId)
        {
            List<BranchListDTO> pharmacyList;

            var license = _dataService.GetDto<CurrentLicenseDTO>(x => x.ApplicationId == appId && x.IsRelevant && x.LicType == "TRL" && x.LicState == "Active").FirstOrDefault();
            if (license != null)
            {
                if (id != null)
                    pharmacyList = _dataService.GetDto<BranchListDTO>(x => (x.ApplicationId == appId || x.ApplicationId == license.LicenseParentId) && x.BranchType == "Pharmacy" && x.Id != id.Value).ToList();
                else
                    pharmacyList = _dataService.GetDto<BranchListDTO>(x => (x.ApplicationId == appId || x.ApplicationId == license.LicenseParentId) && x.BranchType == "Pharmacy").ToList();
            }
            else
            {
                if (id != null)
                    pharmacyList = _dataService.GetDto<BranchListDTO>(x => x.ApplicationId == appId && x.BranchType == "Pharmacy" && x.Id != id.Value).ToList();
                else
                    pharmacyList = _dataService.GetDto<BranchListDTO>(x => x.ApplicationId == appId && x.BranchType == "Pharmacy").ToList();
            }

            return pharmacyList;
        }

        public IEnumerable<EnumRecord> GetAppActivityTypeList(string sort, Guid? appId)
        {
            IEnumerable<EnumRecord> appActivityTypeList;

            if (sort == "AddBranchApplication")
            {
                var license = _dataService.GetDto<CurrentLicenseDTO>(x => x.ApplicationId == appId && x.IsRelevant && x.LicType == "TRL" && x.LicState == "Active").FirstOrDefault();
                var activityTypeList = _dataService.GetDto<EntityEnumDTO>(x => x.ApplicationId == license.LicenseParentId)
                    .Select(x => x.EnumCode).ToList();
                appActivityTypeList = _dataService.GetEntity<EnumRecord>(x => activityTypeList.Contains(x.Code));
            }
            else
            {
                var activityTypeList = _dataService.GetDto<EntityEnumDTO>(x => x.ApplicationId == appId)
                    .Select(x => x.EnumCode).ToList();
                appActivityTypeList = _dataService.GetEntity<EnumRecord>(x => activityTypeList.Contains(x.Code)).ToList();
            }

            return appActivityTypeList;
        }
    }
}

