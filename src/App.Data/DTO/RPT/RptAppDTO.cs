using System.Collections.Generic;
using App.Data.DTO.APP;
using App.Data.DTO.PRL;

namespace App.Data.DTO.RPT
{
    public class RptPrlAppDTO: PrlAppDetailDTO
    {
        public List<ReportBranchFullDetailsDTO> Branches { get; set; }

        public List<AppAssigneeListDTO> Assignees { get; set; }

        public List<PrlContractorListDTO> Contractors { get; set; }
    }

    public class RptPrlAppAssigneesDTO: PrlAppDetailDTO
    {
        public List<AppAssigneeListDTO> Assignees { get; set; }
    }

    public class RptPrlAppContractorsDTO: PrlAppDetailDTO
    {
        public List<PrlContractorListDTO> Contractors { get; set; }
    }
}
