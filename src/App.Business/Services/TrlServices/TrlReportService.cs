﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Business.Services.ImlServices;
using App.Business.Services.OperationFormList;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.DTO.PRL;
using App.Data.DTO.RPT;
using App.Data.DTO.TRL;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.ORG;

namespace App.Business.Services.TrlServices
{
    public class TrlReportService
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _userInfoService;
        private readonly IObjectMapper _objectMapper;
        private readonly IImlLicenseService _licservice;
        private readonly string path;

        public TrlReportService(ICommonDataService dataService, IUserInfoService userInfoService,
            IObjectMapper objectMapper, IImlLicenseService licservice)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _objectMapper = objectMapper;
            _licservice = licservice;
            path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }


        //public async Task<string> TrlCreateLicenseApp(Guid id)
        //{
        //    // Генерація pdf для TRL

        //    var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();

        //    var branches = await _dataService.GetDtoAsync<BranchDetailsDTO>(x => x.ApplicationId == id);
        //    var assigneeBranches =
        //        _dataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).Select(x => x.AssigneeId);
        //    var assignees = await _dataService.GetDtoAsync<AppAssigneeDetailDTO>(x => assigneeBranches.Contains(x.Id));
        //    if (app == null)
        //    {
        //        throw new Exception("No application");
        //    }

        //    //var trlActivityType = _dataService.GetEntity<EntityEnumRecords>(x => x.EntityId == id).Select(x => x.EnumRecordCode);
        //    //var enumR = _dataService.GetEntity<EnumRecord>(x => x.EnumType == "TrlActivityType" && trlActivityType.Contains(x.Code)).ToList();
        //    var a = _dataService.GetDto<EntityEnumDTO>(x => x.ApplicationId == id).ToList();

        //    var appObject = _objectMapper.Map<RptTrlAppDTO>(app);

        //    var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

        //    var employeeList = "";
        //    var mpdList = "";

        //    var pathHtml = string.IsNullOrEmpty(app.EDRPOU)
        //        ? "Templates/Htmls/TRL/Htmls/CreateLicenseApp/PDFTemplate_CreateLicenseAppTRL_FOP.html"
        //        : "Templates/Htmls/TRL/Htmls/CreateLicenseApp/PDFTemplate_CreateLicenseAppTRL_ORG.html";
        //    var emailPath = Path.Combine(path, pathHtml);
        //    var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

        //    var mpdPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_MPD.html");

        //    var empPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_AutPersonList.html");

        //    foreach (var employee in assignees)
        //    {
        //        var empFile = new StringBuilder(File.ReadAllText(empPath));

        //        empFile.Replace("@@AssigneTypeName@@", employee.AssigneTypeName);
        //        empFile.Replace("@@AutPersonPosition@@", employee.NameOfPosition);
        //        empFile.Replace("@@AutPersonSurname@@", employee.LastName);
        //        empFile.Replace("@@AutPersonName@@", employee.Name);
        //        empFile.Replace("@@AutPersonMiddleName@@", employee.MiddleName);
        //        empFile.Replace("@@AutPersonEducation@@", employee.EducationInstitution);
        //        empFile.Replace("@@AutPersonExperience@@", employee.WorkExperience);

        //        employeeList += empFile.ToString();
        //    }

        //    foreach (var branch in branches)
        //    {
        //        var mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

        //        mpdFile.Replace("@@MPDName@@", branch.Name);

        //        mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
        //        mpdFile.Replace("@@AddressEng@@", branch.AdressEng);
        //        mpdFile.Replace("@@PhoneNumber@@", StandartPhone(branch.PhoneNumber));
        //        mpdFile.Replace("@@FaxNumber@@", StandartPhone(branch.FaxNumber));
        //        mpdFile.Replace("@@E-mail@@", branch.EMail);
        //        mpdFile.Replace("@@ListOfTrlActivityTypeName@@", branch.EMail);
        //        mpdFile.Replace("@@BranchTypeName@@", branch.EMail);
        //        mpdFile.Replace("@@SpecialConditions@@", branch.SpecialConditions);
        //        mpdFile.Replace("@@AsepticConditionsName@@", branch.AsepticConditions);

        //        mpdList += mpdFile.ToString();
        //    }

        //    htmlFile.Replace("@@AutPersonList@@", employeeList);
        //    htmlFile.Replace("@@MPD@@", mpdList);
        //    htmlFile.Replace("@@OrgName@@", appObject.OrgName);
        //    htmlFile.Replace("@@AddressString@@", appObject.PostIndex + ", " + appObject.Address);
        //    htmlFile.Replace("@@OrgDirector@@", appObject.OrgDirector);
        //    htmlFile.Replace("@@LegalFormTypeName@@", appObject.LegalFormName);
        //    htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(appObject.EDRPOU) ? "</br>" : appObject.EDRPOU);
        //    htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));
        //    htmlFile.Replace("@@FaxNumber@@", StandartPhone(appObject.FaxNumber));
        //    htmlFile.Replace("@@EMail@@", appObject.EMail);
        //    htmlFile.Replace("@@EconomicClassificationTypeName@@", appObject.EconomicClassificationTypeName);
        //    htmlFile.Replace("@@ActivityTypeName@@", appObject.ActivityTypeName);
        //    htmlFile.Replace("@@NationalAccount@@", appObject.NationalAccount);
        //    htmlFile.Replace("@@NationalBankRequisites@@", appObject.NationalBankRequisites);
        //    htmlFile.Replace("@@InternationalAccount@@", appObject.InternationalAccount);
        //    htmlFile.Replace("@@InternationalBankRequisites@@", appObject.InternationalBankRequisites);
        //    htmlFile.Replace("@@Duns@@", appObject.Duns);
        //    htmlFile.Replace("@@IsConditionsForControl@@", appObject.IsConditionsForControl ? checkbox : "");
        //    htmlFile.Replace("@@IsCheckMPD@@", appObject.IsCheckMpd ? checkbox : "");
        //    htmlFile.Replace("@@IsPaperLicense@@", appObject.IsPaperLicense ? checkbox : "");
        //    htmlFile.Replace("@@IsCourierDelivery@@", appObject.IsCourierDelivery ? checkbox : "");
        //    htmlFile.Replace("@@IsPostDelivery@@", appObject.IsPostDelivery ? checkbox : "");
        //    htmlFile.Replace("@@IsAgreeLicenseTerms@@", appObject.IsAgreeLicenseTerms ? checkbox : "");
        //    htmlFile.Replace("@@IsAgreeProcesingData@@", appObject.IsAgreeProcessingData ? checkbox : "");
        //    htmlFile.Replace("@@IsCourierResults@@", appObject.IsCourierResults ? checkbox : "");
        //    htmlFile.Replace("@@IsPostResults@@", appObject.IsPostResults ? checkbox : "");
        //    htmlFile.Replace("@@IsElectricFormResults@@", appObject.IsElectricFormResults ? checkbox : "");

        //    htmlFile.Replace("@@PassportSerial@@", appObject.PassportSerial);
        //    htmlFile.Replace("@@PassportNumber@@", appObject.PassportNumber);
        //    htmlFile.Replace("@@PassportDate@@",
        //        appObject.PassportDate.HasValue
        //            ? appObject.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
        //            : "");
        //    htmlFile.Replace("@@PassportIssueUnit@@", appObject.PassportIssueUnit);
        //    htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(appObject.INN) ? "</br>" : appObject.INN);

        //    htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);

        //    htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
        //    htmlFile.Replace("@@Date@@",
        //        DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

        //    var infoFile = GetAddon();
        //    htmlFile.Append(infoFile);

        //    return htmlFile.ToString();
        //}

        public async Task<string> TrlCreateLicenseApp(Guid id)
        {
            // Генерація pdf для TRL

            var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();

            var branches = await _dataService.GetDtoAsync<BranchDetailsDTO>(x => x.ApplicationId == id);
            
            if (app == null)
            {
                throw new Exception("No application");
            }

            var activityTypeList = _dataService.GetDto<EntityEnumDTO>(x => x.ApplicationId == id).Select(x => x.EnumCode);

            var appObject = _objectMapper.Map<RptTrlAppDTO>(app);

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();
            var checkboxEmpty = ((char)ushort.Parse("2610", NumberStyles.HexNumber)).ToString();

            var mpdList = "";

            var pathHtml = "Templates/Htmls/TRL/Htmls/CreateLicenseApp/PDFTemplate_CreateLicenseAppTRL.html";
                
            var emailPath = Path.Combine(path, pathHtml);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_MPD_License.html");

            foreach (var branch in branches)
            {
                var mpdFile = new StringBuilder(File.ReadAllText(mpdPath));
                var activityType = _dataService.GetDto<EntityEnumDTO>(x => x.BranchId == branch.Id).FirstOrDefault()?.EnumName;

                if (branch.BranchType == "PharmacyItem")
                {
                    var farmacyItem = _dataService.GetEntity<PharmacyItemPharmacy>(x => x.PharmacyItemId == branch.Id).ToList();

                    foreach (var item in farmacyItem)
                    {
                        var farmacy = _dataService.GetDto<BranchDetailsDTO>(x => x.Id == item.PharmacyId).ToList();
                        if (farmacy.Any())
                        {
                            mpdFile.Replace("@@MPD@@", branch.Name + ", " + "</br>" +
                                                       farmacy.FirstOrDefault()?.Name + "</br>" +
                                                       (string.IsNullOrEmpty(branch.Lpz) ? "" : ", " + branch.Lpz));
                        }
                        else
                        {
                            mpdFile.Replace("@@MPD@@", branch.Name + ", " + "</br>" +
                                                       (string.IsNullOrEmpty(branch.Lpz) ? "" : ", " + branch.Lpz));
                        }
                    }
                    mpdFile.Replace("@@ActivityType@@", activityType ?? "");
                    mpdFile.Replace("@@MpdAddress@@", branch.PostIndex + ", " + branch.Address);
                }
                else
                {
                    mpdFile.Replace("@@MPD@@", branch.Name);
                    mpdFile.Replace("@@MpdAddress@@", branch.PostIndex + ", " + branch.Address);
                    mpdFile.Replace("@@ActivityType@@", activityType ?? "");
                }

                mpdList += mpdFile.ToString();
            }
            htmlFile.Replace("@@MpdList@@", mpdList);

            htmlFile.Replace("@@Name@@", appObject.OrgName + ", " + appObject.Address);

            if (string.IsNullOrEmpty(appObject.EDRPOU))
            {
                htmlFile.Replace("@@OrgName@@", ".");
                htmlFile.Replace("@@FopName@@", appObject.OrgName);
                htmlFile.Replace("@@PassportData@@", 
                    appObject.PassportSerial + ", " + appObject.PassportNumber + ", "
                                             + (appObject.PassportDate.HasValue ? 
                                                  appObject.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                                                  : ""));
                htmlFile.Replace("@@PassportIssueUnit@@", string.IsNullOrEmpty(appObject.PassportIssueUnit) ? "." : appObject.PassportIssueUnit);
                htmlFile.Replace("@@INN@@", appObject.INN);
                htmlFile.Replace("@@EDRPOU@@", ".");
            }
            else
            {
                htmlFile.Replace("@@FopName@@", ".");
                htmlFile.Replace("@@OrgName@@", appObject.OrgDirector);
                htmlFile.Replace("@@PassportData@@", ".");
                htmlFile.Replace("@@PassportIssueUnit@@", ".");
                htmlFile.Replace("@@INN@@", ".");
                htmlFile.Replace("@@EDRPOU@@", appObject.EDRPOU);
            }

            htmlFile.Replace("@@EMail@@", appObject.EMail);
            htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));

            var retailOfMedicines = false;
            var wholesaleOfMedicines = false;
            var prlInPharmacies = false;

            foreach (var activityType in activityTypeList)
            {
                switch (activityType)
                {
                    case "RetailOfMedicines":
                        retailOfMedicines = true;
                        break;
                    case "WholesaleOfMedicines":
                        wholesaleOfMedicines = true;
                        break;
                    case "PrlInPharmacies":
                        prlInPharmacies = true;
                        break;
                }
            }
            htmlFile.Replace("@@RetailOfMedicines@@", !retailOfMedicines ? checkboxEmpty : checkbox);
            htmlFile.Replace("@@WholesaleOfMedicines@@", !wholesaleOfMedicines ? checkboxEmpty : checkbox);
            htmlFile.Replace("@@PrlInPharmacies@@", !prlInPharmacies ? checkboxEmpty : checkbox);

            htmlFile.Replace("@@LegalFormTypeName@@", appObject.LegalFormName);
            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@IsCheckMPD@@", appObject.IsCheckMpd ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@IsCourierDelivery@@", appObject.IsCourierDelivery ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@IsPostDelivery@@", appObject.IsPostDelivery ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@IsCourierResults@@", appObject.IsCourierResults ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@IsPostResults@@", appObject.IsPostResults ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@IsElectricFormResults@@", appObject.IsElectricFormResults ? checkbox : checkboxEmpty);
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            htmlFile.Replace("@@RegDate@@",
                (appObject.RegDate != null ? 
                    appObject.RegDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) :
                    " \"___\" ___________ 20___"));
            htmlFile.Replace("@@RegNumber@@",
                (!string.IsNullOrEmpty(appObject.RegNumber) ? appObject.RegNumber : "__________"));

            var infoFile = GetAddon();
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> TrlCancelLicenseApp(Guid id)
        {
            // Генерація pdf для Trl/Анулювання ліцензії

            var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (app == null)
            {
                throw new Exception();
            }

            var htmlPath = !string.IsNullOrEmpty(app.EDRPOU)
                ? "Templates/Htmls/TRL/Htmls/CancelLicenseApp/PDFTemplate_CancelLicenseAppTRL_ORG.html"
                : "Templates/Htmls/TRL/Htmls/CancelLicenseApp/PDFTemplate_CancelLicenseAppTRL_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            //Changing fields in report
            htmlFile.Replace("@@OrgName@@", app.OrgName);
            htmlFile.Replace("@@AddressString@@", app.PostIndex + ", " + app.Address);
            htmlFile.Replace("@@OrgDirector@@", app.OrgDirector);
            htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(app.EDRPOU) ? "</br>" : app.EDRPOU);

            htmlFile.Replace("@@PassportSerial@@", app.PassportSerial);
            htmlFile.Replace("@@PassportNumber@@", app.PassportNumber);
            htmlFile.Replace("@@PassportDate@@",
                app.PassportDate.HasValue
                    ? app.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                    : "");
            htmlFile.Replace("@@PassportIssueUnit@@", app.PassportIssueUnit);
            htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(app.INN) ? "</br>" : app.INN);

            htmlFile.Replace("@@LicenseInfo@@", await ImlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = GetAddon();
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> TrlRemBranchApp(Guid id)
        {
            // Генерація pdf для Iml/Звуження переліку МПД

            var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptTrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            var assigneeBranches =
                _dataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).Select(x => x.AssigneeId);

            var assignees = await _dataService.GetDtoAsync<AppAssigneeDetailDTO>(x => assigneeBranches.Contains(x.Id));

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var mpdList = "";
            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/TRL/Htmls/RemBranchApp/PDFTemplate_RemBranchAppTRL_ORG.html"
                : "Templates/Htmls/TRL/Htmls/RemBranchApp/PDFTemplate_RemBranchAppTRL_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/RemBranchApp/PDFTemplate_RemBranchAppTRL_MDP.html");
            var employeeList = "";
            var empPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_AutPersonList.html");

            foreach (var employee in assignees)
            {
                var empFile = new StringBuilder(File.ReadAllText(empPath));

                empFile.Replace("@@OrgPositionType@@", employee.OrgPositionType);
                empFile.Replace("@@AutPersonPosition@@", employee.NameOfPosition);
                empFile.Replace("@@AutPersonSurname@@", employee.LastName);
                empFile.Replace("@@AutPersonName@@", employee.Name);
                empFile.Replace("@@AutPersonMiddleName@@", employee.MiddleName);
                empFile.Replace("@@AutPersonEducation@@", employee.EducationInstitution);
                empFile.Replace("@@AutPersonExperience@@", employee.WorkExperience);

                employeeList += empFile.ToString();
            }

            foreach (var branch in appObject.Branches)
                if ((branch.LicenseDeleteCheck != null) && (branch.LicenseDeleteCheck == true))
                {
                    var mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                    //MainInfoTable
                    mpdFile.Replace("@@MDPName@@", branch.Name);

                    mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);

                    mpdList += mpdFile.ToString();
                }

            //Changing fields in report
            htmlFile.Replace("@@MPD@@", mpdList);
            htmlFile.Replace("@@OrgName@@", appObject.OrgName);
            htmlFile.Replace("@@AddressString@@", appObject.PostIndex + ", " + appObject.Address);
            htmlFile.Replace("@@OrgDirector@@", appObject.OrgDirector);
            htmlFile.Replace("@@LegalFormTypeName@@", appObject.LegalFormName);
            htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(appObject.EDRPOU) ? "</br>" : appObject.EDRPOU);
            htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));
            htmlFile.Replace("@@FaxNumber@@", StandartPhone(appObject.FaxNumber));
            htmlFile.Replace("@@EMail@@", appObject.EMail);
            htmlFile.Replace("@@IsPaperLicense@@", appObject.IsPaperLicense ? checkbox : "");
            htmlFile.Replace("@@IsCourierResults@@", appObject.IsCourierResults ? checkbox : "");
            htmlFile.Replace("@@IsPostResults@@", appObject.IsPostResults ? checkbox : "");
            htmlFile.Replace("@@IsElectricFormResults@@", appObject.IsElectricFormResults ? checkbox : "");
            
            htmlFile.Replace("@@PassportSerial@@", appObject.PassportSerial);
            htmlFile.Replace("@@PassportNumber@@", appObject.PassportNumber);
            htmlFile.Replace("@@PassportDate@@",
                appObject.PassportDate.HasValue
                    ? appObject.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                    : "");
            htmlFile.Replace("@@PassportIssueUnit@@", appObject.PassportIssueUnit);
            htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(appObject.INN) ? "</br>" : appObject.INN);
            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            htmlFile.Replace("@@LicenseInfo@@", await ImlGetLicenseInfoRpt(app.OrgUnitId));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = GetAddon();
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> TrlAddBranchApp(Guid id)
        {
            var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assigneeBranches =
                _dataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).Select(x => x.AssigneeId);
            var assignees = await _dataService.GetDtoAsync<AppAssigneeDetailDTO>(x => assigneeBranches.Contains(x.Id));
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptTrlAppDTO>(app);
            appObject.Branches = branches.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var mpdList = "";

            var pathHtml = string.IsNullOrEmpty(app.EDRPOU)
                ? "Templates/Htmls/TRL/Htmls/AddBranchApp/PDFTemplate_AddBranchAppTRL_FOP.html"
                : "Templates/Htmls/TRL/Htmls/AddBranchApp/PDFTemplate_AddBranchAppTRL_ORG.html";
            var emailPath = Path.Combine(path, pathHtml);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_MPD.html");

            var empPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_AutPersonList.html");

            foreach (var employee in assignees)
            {
                var empFile = new StringBuilder(File.ReadAllText(empPath));

                empFile.Replace("@@OrgPositionType@@", employee.OrgPositionType);
                empFile.Replace("@@AutPersonPosition@@", employee.NameOfPosition);
                empFile.Replace("@@AutPersonSurname@@", employee.LastName);
                empFile.Replace("@@AutPersonName@@", employee.Name);
                empFile.Replace("@@AutPersonMiddleName@@", employee.MiddleName);
                empFile.Replace("@@AutPersonEducation@@", employee.EducationInstitution);
                empFile.Replace("@@AutPersonExperience@@", employee.WorkExperience);

                employeeList += empFile.ToString();
            }

            foreach (var branch in appObject.Branches)
            {
                var mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                mpdFile.Replace("@@MPDName@@", branch.Name);

                mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                mpdFile.Replace("@@AddressEng@@", branch.AdressEng);
                mpdFile.Replace("@@PhoneNumber@@", StandartPhone(branch.PhoneNumber));
                mpdFile.Replace("@@FaxNumber@@", StandartPhone(branch.FaxNumber));
                mpdFile.Replace("@@E-mail@@", branch.EMail);
                mpdFile.Replace("@@IMLIsAvailiableStorageZone@@", branch.IMLIsAvailiableStorageZone ? checkbox : "");
                mpdFile.Replace("@@IMLIsAvailiableQuality@@", branch.IMLIsAvailiableQuality ? checkbox : "");
                mpdFile.Replace("@@IMLIsAvailiablePermitIssueZone@@", branch.IMLIsAvailiablePermitIssueZone ? checkbox : "");

                mpdList += mpdFile.ToString();
            }

            htmlFile.Replace("@@MPD@@", mpdList);
            htmlFile.Replace("@@OrgName@@", appObject.OrgName);
            htmlFile.Replace("@@AddressString@@", appObject.PostIndex + ", " + appObject.Address);
            htmlFile.Replace("@@OrgDirector@@", appObject.OrgDirector);
            htmlFile.Replace("@@LegalFormTypeName@@", appObject.LegalFormName);
            htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(appObject.EDRPOU) ? "</br>" : appObject.EDRPOU);
            htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));
            htmlFile.Replace("@@FaxNumber@@", StandartPhone(appObject.FaxNumber));
            htmlFile.Replace("@@EMail@@", appObject.EMail);
            htmlFile.Replace("@@IsCheckMPD@@", appObject.IsCheckMpd ? checkbox : "");
            htmlFile.Replace("@@IsPaperLicense@@", appObject.IsPaperLicense ? checkbox : "");
            htmlFile.Replace("@@IsCourierDelivery@@", appObject.IsCourierDelivery ? checkbox : "");
            htmlFile.Replace("@@IsPostDelivery@@", appObject.IsPostDelivery ? checkbox : "");
            htmlFile.Replace("@@IsAgreeLicenseTerms@@", appObject.IsAgreeLicenseTerms ? checkbox : "");
            htmlFile.Replace("@@IsAgreeProcesingData@@", appObject.IsAgreeProcessingData ? checkbox : "");
            htmlFile.Replace("@@IsCourierResults@@", appObject.IsCourierResults ? checkbox : "");
            htmlFile.Replace("@@IsPostResults@@", appObject.IsPostResults ? checkbox : "");
            htmlFile.Replace("@@IsElectricFormResults@@", appObject.IsElectricFormResults ? checkbox : "");

            htmlFile.Replace("@@PassportSerial@@", appObject.PassportSerial);
            htmlFile.Replace("@@PassportNumber@@", appObject.PassportNumber);
            htmlFile.Replace("@@PassportDate@@",
                appObject.PassportDate.HasValue
                    ? appObject.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                    : "");
            htmlFile.Replace("@@PassportIssueUnit@@", appObject.PassportIssueUnit);
            htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(appObject.INN) ? "</br>" : appObject.INN);
            htmlFile.Replace("@@IsGoodManufacturingPractice@@", appObject.IsGoodManufacturingPractice ? checkbox : "");

            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);

            htmlFile.Replace("@@LicenseInfo@@", await ImlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = GetAddon();
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> TrlChangeAutPersonApp(Guid id)
        {
            // Генерація pdf для Trl/Зміна уповноважених осіб

            var app = (await _dataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assigneeBranches =
                _dataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).Select(x => x.AssigneeId);
            var assignees = await _dataService.GetDtoAsync<AppAssigneeDetailDTO>(x => assigneeBranches.Contains(x.Id));
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptTrlAppAssigneesDTO>(app);

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/TRL/Htmls/ChangeAutPersonApp/PDFTemplate_ChangeAutPersonAppTRL_ORG.html"
                : "Templates/Htmls/TRL/Htmls/ChangeAutPersonApp/PDFTemplate_ChangeAutPersonAppTRL_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var empPath = Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_AutPersonList.html");

            foreach (var employee in assignees)
            {
                var empFile = new StringBuilder(File.ReadAllText(empPath));

                empFile.Replace("@@OrgPositionType@@", employee.OrgPositionType);
                empFile.Replace("@@AutPersonPosition@@", employee.NameOfPosition);
                empFile.Replace("@@AutPersonSurname@@", employee.LastName);
                empFile.Replace("@@AutPersonName@@", employee.Name);
                empFile.Replace("@@AutPersonMiddleName@@", employee.MiddleName);
                empFile.Replace("@@AutPersonEducation@@", employee.EducationInstitution);
                empFile.Replace("@@AutPersonExperience@@", employee.WorkExperience);

                employeeList += empFile.ToString();
            }

            htmlFile.Replace("@@AutPersonList@@", employeeList);
            htmlFile.Replace("@@OrgName@@", appObject.OrgName);
            htmlFile.Replace("@@AddressString@@", appObject.PostIndex + ", " + appObject.Address);
            htmlFile.Replace("@@OrgDirector@@", appObject.OrgDirector);
            htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(appObject.EDRPOU) ? "</br>" : appObject.EDRPOU);
            htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));
            htmlFile.Replace("@@FaxNumber@@", StandartPhone(appObject.FaxNumber));
            htmlFile.Replace("@@EMail@@", appObject.EMail);

            htmlFile.Replace("@@ISChangeManufacture@@", false ? checkbox : "");
            htmlFile.Replace("@@ISChangeLaboratory@@", false ? checkbox : "");
            htmlFile.Replace("@@ISChangePersons@@", true ? checkbox : "");

            htmlFile.Replace("@@IsCourierDelivery@@", appObject.IsCourierDelivery ? checkbox : "");
            htmlFile.Replace("@@IsPostDelivery@@", appObject.IsPostDelivery ? checkbox : "");
            htmlFile.Replace("@@IsCourierResults@@", appObject.IsCourierResults ? checkbox : "");
            htmlFile.Replace("@@IsPostResults@@", appObject.IsPostResults ? checkbox : "");
            htmlFile.Replace("@@IsElectricFormResults@@", appObject.IsElectricFormResults ? checkbox : "");
            htmlFile.Replace("@@IsGoodManufacturingPractice@@", appObject.IsGoodManufacturingPractice ? checkbox : "");

            htmlFile.Replace("@@PassportSerial@@", appObject.PassportSerial);
            htmlFile.Replace("@@PassportNumber@@", appObject.PassportNumber);
            htmlFile.Replace("@@PassportDate@@",
                appObject.PassportDate.HasValue
                    ? appObject.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                    : "");
            htmlFile.Replace("@@PassportIssueUnit@@", appObject.PassportIssueUnit);
            htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(appObject.INN) ? "</br>" : appObject.INN);

            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);
            htmlFile.Replace("@@Contractors@@", "без змін");
            htmlFile.Replace("@@ContractorLabs@@", "без змін");

            htmlFile.Replace("@@LicenseInfo@@", await ImlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = GetAddon();
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }


        private string GetAddon()
        {
            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/TRL/Htmls/PDFTemplate_TRL_INF.html")));
            infoFile.Insert(0, @"<div style='page-break-before:always;'>
                </ div > ");
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            return infoFile.ToString();
        }

        private async Task<string> ImlGetLicenseInfoRpt(Guid? orgId)
        {

            Guid? licGuid;
            if (orgId != null && orgId != Guid.Empty)
                licGuid = _licservice.GetLicenseGuid(orgId);
            else
                licGuid = _licservice.GetLicenseGuid();
            if (licGuid == null)
                return "Ліцензія з імпорту відсутня";
            var license = (await _dataService.GetDtoAsync<ImlLicenseDetailDTO>(dto => dto.Id == licGuid.Value)).FirstOrDefault();

            if ((license == null) || (license.LicType != "IML"))
            {
                return "Ліцензія з імпорту відсутня";
            }

            var licDescription = "Ліцензія номер: ";

            if (!string.IsNullOrEmpty(license.LicenseNumber))
            {
                licDescription += "'" + license.LicenseNumber + "'";
            }
            else
            {
                licDescription += "'без номеру'";
            }

            if (license.LicenseDate != null)
            {
                licDescription += ", від " + license.LicenseDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ".";
            }
            else
            {
                licDescription += ", дату видачі ліцензії не збережено.";
            }

            if (!string.IsNullOrEmpty(license.OrderNumber))
            {
                licDescription += " Видана наказом № " + license.OrderNumber;

                if (license.OrderDate != null)
                {
                    licDescription += ", від " + license.OrderDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ".";
                }
            }
            else
            {
                licDescription += "Інформацію щодо наказу про видачу ліцензії не збережено.";
            }

            // var licDescription = "Інформація про ліцензію відсутня";

            return licDescription;
        }

        private static string StandartPhone(string PhoneNumber)
        {
            // Временное решение до устранения бага на стороне фронта

            string NewPhoneNumber = PhoneNumber;

            if (NewPhoneNumber == null)
                return "";

            if (!(PhoneNumber.Contains("+")) & (PhoneNumber.Length == 10))
            {
                NewPhoneNumber = string.Concat("+38", PhoneNumber[0], "(", PhoneNumber[1], PhoneNumber[2], ")", PhoneNumber[3], PhoneNumber[4], PhoneNumber[5], "-", PhoneNumber[6], PhoneNumber[7], "-", PhoneNumber[8], PhoneNumber[9]);
            }
            else NewPhoneNumber = PhoneNumber;

            return NewPhoneNumber;
        }
    }
}
