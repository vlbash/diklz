using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Data.Entities.Common;
using App.Data.DTO.BRN;

namespace App.Business.Services.BranchService
{
    public interface IBranchService
    {
        bool SaveChangingInfoAsync(Guid id, string _string);

        Task<Guid> Save(BranchDetailsDTO model);

        Task Delete(Guid id);

        Task UpdateBranch(Guid appId);

        Task UpdateBranch(List<Guid> branchIdList);

        string GetOperationList(string operationListJson);

        List<BranchListDTO> GetPharmacyList(Guid? id, Guid? appId);

        IEnumerable<EnumRecord> GetAppActivityTypeList(string sort, Guid? appId);
    }
}
