using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Data.DTO.ATU;
using App.Data.DTO.ORG;
using App.Data.DTO.RPT;
using App.Data.Models.MSG;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Business.Services.RptServices
{
    [Authorize(Policy = "Registered")]
    [Area("Msg")]

    public class MsgReportService: IMgsReportService
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _userInfoService;
        private readonly IConverter _converter;
        private readonly IPrlLicenseService _prlLicenseService;
        private readonly IImlLicenseService _imlLicenseService;
        private readonly string _path;

        public MsgReportService(ICommonDataService dataService, IUserInfoService userInfoService, IConverter converter, IPrlLicenseService prlLicenseService, IImlLicenseService imlLicenseService)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _converter = converter;
            _prlLicenseService = prlLicenseService;
            _imlLicenseService = imlLicenseService;
            _path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private void BaseTemplate(string templatePath, Guid id, out StringBuilder htmlFile, out Message pdfMessage)
        {
            var orgExt = new OrganizationExtDetailDTO();

            try
            {
                pdfMessage = _dataService.GetEntity<Message>(x => x.Id == id).FirstOrDefault();
                var orgId = pdfMessage.OrgUnitId;
                orgExt = _dataService.GetDto<OrganizationExtDetailDTO>(x => x.Id == orgId).FirstOrDefault();
            }
            catch
            {
                throw new Exception("Помилка при отриманні інформації про повідомлення та організацію");
            }

            var emailPath = Path.Combine(_path, templatePath);
            htmlFile = new StringBuilder(File.ReadAllText(emailPath));

            htmlFile.Replace("@@RegDate@@", " " + pdfMessage.MessageDate.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            htmlFile.Replace("@@RegNumber@@", " " + pdfMessage.MessageNumber);
            htmlFile.Replace("@@OrgName@@", " '" + orgExt.Name + "' ");

            if (string.IsNullOrEmpty(orgExt.INN))
            {
                htmlFile.Replace("@@OrgCode@@", " (ЄДРПОУ " + orgExt.EDRPOU + ")&emsp;");
            }
            else
            {
                htmlFile.Replace("@@OrgCode@@", " (РНОКПП (Індивідуальний податковий номер) " + orgExt.INN + ")&emsp;");
            }

            if (!string.IsNullOrEmpty(pdfMessage.MessageText))
            {
                var commentPath = Path.Combine(_path, "Templates/Messages/PDFTemplate_MessageComment.html");
                var htmlCommentFile = new StringBuilder(File.ReadAllText(commentPath));

                htmlCommentFile.Replace("@@MessageComment@@", pdfMessage.MessageText);
                htmlFile.Replace("@@MessageComment@@", htmlCommentFile.ToString());
            }
            else
            {
                htmlFile.Replace("@@MessageComment@@", "");
            }
            
            htmlFile.Replace("@@UserName@@", _userInfoService.GetCurrentUserInfo().FullName());
            htmlFile.Replace("@@Date@@", DateTime.Now.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
        }

        public async Task<string> MsgToPDFSgdChiefNameChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_SgdChiefNameChange.html", id, out htmlFile, out pdfMessage);
            
            htmlFile.Replace("@@PIB@@", pdfMessage.SgdShiefFullName);

            var listPath = Path.Combine(_path, "Templates/Messages/PDFTemplate_MsgList.html");
            var listFile = new StringBuilder();

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                Guid? parentId = pdfMessage.ParentId;
                if (pdfMessage.MessageHierarchyType == "Parent")
                {
                    parentId = _dataService.GetEntity<Message>(p => p.MessageParentId == id && p.IsPrlLicense)
                        .Select(p => p.ParentId).SingleOrDefault();
                }
                var licPRL = _dataService.GetDto<LicenseRptMinDetailSgdChiefName>(x => x.Id == parentId).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                listTemp.Replace("@@OrgDir@@", "  (ПІБ Керівника, вказане в ліцензії: " + licPRL.OrgDirector.ToString() + ")");
                listFile.Append(listTemp);
            }
            if (pdfMessage.IsImlLicense)
            {
                Guid? parentId = pdfMessage.ParentId;
                if (pdfMessage.MessageHierarchyType == "Parent")
                {
                    parentId = _dataService.GetEntity<Message>(p => p.MessageParentId == id && p.IsImlLicense)
                        .Select(p => p.ParentId).SingleOrDefault();
                }
                var licIML = _dataService.GetDto<LicenseRptMinDetailSgdChiefName>(x => x.Id == parentId).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                listTemp.Replace("@@OrgDir@@", "  (ПІБ Керівника, вказане в ліцензії: " + licIML.OrgDirector.ToString() + ")");
                listFile.Append(listTemp);
            }

            htmlFile.Replace("@@LicenceList@@", listFile.ToString());

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFOrgFopLocationChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_OrgFopLocationChange.html", id, out htmlFile, out pdfMessage);

            if (pdfMessage.NewLocationId != Guid.Empty)
            {
                var subAddress = _dataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == pdfMessage.NewLocationId).SingleOrDefault();

                if (subAddress != null)
                {
                    htmlFile.Replace("@@NewAddress@@", subAddress.Address);
                }
            }

            var listPath = Path.Combine(_path, "Templates/Messages/PDFTemplate_MsgList.html");
            var listFile = new StringBuilder();

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licPRL.LicenseDate.ToString();

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                listTemp.Replace("@@OrgDir@@", "  (Адреса, вказана в ліцензії: " + licPRL.Address.ToString() + ")");
                listFile.Append(listTemp);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licIML.LicenseDate.ToString();

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                listTemp.Replace("@@OrgDir@@", "  (Адреса, вказана в ліцензії: " + licIML.Address.ToString() + ")");
                listFile.Append(listTemp);
            }

            htmlFile.Replace("@@LicenceList@@", listFile.ToString());

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFSgdNameChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_SgdNameChange.html", id, out htmlFile, out pdfMessage);

            htmlFile.Replace("@@NewName@@", pdfMessage.SgdNewFullName);

            var listPath = Path.Combine(_path, "Templates/Messages/PDFTemplate_MsgList.html");
            var listFile = new StringBuilder();

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                var nameOrg = _dataService.GetDto<OrgRptNameField>(x => x.Id == licPRL.OrgUnitId).FirstOrDefault();
                listTemp.Replace("@@OrgDir@@", "  (Назва компанії, вказана в ліцензії: " + nameOrg.OrgName + ")");
                listFile.Append(listTemp);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@ListItem@@", changeString);
                var nameOrg = _dataService.GetDto<OrgRptNameField>(x => x.Id == licIML.OrgUnitId).FirstOrDefault();
                listTemp.Replace("@@OrgDir@@", "  (Назва компанії, вказана в ліцензії: " + nameOrg.OrgName + ")");
                listFile.Append(listTemp);
            }

            htmlFile.Replace("@@LicenceList@@", listFile.ToString());

            return htmlFile.ToString();
        }
        
        public async Task<string> MsgToPDFAnotherEvent(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_AnotherEvent.html", id, out htmlFile, out pdfMessage);

            htmlFile.Replace("@@Comment@@", pdfMessage.MessageText);

            var listPath = Path.Combine(_path, "Templates/Messages/PDFTemplate_MsgList.html");
            var listFile = new StringBuilder();

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@OrgDir@@", "");
                listTemp.Replace("@@ListItem@@", changeString);
                listFile.Append(listTemp);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var listTemp = new StringBuilder(File.ReadAllText(listPath));
                var changeString = "Ліцензія на діяльність від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));
                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + ", ";
                }

                changeString += "наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                listTemp.Replace("@@OrgDir@@", "");
                listTemp.Replace("@@ListItem@@", changeString);
                listFile.Append(listTemp);
            }

            htmlFile.Replace("@@LicenceList@@", listFile.ToString());

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFMPDActivitySuspension(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_MPDActivitySuspension.html", id, out htmlFile, out pdfMessage);            

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            if (pdfMessage.SuspensionStartDate != null)
            {
                DateTime myDate = DateTime.Parse(pdfMessage.SuspensionStartDate.ToString());
                htmlFile.Replace("@@MPDClosingDate@@", myDate.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            }

            var orgInfo = _dataService.GetDto<OrgUnitRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            htmlFile.Replace("@@MPDInfo@@", orgInfo.MpdName);

            htmlFile.Replace("@@MPDClosingReason@@", pdfMessage.SuspensionReason);

            return htmlFile.ToString();
        }

        // БАГ ПРИ СОЗДАНИИ МЕССЕДЖА - Изменить после устранения!
        public async Task<string> MsgToPDFMPDActivityRestoration(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_MPDActivityRestoration.html", id, out htmlFile, out pdfMessage);

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            if (pdfMessage.SuspensionStartDate != null)
            {
                DateTime myDate = DateTime.Parse(pdfMessage.SuspensionStartDate.ToString());
                htmlFile.Replace("@@MPDClosingDate@@", myDate.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            }

            var orgInfo = _dataService.GetDto<OrgUnitRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            htmlFile.Replace("@@MPDInfo@@", orgInfo.MpdName);

            htmlFile.Replace("@@MPDClosingReason@@", pdfMessage.SuspensionReason);

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFMPDClosingForSomeActivity(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_MPDClosingForSomeActivity.html", id, out htmlFile, out pdfMessage);

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            if (pdfMessage.ClosingDate != null)
            {
                DateTime myDate = DateTime.Parse(pdfMessage.ClosingDate.ToString());
                htmlFile.Replace("@@MPDClosingDate@@", myDate.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            }

            var orgInfo = _dataService.GetDto<OrgUnitRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            htmlFile.Replace("@@MPDInfo@@", orgInfo.MpdName);

            htmlFile.Replace("@@MPDClosingReason@@", pdfMessage.ClosingReason);

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFMPDRestorationAfterSomeActivity(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_MPDRestorationAfterSomeActivity.html", id, out htmlFile, out pdfMessage);

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            if (pdfMessage.RestorationDate != null)
            {
                DateTime myDate = DateTime.Parse(pdfMessage.RestorationDate.ToString());
                htmlFile.Replace("@@MPDClosingDate@@", myDate.ToString("« dd » MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")));
            }

            var orgInfo = _dataService.GetDto<OrgUnitRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            htmlFile.Replace("@@MPDInfo@@", orgInfo.MpdName);

            htmlFile.Replace("@@MPDClosingReason@@", pdfMessage.RestorationReason);

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFMPDLocationRatification(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_MPDLocationRatification.html", id, out htmlFile, out pdfMessage);

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var idLic = _prlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var idLic = _imlLicenseService.GetLicenseGuid(pdfMessage.OrgUnitId);
                var licIml = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == idLic).FirstOrDefault();
                var changeString = " від " + licIml.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIml.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIml.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIml.OrderNumber.ToString() + ", від " + licIml.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            var orgInfo = _dataService.GetDto<OrgUnitRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            htmlFile.Replace("@@MPDInfo@@", orgInfo.MpdName);

            // адрес старый

            var orgAddr = _dataService.GetDto<MPDAddressRptMinDetail>(x => x.Id == pdfMessage.MpdSelectedId).FirstOrDefault();
            var orgAddrStr = _dataService.GetDto<AtuSubjectAddressDTO>(x => x.Id == orgAddr.MpdAddress).FirstOrDefault();
            // AtuSubjectAddressDTO
            htmlFile.Replace("@@OldAddress@@", orgAddrStr.Address);

            // адрес новый
            if (pdfMessage.AddressBusinessActivityId != Guid.Empty)
            {
                var subAddress = _dataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == pdfMessage.AddressBusinessActivityId).SingleOrDefault();

                if (subAddress != null)
                {
                    htmlFile.Replace("@@NewAddress@@", subAddress.Address);
                }
            }

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFPharmacyHeadReplacement(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            htmlFile.Append("Генерацію PDF файлу не реалізовано!");

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFPharmacyAreaChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            htmlFile.Append("Генерацію PDF файлу не реалізовано!");

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFPharmacyNameChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            htmlFile.Append("Генерацію PDF файлу не реалізовано!");

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFLeaseAgreementChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            htmlFile.Append("Генерацію PDF файлу не реалізовано!");

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFProductionDossierChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            BaseTemplate("Templates/Messages/PDFTemplate_ProductionDossierChange.html", id, out htmlFile, out pdfMessage);

            // TODO - добавить обработку других лицензий, когда они появятся
            if (pdfMessage.IsPrlLicense)
            {
                var licPRL = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == _prlLicenseService.GetLicenseGuid()).FirstOrDefault();
                var changeString = " від " + licPRL.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licPRL.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licPRL.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licPRL.OrderNumber.ToString() + ", від " + licPRL.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }
            if (pdfMessage.IsImlLicense)
            {
                var licIML = _dataService.GetDto<LicenseRptMinDetailOrgFopLocation>(x => x.Id == _imlLicenseService.GetLicenseGuid()).FirstOrDefault();
                var changeString = " від " + licIML.LicenseDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"));

                if (string.IsNullOrEmpty(licIML.LicenseNumber))
                {
                    changeString += " б/н, ";
                }
                else
                {
                    changeString += "№" + licIML.LicenseNumber.ToString() + " ";
                }

                changeString += "(наказ №" + licIML.OrderNumber.ToString() + ", від " + licIML.OrderDate.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + ")";

                htmlFile.Replace("@@Licence@@", changeString);
            }

            return htmlFile.ToString();
        }

        public async Task<string> MsgToPDFSupplierChange(Guid id)
        {
            var pdfMessage = new Message();
            var htmlFile = new StringBuilder();

            htmlFile.Append("Генерацію PDF файлу не реалізовано!");

            return htmlFile.ToString();
        }
    }
}
