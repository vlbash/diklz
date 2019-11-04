using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.BRN;
using App.Data.DTO.PRL;
using App.Data.Models.PRL;

namespace App.Business.Services.PrlServices
{
    public class PrlContractorService
    {
        public ICommonDataService DataService;

        public PrlContractorService(ICommonDataService dataService)
        {
            DataService = dataService;
        }

        public async Task<IEnumerable<PrlContractorListDTO>> GetAssigneeList(Guid? appId)
        {
            var contractorList =
                (await DataService.GetDtoAsync<PrlContractorListDTO>(extraParameters: new object[] {$"\'{appId}\'"}))
                .ToList();
            var contractorsIds = contractorList.Select(x => x.Id);

            var branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId);
            var branchAssignee = DataService
                .GetEntity<PrlBranchContractor>(x => contractorsIds.Contains(x.ContractorId))
                .ToList();

            foreach (var contr in contractorList)
            {
                contr.ListOfBranchsNames = new List<string>();

                var branchAssigneeIds = branchAssignee.Where(x => x.ContractorId == contr.Id).Select(x => x.BranchId);
                var branchNamesList = branchList.Where(br => branchAssigneeIds.Contains(br.Id))
                    .Select(st => st.Name + ',' + st.PhoneNumber);
                contr.ListOfBranchsNames.AddRange(branchNamesList);
            }

            return contractorList;
        }
    }
}
