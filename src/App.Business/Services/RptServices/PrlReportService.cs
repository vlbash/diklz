using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Business.Services.OperationFormList;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Data.DTO.HelperDTOs;
using App.Data.DTO.PRL;
using App.Data.DTO.RPT;
using DinkToPdf.Contracts;
using Newtonsoft.Json;


namespace App.Business.Services.RptServices
{
    public class PrlReportService: IPrlReportService
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _userInfoService;
        private readonly AppAssigneeService _assigneeService;
        private readonly PrlContractorService _contractorService;
        private readonly IObjectMapper _objectMapper;
        private readonly IOperationFormListService _formListService;
        private readonly IPrlLicenseService _licservice;
        private readonly string path;

        public PrlReportService(ICommonDataService dataService, IUserInfoService userInfoService,
            AppAssigneeService assigneeService, PrlContractorService contractorService, IObjectMapper objectMapper,
            IOperationFormListService formListService, IPrlLicenseService licservice)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _assigneeService = assigneeService;
            _contractorService = contractorService;
            _objectMapper = objectMapper;
            _formListService = formListService;
            _licservice = licservice;
            path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public async Task<string> PrlGetAppSort(Guid id)
        {
            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();

            if (app == null)
            {
                throw new Exception();
            }

            return app.AppSort;
        }

        public async Task<string> PrlCreateLicenseApp(Guid id)
        {
            // Генерація pdf для PRL/Отримання ліцензії на виробництво

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assignees = await _assigneeService.GetAssigneeList(id);
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception("No application");
            }

            var appObject = _objectMapper.Map<RptPrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            appObject.Assignees = assignees.ToList();
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var contractorList = "";
            var contractorLabList = "";
            var mpdList = "";

            var pathHtml = string.IsNullOrEmpty(app.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_CreateLicenseApp_FOP.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_CreateLicenseApp_ORG.html";
            var emailPath = Path.Combine(path, pathHtml);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_CreateLicenseApp_MPD.html");
            var mpdFile = new StringBuilder();

            var operationListDTO = _formListService.GetOperationListDTO();

            var firstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!firstTime) { employeeList += "<br>"; }
                firstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            foreach (var branch in appObject.Branches)
            {
                mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                mpdFile.Replace("@@MPDName@@", branch.Name);

                mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                mpdFile.Replace("@@AddressEng@@", branch.AdressEng);
                mpdFile.Replace("@@PhoneNumber@@", StandartPhone(branch.PhoneNumber));
                mpdFile.Replace("@@FaxNumber@@", StandartPhone(branch.FaxNumber));
                mpdFile.Replace("@@E-mail@@", branch.EMail);
                mpdFile.Replace("@@PRLIsAvailiableProdSites@@", branch.PRLIsAvailiableProdSites ? checkbox : "");
                mpdFile.Replace("@@PRLIsAvailiableQualityZone@@", branch.PRLIsAvailiableQualityZone ? checkbox : "");
                mpdFile.Replace("@@PRLIsAvailiableStorageZone@@", branch.PRLIsAvailiableStorageZone ? checkbox : "");
                mpdFile.Replace("@@PRLIsAvailiablePickupZone@@", branch.PRLIsAvailiablePickupZone ? checkbox : "");

                //OperationListFromJSON
                var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListForm);

                var result = new Item();
                var MPDListText = "";

                if (list != null)
                {
                    foreach (var root in list)
                    {
                        foreach (var i in root)
                        {
                            if (i.Key == "id")
                            {
                                result = Find(operationListDTO.FirstLevels, i.Value);
                                if (result != null)
                                {
                                    MPDListText += result.Code + " " + result.Name + "</br>";
                                }
                            }
                            else
                            {
                                if (i.Value != "true" && i.Value != "false")
                                {
                                    MPDListText += "\t" + i.Value + "</br>";
                                }
                            }
                        }
                    }
                }

