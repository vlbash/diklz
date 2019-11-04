using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Data.DTO.TRL;
using App.Data.Models.ORG;

namespace App.Business.Services.TrlServices
{
    public interface ITrlOrganizationService
    {
        ICommonDataService DataService { get; }
        Task<Dictionary<string, string>> SaveNewOrg(TrlOrganizationExtFullDTO arg);
    }

    public class TrlOrganizationService: ITrlOrganizationService
    {
        public ICommonDataService DataService { get; }
        private readonly IObjectMapper _objectMapper;

        public TrlOrganizationService(ICommonDataService dataService, IObjectMapper objectMapper)
        {
            DataService = dataService;
            _objectMapper = objectMapper;
        }

        public async Task<Dictionary<string, string>> SaveNewOrg(TrlOrganizationExtFullDTO orgDto)
        {
            var errorList = new Dictionary<string, string>();

            if (orgDto.Id != Guid.Empty)
            {
                errorList.Add("", "Неможливо змінити інформацію про існуючу організацію");
                return errorList;
            }

            bool isSameEntity;

            {
                var isFopEntity = false;
                var isOrgEntity = false;

                if (!string.IsNullOrWhiteSpace(orgDto.EDRPOU))
                {
                    var sameOrg = DataService.GetEntity<OrganizationExt>(ext =>
                        ext.EDRPOU != null &&
                        orgDto.EDRPOU.Equals(ext.EDRPOU, StringComparison.InvariantCultureIgnoreCase));
                    isOrgEntity = sameOrg.Any();
                }

                if (!string.IsNullOrWhiteSpace(orgDto.INN))
                {
                    var sameFop = DataService.GetEntity<OrganizationExt>(ext =>
                        ext.INN != null &&
                        orgDto.INN.Equals(ext.INN, StringComparison.InvariantCultureIgnoreCase));
                    isFopEntity = sameFop.Any();
                }

                isSameEntity = isFopEntity || isOrgEntity;
            }

            if (isSameEntity)
            {
                errorList.Add("", "Організація з таким ІНН\\ЄДРПОУ вже існує");
                return errorList;
            }

            var orgGuid = DataService.Add<OrganizationExt>(orgDto, false);

            orgDto.Id = orgGuid;
            var orgInfo = _objectMapper.Map<OrganizationInfo>(orgDto);
            orgInfo.OrganizationId = orgGuid;
            orgInfo.Type = "TRL";
            orgInfo.IsActualInfo = true;
            DataService.Add(orgInfo, false);

            await DataService.SaveChangesAsync();

            return errorList;
        }
    }
}
