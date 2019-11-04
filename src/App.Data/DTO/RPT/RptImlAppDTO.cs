using System.Collections.Generic;
using App.Data.DTO.APP;
using App.Data.DTO.IML;
using App.Data.DTO.PRL;

namespace App.Data.DTO.RPT
{
    public class RptImlAppDTO: ImlAppDetailDTO
    {
        public List<ReportBranchFullDetailsDTO> Branches { get; set; }

        public List<AppAssigneeListDTO> Assignees { get; set; }

        public List<PrlContractorListDTO> Contractors { get; set; }
    }

    public class RptImlAppAssigneesDTO: ImlAppDetailDTO
    {
        public List<AppAssigneeListDTO> Assignees { get; set; }
    }

    public class RptImlAppContractorsDTO: ImlAppDetailDTO
    {
        public List<PrlContractorListDTO> Contractors { get; set; }
    }
}
