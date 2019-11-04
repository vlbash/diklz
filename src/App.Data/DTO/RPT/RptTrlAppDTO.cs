using System.Collections.Generic;
using App.Data.DTO.APP;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;

namespace App.Data.DTO.RPT
{
    public class RptTrlAppDTO: TrlAppDetailDTO
    {
        public List<ReportBranchFullDetailsDTO> Branches { get; set; }

        public List<AppAssigneeListDTO> Assignees { get; set; }

        public List<PrlContractorListDTO> Contractors { get; set; }
    }

    public class RptTrlAppAssigneesDTO: TrlAppDetailDTO
    {
        public List<AppAssigneeListDTO> Assignees { get; set; }
    }

    public class RptTrlAppContractorsDTO: TrlAppDetailDTO
    {
    public List<PrlContractorListDTO> Contractors { get; set; }
    }
}