                mpdFile.Replace("@@LicFormsList@@", MPDListText);
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
            htmlFile.Replace("@@NationalAccount@@", appObject.NationalAccount);
            htmlFile.Replace("@@NationalBankRequisites@@", appObject.NationalBankRequisites);
            htmlFile.Replace("@@InternationalAccount@@", appObject.InternationalAccount);
            htmlFile.Replace("@@InternationalBankRequisites@@", appObject.InternationalBankRequisites);
            htmlFile.Replace("@@Duns@@", appObject.Duns);
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

            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlCancelLicenseApp(Guid id)
        {
            // Генерація pdf для PRL/Анулювання ліцензії

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (app == null)
            {
                throw new Exception();
            }

            var htmlPath = !string.IsNullOrEmpty(app.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_CancelLicenseApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_CancelLicenseApp_FOP.html";
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

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlRemBranchApp(Guid id)
        {
            // Генерація pdf для PRL/Звуження переліку МПД

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assignees = await _assigneeService.GetAssigneeList(id);
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            appObject.Assignees = assignees.ToList();
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var contractorList = "";
            var contractorLabList = "";
            var mpdList = "";
            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchApp_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchApp_MPD.html");
            var mpdFile = new StringBuilder();

            var operationListDTO = _formListService.GetOperationListDTO();
            var FirstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!FirstTime) { employeeList += "<br>"; }
                FirstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            foreach (var branch in appObject.Branches)
                if ((branch.LicenseDeleteCheck != null) && (branch.LicenseDeleteCheck == true))
                {
                    mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                    //MainInfoTable
                    mpdFile.Replace("@@MPDName@@", branch.Name);

                    mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                    mpdFile.Replace("@@PRLIsClose@@", true ? checkbox : "");
                    mpdFile.Replace("@@PRLIsDelete@@", false ? checkbox : "");

                    //OperationListFromJSON
                    var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListForm);

                    var result = new Item();
                    var MPDListText = "";

                    if (list != null)
                    {
                        foreach (var root in list)
                        {
                            foreach (var i in root)
                            {
                                if (i.Key == "id")
                                {
                                    result = Find(operationListDTO.FirstLevels, i.Value);
                                    if (result != null)
                                    {
                                        MPDListText += result.Code + " " + result.Name + "</br>";
                                    }
                                }
                                else
                                {
                                    if (i.Value != "true" && i.Value != "false")
                                    {
                                        MPDListText += "\t" + i.Value + "</br>";
                                    }
                                }
                            }
                        }
                    }

                    mpdFile.Replace("@@LicFormsList@@", MPDListText);
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
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlRemBranchInfoApp(Guid id)
        {
            // Генерація pdf для PRL/Видалення інформації про МПД

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assignees = await _assigneeService.GetAssigneeList(id);
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            appObject.Assignees = assignees.ToList();
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var contractorList = "";
            var contractorLabList = "";
            var mpdList = "";
            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchInfoApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchInfoApp_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_RemBranchInfoApp_MPD.html");
            var mpdFile = new StringBuilder();

            var operationListDTO = _formListService.GetOperationListDTO();

            var firstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!firstTime) { employeeList += "<br>"; }
                firstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            foreach (var branch in appObject.Branches)
            {
                var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListFormChanging);

                if ((list != null) && (list.Count() > 0))
                {
                    mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                    //MainInfoTable
                    mpdFile.Replace("@@MPDName@@", branch.Name);

                    mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                    mpdFile.Replace("@@PRLIsClose@@", false ? checkbox : "");
                    mpdFile.Replace("@@PRLIsDelete@@", true ? checkbox : "");

                    //OperationListFromJSON                  

                    var result = new Item();
                    var MPDListText = "";

                    if (list != null)
                    {
                        foreach (var root in list)
                        {
                            foreach (var i in root)
                            {
                                if (i.Key == "id")
                                {
                                    result = Find(operationListDTO.FirstLevels, i.Value);
                                    if (result != null)
                                    {
                                        MPDListText += result.Code + " " + result.Name + "</br>";
                                    }
                                }
                                else
                                {
                                    if (i.Value != "true" && i.Value != "false")
                                    {
                                        MPDListText += "\t" + i.Value + "</br>";
                                    }
                                }
                            }
                        }
                    }

                    mpdFile.Replace("@@LicFormsList@@", MPDListText);
                    mpdList += mpdFile.ToString();
                }
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
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlAddBranchApp(Guid id)
        {
            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assignees = await _assigneeService.GetAssigneeList(id);
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            appObject.Assignees = assignees.ToList();
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var contractorList = "";
            var contractorLabList = "";
            var mpdList = "";

            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchApp_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchApp_MPD.html");
            var mpdFile = new StringBuilder();

            var operationListDTO = _formListService.GetOperationListDTO();

            var firstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!firstTime) { employeeList += "<br>"; }
                firstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            foreach (var branch in appObject.Branches)
            {
                if ((branch.IsFromLicense != null) && (branch.IsFromLicense == false))
                {
                    mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                    mpdFile.Replace("@@MPDName@@", branch.Name);

                    mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                    mpdFile.Replace("@@AddressEng@@", branch.AdressEng);
                    mpdFile.Replace("@@PhoneNumber@@", StandartPhone(branch.PhoneNumber));
                    mpdFile.Replace("@@FaxNumber@@", StandartPhone(branch.FaxNumber));
                    mpdFile.Replace("@@E-mail@@", branch.EMail);

                    mpdFile.Replace("@@PRLAddMPD@@", true ? checkbox : "");
                    mpdFile.Replace("@@PRLAddSites@@", false ? checkbox : "");

                    mpdFile.Replace("@@PRLIsAvailiableProdSites@@", branch.PRLIsAvailiableProdSites ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiableQualityZone@@", branch.PRLIsAvailiableQualityZone ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiableStorageZone@@", branch.PRLIsAvailiableStorageZone ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiablePickupZone@@", branch.PRLIsAvailiablePickupZone ? checkbox : "");

                    //OperationListFromJSON
                    var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListForm);

                    var result = new Item();
                    var MPDListText = "";
                    if (list != null)
                    {
                        foreach (var root in list)
                        {
                            foreach (var i in root)
                            {
                                if (i.Key == "id")
                                {
                                    result = Find(operationListDTO.FirstLevels, i.Value);
                                    if (result != null)
                                    {
                                        MPDListText += result.Code + " " + result.Name + "</br>";
                                    }
                                }
                                else
                                {
                                    if (i.Value != "true" && i.Value != "false")
                                    {
                                        MPDListText += "\t" + i.Value + "</br>";
                                    }
                                }
                            }
                        }
                    }

                    mpdFile.Replace("@@LicFormsList@@", MPDListText);
                    mpdList += mpdFile.ToString();
                }
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

            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlAddBranchInfoApp(Guid id)
        {
            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var branches = await _dataService.GetDtoAsync<ReportBranchFullDetailsDTO>(x => x.ApplicationId == id);
            var assignees = await _assigneeService.GetAssigneeList(id);
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppDTO>(app);
            appObject.Branches = branches.ToList();
            appObject.Assignees = assignees.ToList();
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var contractorList = "";
            var contractorLabList = "";
            var mpdList = "";

            var emailPath = Path.Combine(path, !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchInfoApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchInfoApp_FOP.html");
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var mpdPath = Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AddBranchInfoApp_MPD.html");
            var mpdFile = new StringBuilder();

            var operationListDTO = _formListService.GetOperationListDTO();

            var firstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!firstTime) { employeeList += "<br>"; }
                firstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            foreach (var branch in appObject.Branches)
            {
                var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(branch.OperationListFormChanging);

                if ((list != null) && (list.Count() > 0))
                {
                    mpdFile = new StringBuilder(File.ReadAllText(mpdPath));

                    mpdFile.Replace("@@MPDName@@", branch.Name);

                    mpdFile.Replace("@@AddressString@@", branch.PostIndex + ", " + branch.Address);
                    mpdFile.Replace("@@AddressEng@@", branch.AdressEng);
                    mpdFile.Replace("@@PhoneNumber@@", StandartPhone(branch.PhoneNumber));
                    mpdFile.Replace("@@FaxNumber@@", StandartPhone(branch.FaxNumber));
                    mpdFile.Replace("@@E-mail@@", branch.EMail);

                    mpdFile.Replace("@@PRLAddMPD@@", false ? checkbox : "");
                    mpdFile.Replace("@@PRLAddSites@@", true ? checkbox : "");

                    mpdFile.Replace("@@PRLIsAvailiableProdSites@@", branch.PRLIsAvailiableProdSites ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiableQualityZone@@", branch.PRLIsAvailiableQualityZone ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiableStorageZone@@", branch.PRLIsAvailiableStorageZone ? checkbox : "");
                    mpdFile.Replace("@@PRLIsAvailiablePickupZone@@", branch.PRLIsAvailiablePickupZone ? checkbox : "");

                    //OperationListFromJSON

                    var result = new Item();
                    var MPDListText = "";
                    if (list != null)
                    {
                        foreach (var root in list)
                        {
                            foreach (var i in root)
                            {
                                if (i.Key == "id")
                                {
                                    result = Find(operationListDTO.FirstLevels, i.Value);
                                    if (result != null)
                                    {
                                        MPDListText += result.Code + " " + result.Name + "</br>";
                                    }
                                }
                                else
                                {
                                    if (i.Value != "true" && i.Value != "false")
                                    {
                                        MPDListText += "\t" + i.Value + "</br>";
                                    }
                                }
                            }
                        }
                    }

                    mpdFile.Replace("@@LicFormsList@@", MPDListText);
                    mpdList += mpdFile.ToString();
                }
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

            htmlFile.Replace("@@OrgEmployeeExt@@", employeeList);
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        //public async Task<string> PrlRenewLicenseApp(Guid id)
        //{
        //    // Генерація pdf для PRL/Переоформлення ліцензії на виробництво

        //    var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();

        //    if (app == null)
        //    {
        //        throw new Exception();
        //    }

        //    var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

        //    var emailPath = Path.GetFullPath(!string.IsNullOrEmpty(app.EDRPOU)
        //        ? "Templates/Htmls/PRL/Htmls/PDFTemplate_RenewLicenseApp_ORG.html"
        //        : "Templates/Htmls/PRL/Htmls/PDFTemplate_RenewLicenseApp_FOP.html");
        //    var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

        //    htmlFile.Replace("@@OrgName@@", app.OrgName);
        //    htmlFile.Replace("@@AddressString@@", app.PostIndex + ", " + app.Address);
        //    htmlFile.Replace("@@OrgDirector@@", app.OrgDirector);
        //    htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(app.EDRPOU) ? "</br>" : app.EDRPOU);
        //    htmlFile.Replace("@@Phone@@", StandartPhone(app.PhoneNumber));
        //    htmlFile.Replace("@@FaxNumber@@", StandartPhone(app.FaxNumber));
        //    htmlFile.Replace("@@EMail@@", app.EMail);

        //    htmlFile.Replace("@@IsPaperLicense@@", app.IsPaperLicense ? checkbox : "");
        //    htmlFile.Replace("@@IsCourierDelivery@@", app.IsCourierDelivery ? checkbox : "");
        //    htmlFile.Replace("@@IsPostDelivery@@", app.IsPostDelivery ? checkbox : "");
        //    htmlFile.Replace("@@IsCourierResults@@", app.IsCourierResults ? checkbox : "");
        //    htmlFile.Replace("@@IsPostResults@@", app.IsPostResults ? checkbox : "");
        //    htmlFile.Replace("@@IsElectricFormResults@@", app.IsElectricFormResults ? checkbox : "");

        //    htmlFile.Replace("@@PassportSerial@@", app.PassportSerial);
        //    htmlFile.Replace("@@PassportNumber@@", app.PassportNumber);
        //    htmlFile.Replace("@@PassportDate@@",
        //        app.PassportDate.HasValue
        //            ? app.PassportDate.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
        //            : "");
        //    htmlFile.Replace("@@PassportIssueUnit@@", app.PassportIssueUnit);
        //    htmlFile.Replace("@@INN@@", string.IsNullOrEmpty(app.INN) ? "</br>" : app.INN);

        //    htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt());

        //    htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
        //    htmlFile.Replace("@@Date@@",
        //        DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

        //    // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
        //    var infoFile = new StringBuilder(File.ReadAllText(Path.GetFullPath("Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
        //    infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
        //    htmlFile.Append(infoFile);

        //    return htmlFile.ToString();
        //}

        public async Task<string> PrlChangeContrApp(Guid id)
        {
            // Генерація pdf для PRL/Зміна контрактних виробників та лабораторій

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var contractors = await _contractorService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppContractorsDTO>(app);
            appObject.Contractors = contractors.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var contractorList = "";
            var contractorLabList = "";

            var emailPath = Path.Combine(path, !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_ChangeContrApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_ChangeContrApp_FOP.html");
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var firstTimeContractor = true;
            var firstTimeLaboratory = true;

            foreach (var contractor in appObject.Contractors)
            {
                if (contractor.ContractorType == "Manufacturer")
                {
                    if (!firstTimeContractor) { contractorList += "<br>"; }
                    firstTimeContractor = false;

                    contractorList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ") ";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorList += "<li>" + branch + "</li>";
                        }
                        contractorList += "</ul>";
                    }
                }

                if (contractor.ContractorType == "Laboratory")
                {
                    if (!firstTimeLaboratory) { contractorLabList += "<br>"; }
                    firstTimeLaboratory = false;

                    contractorLabList += "<b>" + contractor.Name + "</b> (" + contractor.Address + ")";

                    if ((contractor.ListOfBranchsNames != null) && (contractor.ListOfBranchsNames.Count > 0))
                    {
                        contractorLabList += "<ul>";
                        foreach (var branch in contractor.ListOfBranchsNames)
                        {
                            contractorLabList += "<li>" + branch + "</li>";
                        }
                        contractorLabList += "</ul>";
                    }
                }
            }

            htmlFile.Replace("@@OrgName@@", appObject.OrgName);
            htmlFile.Replace("@@AddressString@@", appObject.PostIndex + ", " + appObject.Address);
            htmlFile.Replace("@@OrgDirector@@", appObject.OrgDirector);
            htmlFile.Replace("@@EDRPOU@@", string.IsNullOrEmpty(appObject.EDRPOU) ? "</br>" : appObject.EDRPOU);
            htmlFile.Replace("@@Phone@@", StandartPhone(appObject.PhoneNumber));
            htmlFile.Replace("@@FaxNumber@@", StandartPhone(appObject.FaxNumber));
            htmlFile.Replace("@@EMail@@", appObject.EMail);

            htmlFile.Replace("@@ISChangeManufacture@@", true ? checkbox : "");
            htmlFile.Replace("@@ISChangeLaboratory@@", true ? checkbox : "");
            htmlFile.Replace("@@ISChangePersons@@", false ? checkbox : "");

            htmlFile.Replace("@@IsCourierDelivery@@", appObject.IsCourierDelivery ? checkbox : "");
            htmlFile.Replace("@@IsPostDelivery@@", appObject.IsPostDelivery ? checkbox : "");
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

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@OrgEmployeeExt@@", "без змін");
            htmlFile.Replace("@@Contractors@@", contractorList);
            htmlFile.Replace("@@ContractorLabs@@", contractorLabList);

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
        }

        public async Task<string> PrlChangeAutPersonApp(Guid id)
        {
            // Генерація pdf для PRL/Зміна уповноважених осіб

            var app = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            var assignees = await _assigneeService.GetAssigneeList(id);
            if (app == null)
            {
                throw new Exception();
            }

            var appObject = _objectMapper.Map<RptPrlAppAssigneesDTO>(app);
            appObject.Assignees = assignees.ToList();

            var checkbox = ((char)ushort.Parse("2611", NumberStyles.HexNumber)).ToString();

            var employeeList = "";
            var htmlPath = !string.IsNullOrEmpty(appObject.EDRPOU)
                ? "Templates/Htmls/PRL/Htmls/PDFTemplate_ChangeAutPersonApp_ORG.html"
                : "Templates/Htmls/PRL/Htmls/PDFTemplate_ChangeAutPersonApp_FOP.html";
            var emailPath = Path.Combine(path, htmlPath);
            var htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            var firstTime = true;

            foreach (var employee in appObject.Assignees)
            {
                if (!firstTime) { employeeList += "<br>"; }
                firstTime = false;

                employeeList += "<b>" + employee.FIO + "</b> (" + employee.NameOfPosition + ") ";
                employeeList += "<ul>";

                foreach (var employeebranch in employee.ListOfBranches)
                {
                    employeeList += "<li>";
                    employeeList += employeebranch.Name + " (" + employeebranch.Address + " )";
                    employeeList += "</li>";
                }

                employeeList += "</ul>";
            }


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

            htmlFile.Replace("@@LicenseInfo@@", await PrlGetLicenseInfoRpt(app.OrgUnitId));

            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@",
                DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));

            // Файл для добавления с новой страницы аттача подтверждения отсутствия агресиии нерезидента
            var infoFile = new StringBuilder(File.ReadAllText(Path.Combine(path, "Templates/Htmls/PRL/Htmls/PDFTemplate_AppAddon.html")));
            infoFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Append(infoFile);

            return htmlFile.ToString();
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

        public static Item Find(List<Item> node, string code)
        {
            foreach (var i in node)
            {
                if (i == null)
                {
                    return null;
                }

                if (i.Code == code)
                {
                    return i;
                }

                if (i.ChildItems != null)
                {
                    foreach (var child in i.ChildItems)
                    {
                        var found = Find(i.ChildItems, code);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }

            return null;
        }

        private async Task<string> PrlGetLicenseInfoRpt(Guid? orgId)
        {

            Guid licGuid;
            if (orgId != null && orgId != Guid.Empty)
                licGuid = _licservice.GetLicenseGuid(orgId).Value;
            else
                licGuid = _licservice.GetLicenseGuid().Value;

            var license = (await _dataService.GetDtoAsync<PrlLicenseDetailDTO>(dto => dto.Id == licGuid)).FirstOrDefault();

            if ((license == null) || (license.LicType != "PRL"))
            {
                return "Ліцензія з виробництва відсутня";
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

        #region ExcelReport

        //  Тест Excel - метод -------------------------------------------------------------------------------------------------------------------
        //  FileInfo existingFile = new FileInfo(Path.GetFullPath("Templates/Htmls/PRL/Htmls/TestExcelUnderLinux.xlsx"));

        //  ExcelPackage eP = new ExcelPackage(existingFile, true);

        //  eP.Workbook.Properties.Author = "Бит Софт Групп";
        //  eP.Workbook.Properties.Title = "Новый тестовый лист";
        //  eP.Workbook.Properties.Company = "БитСофтГрупп";

        //  var sheet = eP.Workbook.Worksheets.Add("Testlist2");

        //  for (int i = 1; i< 21; i++)
        //  {
        //      sheet.Cells[i + 11, 6].Value = i;
        //  }

        //  byte[] file = eP.GetAsByteArray();

        //  return Convert.ToBase64String(file);

        // контроллер -----------------------------------------------------------------------------------------------------------------------------
        // string str = await _prlReportService.PrlRenewLicenseApp(id);
        // file = Convert.FromBase64String(str);
        //                return File(file, "application/vnd.ms-excel", "TestExcelUnderLinux.xlsx");

        #endregion
    }
}
