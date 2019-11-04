using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Data.DTO.IML;
using App.Data.DTO.P902;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;
using App.Data.Models.IML;
using App.Data.Models.P902;
using Microsoft.AspNetCore.Mvc;

namespace App.Business.Services.P902Services
{
    public class ResultInputControlService
    {
        public readonly ICommonDataService DataService;
        private readonly IUserInfoService _userInfoService;
        private readonly LimsExchangeService _exchangeService;

        public ResultInputControlService(ICommonDataService commonDataService, IUserInfoService userInfoService, LimsExchangeService exchangeService)
        {
            DataService = commonDataService;
            _userInfoService = userInfoService;
            _exchangeService = exchangeService;
        }

        public async Task<ResultInputControlDetailsDTO> GetEditFunction(Guid? id, IDictionary<string, string> paramList)
        {
            ResultInputControlDetailsDTO model;
            if (id == null)
            {
                var licenseIml = (await DataService.GetDtoAsync<ImlLicenseDetailDTO>(x =>
                    x.OrgUnitId == new Guid(_userInfoService.GetCurrentUserInfo().OrganizationId()) && x.LicState == "Active" && x.IsRelevant == true)).FirstOrDefault();
                if (licenseIml != null)
                {
                    model = new ResultInputControlDetailsDTO
                    {
                        State = "Project",
                        LicenseId = licenseIml.Id,
                        LicenseString = licenseIml.LicenseNumber,
                        DistrictName = licenseIml.DistrictName,
                        CityName = licenseIml.CityName,
                        CityEnum = licenseIml.CityEnum,
                        AddressId = licenseIml.AddressId,
                        AddressType = licenseIml.AddressType,
                        RegionName = licenseIml.RegionName,
                        StreetName = licenseIml.StreetName,
                        Building = licenseIml.Building,
                        Edrpou = licenseIml.EDRPOU,
                        OrgName = licenseIml.OrgName,
                        OrgUnitId = licenseIml.OrgUnitId
                    };
                    return model;
                }

                var licensePrl = (await DataService.GetDtoAsync<PrlLicenseDetailDTO>(x =>
                    x.OrgUnitId == new Guid(_userInfoService.GetCurrentUserInfo().OrganizationId()) && x.LicState == "Active" && x.IsRelevant == true)).FirstOrDefault();
                if (licensePrl != null)
                {
                    model = new ResultInputControlDetailsDTO
                    {
                        State = "Project",
                        LicenseId = licensePrl.Id,
                        LicenseString = licensePrl.LicenseNumber,
                        DistrictName = licensePrl.DistrictName,
                        CityName = licensePrl.CityName,
                        CityEnum = licensePrl.CityEnum,
                        AddressId = licensePrl.AddressId,
                        AddressType = licensePrl.AddressType,
                        RegionName = licensePrl.RegionName,
                        StreetName = licensePrl.StreetName,
                        Building = licensePrl.Building,
                        Edrpou = licensePrl.EDRPOU,
                        OrgName = licensePrl.OrgName,
                        OrgUnitId = licensePrl.OrgUnitId
                    };
                    return model;
                }

                var licenseTrl = (await DataService.GetDtoAsync<TrlLicenseDetailDTO>(x =>
                    x.OrgUnitId == new Guid(_userInfoService.GetCurrentUserInfo().OrganizationId()) && x.LicState == "Active" && x.IsRelevant == true)).FirstOrDefault();
                if (licenseTrl != null)
                {
                    model = new ResultInputControlDetailsDTO
                    {
                        State = "Project",
                        LicenseId = licenseTrl.Id,
                        LicenseString = licenseTrl.LicenseNumber,
                        DistrictName = licenseTrl.DistrictName,
                        CityName = licenseTrl.CityName,
                        CityEnum = licenseTrl.CityEnum,
                        AddressId = licenseTrl.AddressId,
                        AddressType = licenseTrl.AddressType,
                        RegionName = licenseTrl.RegionName,
                        StreetName = licenseTrl.StreetName,
                        Building = licenseTrl.Building,
                        Edrpou = licenseTrl.EDRPOU,
                        OrgName = licenseTrl.OrgName,
                        OrgUnitId = licenseTrl.OrgUnitId
                    };
                    return model;
                }
                throw new InvalidOperationException("User don't have license");
            }
            else
            {
                return (await DataService.GetDtoAsync<ResultInputControlDetailsDTO>(x => x.Id == id.Value)).FirstOrDefault();
            }
        }

        public async Task<bool> ChangeCheck(Guid sendId)
        {
            var drug = DataService.GetEntity<ResultInputControl>(x => x.Id == sendId).FirstOrDefault();
            if (drug == null)
            {
                return false;
            }
            try
            {
                drug.SendCheck = !drug.SendCheck;
                await DataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateStatus()
        {
            try
            {
                await _exchangeService.UpdateRepDrugStatus();
                var drugSend = DataService.GetEntity<ResultInputControl>(x => x.SendCheck == true && x.State == "Project" /*&& x.OldLimsId != 0*/).ToList();
                if (drugSend.Any())
                {
                    foreach (var item in drugSend)
                    {
                        item.State = "Sent";
                        await DataService.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        #region P902_LimsUpdate

        public async Task InsertSgdRepDrugList(ResultInputControl resultInputControl)
        {
           await _exchangeService.InsertSgdRepDrugList(resultInputControl);
        }

        #endregion
    }
}
