using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.Models.APP;
using App.Data.Models.ORG;

namespace App.Business.Services.AppServices
{
    public class AppAssigneeService
    {
        public ICommonDataService DataService;

        public AppAssigneeService(ICommonDataService dataService)
        {
            DataService = dataService;
        }

        public async Task<AppAssigneeDetailDTO> Edit(Guid? id, IDictionary<string, string> paramList)
        {
            paramList.TryGetValue("appId", out var strAppId);
            paramList.TryGetValue("AppSort", out var sort);
            paramList.TryGetValue("AppType", out var type);

            var appId = new Guid(strAppId);
            AppAssigneeDetailDTO model;

            if (id == null)
            {
                model = new AppAssigneeDetailDTO();
                if (type == "PRL" || type == "IML")
                    model.OrgPositionType = "Authorized";
                else if (type == "TRL")
                    model.AppType = type;
            }
            else
            {
                var entityId = Guid.Parse(id.ToString());
                model = DataService.GetDto<AppAssigneeDetailDTO>(dto => dto.Id == entityId).Single();
                if (type == "TRL")
                    model.AppType = type;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                model.AppSort = sort;
            }

            
            if (model.OrgPositionType == "Manager")
                model.BranchId = GetSelectedBranches(model.Id).FirstOrDefault();
            else
                model.ListOfBranches = GetSelectedBranches(model.Id).ToList();

            model.appId = appId;
            return model;
        }

        public AppAssigneeDetailDTO Edit(AppAssigneeDetailDTO model)
        {
            if (model.AppType == "TRL")
            {
                if (model.OrgPositionType == "Manager")
                    model.ListOfBranches = null;
                else
                    model.BranchId = Guid.Empty;
            }
            model.Id = DataService.Add<AppAssignee>(model);

            DataService.SaveChanges();

            var assigneeBranches = DataService.GetEntity<AppAssigneeBranch>(x => x.AssigneeId == model.Id);
            foreach (var x in assigneeBranches)
            {
                DataService.Remove(x);
            }

            if (model.AppType == "TRL")
            {
                if (model.OrgPositionType == "Manager")
                {
                    DataService.Add(new AppAssigneeBranch
                    {
                        AssigneeId = model.Id,
                        BranchId = model.BranchId.Value
                    });
                }
                else
                {
                    model.ListOfBranches?.ForEach(branchId => DataService.Add(new AppAssigneeBranch
                    {
                        AssigneeId = model.Id,
                        BranchId = branchId
                    }));
                }
            }
            else
            {
                model.ListOfBranches?.ForEach(branchId => DataService.Add(new AppAssigneeBranch
                {
                    AssigneeId = model.Id,
                    BranchId = branchId
                }));
            }
            
            DataService.SaveChanges();

            return model;
        }

        public async Task<AppAssigneeDetailDTO> EditLicense(AppAssigneeDetailDTO model)
        {
            var assigneeBranches = DataService.GetEntity<AppAssigneeBranch>(x => x.AssigneeId == model.Id).ToList();
            var branches = DataService.GetEntity<Branch>(x => assigneeBranches.Select(y => y.BranchId).Contains(x.Id)).ToList();
            var branchesToDelete = branches.Where(x => x.IsFromLicense != true).ToList();
            assigneeBranches.Where(x => branchesToDelete.Select(y => y.Id).Contains(x.BranchId)).ToList().ForEach(x => DataService.Remove(x));
            model.ListOfBranches?.ForEach(branchId => DataService.Add(new AppAssigneeBranch
            {
                AssigneeId = model.Id,
                BranchId = branchId
            }));

            await DataService.SaveChangesAsync();

            return model;
        }

        public async Task<IEnumerable<AppAssigneeListDTO>> GetAssigneeList(Guid? appId)
        {
            var appAssigneeList =
                await DataService.GetDtoAsync<AppAssigneeListDTO>(extraParameters: new object[] { $"\'{appId}\'" });
            var assigneeIds = appAssigneeList.Select(x => x.Id);

            var branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId);
            var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => assigneeIds.Contains(x.AssigneeId));

            foreach (var assg in appAssigneeList)
            {
                var branchAssigneeIds = branchAssignee.Where(x => x.AssigneeId == assg.Id).Select(x => x.BranchId);
                assg.ListOfBranches = new List<BranchListDTO>(branchList.Where(br => branchAssigneeIds.Contains(br.Id)).ToList());
            }

            return appAssigneeList;
        }


        public IEnumerable<Guid> GetSelectedBranches(Guid assigneId)
        {
            return DataService.GetEntity<AppAssigneeBranch>(x => x.AssigneeId == assigneId).Select(x => x.BranchId);
        }

        public async Task<string> GetAppType(Guid? appId)
        {
            return (await DataService.GetDtoAsync<AppStateDTO>(x => x.Id == appId)).FirstOrDefault()?.AppType;
            
        }
    }
}
