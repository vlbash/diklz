using System.Collections.Generic;
using App.Core.Business.Services;
using App.Core.Data.DTO.Common;

namespace App.Core.Business
{
    public class ExtendedReport<TPredicateDTO, TResultDTO>: BaseReport<TResultDTO>
        where TPredicateDTO : BaseDTO
        where TResultDTO : class
    {
        protected IDTOService<TPredicateDTO> DtoPredicateService;

        public ExtendedReport(IDTOService<TPredicateDTO> dtoPredicateService,
                              IDTOService<TResultDTO> dtoResultService) : base(dtoResultService)
        {
            DtoPredicateService = dtoPredicateService;
        }

        public override void Generate(IDictionary<string, string> paramList)
        {
            var nestedSelect = DtoPredicateService.DTORepository.GetParameterizedQueryString(paramList);
            ReportData = DtoService.DTORepository.GetDTO(null, nestedSelect);
        }
    }
}
