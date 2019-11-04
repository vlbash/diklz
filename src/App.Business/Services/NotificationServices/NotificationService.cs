using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Data.DTO.PRL;
using App.Data.DTO.NTF;
using App.Data.Models.NTF;
using App.Business.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using App.Data.DTO.USER;
using App.Data.Models.ORG;
using App.Business.Services.EmailService;
using App.Data.DTO.RPT;

namespace App.Business.Services.NotificationServices
{
    [Authorize(Policy = "Registered")]
    [Area("Ntf")]

    public class NotificationService: INotificationService
    {
        private ICommonDataService _dataService;
        private ISendEmailService _sendEmailService;
        private IUserInfoService _userInfoService;
        private IFeedBackService _feedBackService;
        private IConfiguration _config;
        private readonly string _path;

        public NotificationService(ICommonDataService dataService, IUserInfoService userInfoService, IConfiguration configuration, ISendEmailService sendEmailService, IFeedBackService feedBackService)
        {
            _dataService = dataService;
            _userInfoService = userInfoService;
            _config = configuration;
            _sendEmailService = sendEmailService;
            _feedBackService = feedBackService;
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        private Guid Save(Notification model)
        {
            var notificationId = Guid.Empty;

            try
            { 
                notificationId = _dataService.Add<Notification>(model);
                _dataService.SaveChanges();
            }
            catch
            {
                notificationId = Guid.Empty;
            }

            return notificationId;
        }

        public string GetFeedbackLink(Guid id)
        {
            var linkString = _config["EmailPath:Urls"] + "/evaluation?id=" + id.ToString();

            return linkString;
        }

        public string GetAppLink(Guid id)
        {
            var app = (_dataService.GetDto<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();

            if (app == null)
            {
                throw new Exception();
            }

            string linkString = null;

            // Тип заявы - на получение, статус - проект => В первый контроллер
            if ((app.AppState == "Project") && (app.AppSort == "GetLicenseApplication"))
            {
                linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:appPath"] + "/Details/" + id.ToString();
            }

            // Тип заявы - НЕ на получение, статус - проект => В основной контроллер
            if ((app.AppState == "Project") && !(app.AppSort == "GetLicenseApplication"))
            {
                linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:appAltPath"] + "/AltAppDetails/" + id.ToString() + "?sort=" + app.AppSort.ToString();
            }

            // Cтатус заявы - НЕ проект => В мониторинг
            if (!(app.AppState == "Project"))
            {
                linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:messagePath"] + "/" + id.ToString() + "?sort=" + app.AppSort.ToString();
            }

            return linkString;
        }

        public string GetMsgLink(Guid id)
        {
            var linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:messagePath"] + "/Details/" + id.ToString();

            return linkString;
        }

        public string GetMonitoringLink(Guid id)
        {
            var app = (_dataService.GetDto<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            string linkString;

            if (app == null)
            {
                linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:monitoringPath"];
            }
            else
            {
                linkString = _config["EmailPath:Urls"] + "/" + _config["EmailPath:monitoringPath"] + "/Details/" + id.ToString() + "?sort=" + app.AppSort.ToString();
            }            

            return linkString;
        }

        public string GetCodeAppSort(string appSort)
        {
            var codeAppSort = "";

            switch (appSort)
            {
                case "GetLicenseApplication":
                    codeAppSort = "Заяву про отримання ліцензії на провадження діяльності";
                    break;

                case "AddBranchApplication":
                    codeAppSort = "Заяву про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання МПД";
                    break;

                case "AddBranchInfoApplication":
                    codeAppSort = "Заяву про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання інформації про МПД";
                    break;

                case "RemBranchApplication":
                    codeAppSort = "Заяву про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення МПД";
                    break;

                case "RemBranchInfoApplication":
                    codeAppSort = "Заяву про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення інформації про МПД";
                    break;

                case "ChangeAutPersonApplication":
                    codeAppSort = "Заяву про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна уповноважених осіб";
                    break;

                case "ChangeContrApplication":
                    codeAppSort = "Заяву про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна контрактних контрагентів";
                    break;

                case "DecreaseActTypeApplication":
                    codeAppSort = "Заяву про звуження провадження виду господарської діяльності";
                    break;

                case "IncreaseToPRLApplication":
                    codeAppSort = "Заяву про розширення провадження виду господарської діяльності - Розширення до виробництва лікарських засобів";
                    break;

                case "IncreaseToIRLApplication":
                    codeAppSort = "Заяву про розширення провадження виду господарської діяльності - Розширення до імпорту лікарських засобів";
                    break;

                case "IncreaseToMRLApplication":
                    codeAppSort = "Заяву про розширення провадження виду господарської діяльності - Розширення до торгівлі лікарськими засобами";
                    break;

                case "CancelLicenseApplication":
                    codeAppSort = "Заяву про анулювання ліцензії";
                    break;
            }

            return codeAppSort;
        }

        public string GetCodeMsgSort(string msgSort)
        {
            var codeMsgSort = "";

            switch (msgSort)
            {
                case "SgdChiefNameChange":
                    codeMsgSort = "Повідомлення про зміну ПІБ керівника Суб'єкту господарювання";
                    break;

                case "SgdNameChange":
                    codeMsgSort = "Повідомлення про зміну найменування Суб'єкту господарювання";
                    break;

                case "OrgFopLocationChange":
                    codeMsgSort = "Повідомлення про зміну місця знаходження юридичної особи / фізичної особи підприємця";
                    break;

                case "MPDActivitySuspension":
                    codeMsgSort = "Повідомлення про призупинення провадження діяльності МПД";
                    break;

                case "MPDActivityRestoration":
                    codeMsgSort = "Повідомлення про відновлення провадження діяльності МПД";
                    break;

                case "MPDClosingForSomeActivity":
                    codeMsgSort = "Повідомлення про закриття місця провадження діяльності для проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності";
                    break;

                case "MPDRestorationAfterSomeActivity":
                    codeMsgSort = "Повідомлення про відновлення роботи місця провадження діяльності після проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності";
                    break;

                case "MPDLocationRatification":
                    codeMsgSort = "Повідомлення про уточнення адреси місця провадження діяльності";
                    break;

                case "PharmacyHeadReplacement":
                    codeMsgSort = "Повідомлення про заміну завідуючого аптечного пункту";
                    break;

                case "PharmacyAreaChange":
                    codeMsgSort = "Повідомлення про зміну площі аптечного закладу";
                    break;

                case "PharmacyNameChange":
                    codeMsgSort = "Повідомлення про заміну назви аптечного закладу";
                    break;

                case "LeaseAgreementChange":
                    codeMsgSort = "Повідомлення про зміну договору оренди";
                    break;

                case "ProductionDossierChange":
                    codeMsgSort = "Повідомлення про заміну або нова редакція Досьє з виробництва";
                    break;

                case "SupplierChange":
                    codeMsgSort = "Повідомлення про зміну постачальника";
                    break;

                case "AnotherEvent":
                    codeMsgSort = "Повідомлення про довільну подію (не з переліку)";
                    break;
            }

            return codeMsgSort;
        }

        public bool GetPayNeed(string appSort)
        {
            bool needPayResult;

            switch (appSort)
            {
                case "GetLicenseApplication":
                    needPayResult = true;
                    break;

                case "AddBranchApplication":
                    needPayResult = true;
                    break;

                default:
                    needPayResult = false;
                    break;
            }

            return needPayResult;
        }

        private bool IsFOPbyID(Guid id)
        {
            var orgInfo = _dataService.GetDto<OrgInnEdrpouRptMinDetail>(x => x.Id == id).FirstOrDefault();

            if (orgInfo == null)
            {
                throw new Exception("Не вдалося отримати інформацію про організацію при формуванні сповіщення");
            }

            if ((orgInfo == null) && (!string.IsNullOrEmpty(orgInfo.EDRPOU)))
            {
                return true;
            }

            return false;
        }

        private List<string> GetPersonRecipientList(UserInfo userInfo, string notificationSort)
        {
            var recipientList = new List<string>();

            try
            {
                var employeeInfo = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault();

                switch (notificationSort)
                {
                    case "NotificationAppSend_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeOwnApplication))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationAppSend_Org":
                        break;

                    case "NotificationMsgSend_Org":
                        break;

                    case "NotificationMsgSend_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeAllMessage))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationAppRegister_Org":
                        break;

                    case "NotificationAppRegister_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeOwnApplication))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationAppResolve_Org":
                        break;

                    case "NotificationAppResolve_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeOwnApplication))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationAppResolvePay_Org":
                        break;

                    case "NotificationAppResolvePay_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeOwnApplication))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationAppResolvePayRepeatedly_Org":
                        break;

                    case "NotificationMsgResolve_Org":
                        break;

                    case "NotificationMsgResolve_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.ReceiveOnChangeOwnMessage))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;

                    case "NotificationDeleteDrafts_User":
                        if ((!string.IsNullOrEmpty(employeeInfo.UserEmail)) && (employeeInfo.PersonalCabinetStatus))
                        {
                            recipientList.Add(employeeInfo.UserEmail);
                        }
                        break;
                }
            }
            catch
            {
                throw new Exception("Не вдається отримати інформацію про користувача");
            }

            return recipientList;
        }

        private List<string> GetOrgRecipientList(UserInfo userInfo, string notificationSort, string emailDublicate, Guid orgGuid)
        {
            var recipientList = new List<string>();

            var rList = _dataService.GetDto<NotificationRecipientDTO>();

            var employeeInfo = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault();

            switch (notificationSort)
            {
                case "NotificationAppSend_User":
                    break;

                case "NotificationAppSend_Org":                    
                        foreach (var item in rList)
                        {
                            if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllApplication))
                            {
                                recipientList.Add(item.RecipientEmail);
                            }
                        }
                    break;

                case "NotificationMsgSend_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllMessage))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationMsgSend_User":
                    break;

                case "NotificationAppRegister_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllApplication))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationAppRegister_User":
                    break;

                case "NotificationAppResolve_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllApplication))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationAppResolve_User":
                    break;

                case "NotificationAppResolvePay_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllApplication))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationAppResolvePayRepeatedly_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnOverduePayment))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationMsgResolve_Org":
                    foreach (var item in rList)
                    {
                        if ((!string.IsNullOrEmpty(item.RecipientEmail)) && (item.RecipientEmail != emailDublicate) && (item.ReceiveOnChangeAllMessage))
                        {
                            recipientList.Add(item.RecipientEmail);
                        }
                    }
                    break;

                case "NotificationMsgResolve_User":
                    break;

                case "NotificationDeleteDrafts_User":
                    break;
            }

            // Додавання мейлу організації
            var userOrgEntity = new OrganizationExt();
            try
            {
                userOrgEntity = _dataService.GetEntity<OrganizationExt>(org_organization => (org_organization.Id == orgGuid)).FirstOrDefault();
                recipientList.Add(userOrgEntity.EMail);
            }
            catch
            {
                throw new Exception("Помилка при отриманні інформації по організації");
            }

            return recipientList;
        }

        public async Task<bool> PrlCreateNotificationAppResolve(Guid id, string appSort, string appDate, Guid orgInfoId, bool isPositiveDecision)
        {
            // Сповіщення про прийняття рішення по заяві для якої не потрібна сплата за послуги ДЛС

            var isFOP = IsFOPbyID(orgInfoId);

            if (isPositiveDecision && GetPayNeed(appSort))
            {
                return await PrlCreateNotificationAppResolvePay(id, appSort, appDate, orgInfoId);
            }

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationAppResolve_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolve_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Прийнято рішення по заяві";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationAppResolve_User";

                htmlPersonFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + appDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            string employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationAppResolve_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolve_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Прийнято рішення по заяві";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationAppResolve_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }
                
                htmlOrgFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + appDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? "")); 
                htmlOrgFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);
                
                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson= JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

        public async Task<bool> PrlCreateNotificationAppSend(Guid id, string appSort, string appDate, Guid orgInfoId)
        {
            // Сповіщення про відправку заяви до ДЛС (з фідбеком)

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationAppSend_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppSend_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Заяву відправлено на розгляд до ДЛС";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationAppSend_User";

                htmlPersonFile.Replace("@@msgtype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + appDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));

                // створення фідбеку TODO - записывам не то оргИД
                htmlPersonFile.Replace("@@feedbackLink@@", GetFeedbackLink(await _feedBackService.CreateFeedback(userInfo.UserId, orgInfoId, appSort, id)));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            string employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationAppSend_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppSend_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Заяву відправлено на розгляд до ДЛС";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationAppSend_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@msgtype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + appDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);
                
                Save(modelOrg);
            }

            if ((modelPerson != null) && (modelPerson.IsSend == true))
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if ((modelOrg != null) && (modelOrg.IsSend == true))
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

        public async Task<bool> PrlCreateNotificationAppRegister(Guid id, string appSort, string appDate, Guid orgInfoId, string regNum, string regDate)
        {
            // Сповіщення про реєстрацію заяви у ДЛС

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationAppRegister_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppRegister_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Заяву зареєстровано у ДЛС";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationAppRegister_User";

                htmlPersonFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + appDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@regdate@@", regDate);
                htmlPersonFile.Replace("@@regnumber@@", regNum);
                htmlPersonFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            string employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationAppRegister_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppRegister_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Заяву зареєстровано у ДЛС";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationAppRegister_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + appDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@regdate@@", regDate);
                htmlOrgFile.Replace("@@regnumber@@", regNum);
                htmlOrgFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);

                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));

        }

        public async Task<bool> PrlCreateNotificationAppResolvePay(Guid id, string appSort, string appDate, Guid orgInfoId)
        {
            // Сповіщення про прийняття рішення по заяві для якої потрібна сплата за послуги ДЛС

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationAppResolvePay_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolvePay_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Прийнято рішення по заяві, що потребує сплати за послуги ДЛС";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationAppResolvePay_User";

                htmlPersonFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + appDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            var employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationAppResolvePay_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolvePay_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Прийнято рішення по заяві, що потребує сплати за послуги ДЛС";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationAppResolvePay_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + appDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);

                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

        public async Task<bool> PrlCreateNotificationAppResolvePayRepeatedly(Guid id, string appSort, string appDate, Guid orgInfoId)
        {
            // Нагадування (періодичне) про необхідність сплати за послуги ДЛС

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationAppResolvePay_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolvePay_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Прийнято рішення по заяві, що потребує сплати за послуги ДЛС";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationAppResolvePay_User";

                htmlPersonFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + appDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            var employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationAppResolvePay_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationAppResolvePay_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Прийнято рішення по заяві, що потребує сплати за послуги ДЛС";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationAppResolvePay_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@apptype@@", "'" + GetCodeAppSort(appSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + appDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@monitoringLink@@", GetMonitoringLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);

                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

        public async Task<bool> PrlCreateNotificationMsgSend(Guid id, string msgSort, string msgDate, Guid orgInfoId)
        {
            // Сповіщення про відправку повідомлення до ДЛС (з фідбеком)

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationMsgSend_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationMsgSend_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Повідомлення відправлено на розгляд до ДЛС";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationMsgSend_User";

                htmlPersonFile.Replace("@@msgtype@@", "'" + GetCodeMsgSort(msgSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + msgDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMsgLink(id));

                // створення фідбеку TODO - записывам не то оргИД
                htmlPersonFile.Replace("@@feedbackLink@@", GetFeedbackLink(await _feedBackService.CreateFeedback(userInfo.UserId, orgInfoId, msgSort, id)));

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            string employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationMsgSend_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationMsgSend_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Повідомлення відправлено на розгляд до ДЛС";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationMsgSend_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@msgtype@@", "'" + GetCodeMsgSort(msgSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + msgDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@monitoringLink@@", GetMsgLink(id));
                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);

                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

        public async Task<bool> PrlCreateNotificationMsgResolve(Guid id, string msgSort, string msgDate, Guid orgInfoId, bool isAsseptDecision)
        {
            // Сповіщення про прийняття рішення по повідомленню

            var isFOP = IsFOPbyID(orgInfoId);

            var userInfo = _userInfoService.GetCurrentUserInfo();

            // створення персонального сповіщення
            var personRecipientList = GetPersonRecipientList(userInfo, "NotificationMsgResolve_User");
            var modelPerson = new Notification();

            if ((personRecipientList != null) && (personRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationMsgResolve_User.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlPersonFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelPerson.DateOfCreate = DateTime.Now;
                modelPerson.NotificationSubject = "Прийнято рішення по повідомленню";
                modelPerson.NotificationType = "Особисте";
                modelPerson.NotificationSort = "NotificationMsgResolve_User";                

                htmlPersonFile.Replace("@@msgtype@@", "'" + GetCodeMsgSort(msgSort) + "'");
                htmlPersonFile.Replace("@@date@@", "'" + msgDate + "'");
                htmlPersonFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlPersonFile.Replace("@@monitoringLink@@", GetMsgLink(id));

                if (isAsseptDecision)
                {
                    htmlPersonFile.Replace("@@msgdecision@@", "ПРИЙНЯТО");
                }
                else
                {
                    htmlPersonFile.Replace("@@msgdecision@@", "НЕ ПРИЙНЯТО");
                }                

                modelPerson.NotificationText = htmlPersonFile.ToString();

                modelPerson.IsSend = true;
                modelPerson.RecipientJsonList = JsonConvert.SerializeObject(personRecipientList);

                Save(modelPerson);
            }

            // створення сповіщення по організації 

            string employeeEmail = "";

            if (modelPerson.IsSend == true)
            {
                employeeEmail = _dataService.GetDto<UserAreaEmployeeDetailDTO>(x => (x.Id == userInfo.UserId)).FirstOrDefault().UserEmail;
            }

            var orgRecipientList = GetOrgRecipientList(userInfo, "NotificationMsgResolve_Org", employeeEmail, orgInfoId);
            var modelOrg = new Notification();

            if ((orgRecipientList != null) && (orgRecipientList.Count > 0))
            {
                var pathHtml = "Templates/Emails/NotificationMsgResolve_Org.html";
                var notificationPath = Path.Combine(_path, pathHtml);
                var htmlOrgFile = new StringBuilder(File.ReadAllText(notificationPath));

                modelOrg.DateOfCreate = DateTime.Now;
                modelOrg.NotificationSubject = "Прийнято рішення по повідомленню";
                modelOrg.NotificationType = "Загальне";
                modelOrg.NotificationSort = "NotificationMsgResolve_Org";

                var orgName = _dataService.GetEntity<OrganizationExt>(x => x.Id == Guid.Parse(userInfo.LoginData["OrganizationId"])).FirstOrDefault().Name;

                if (isFOP == true)
                {
                    htmlOrgFile.Replace("@@orgName@@", "ФОП '" + orgName + "'");
                }
                else
                {
                    htmlOrgFile.Replace("@@orgName@@", "'" + orgName + "'");
                }

                htmlOrgFile.Replace("@@msgtype@@", "'" + GetCodeMsgSort(msgSort) + "'");
                htmlOrgFile.Replace("@@date@@", "'" + msgDate + "'");
                htmlOrgFile.Replace("@@name@@", (userInfo.LoginData["LastName"] ?? "") + " " + (userInfo.LoginData["Name"] ?? "") + " " + (userInfo.LoginData["MiddleName"] ?? ""));
                htmlOrgFile.Replace("@@monitoringLink@@", GetMsgLink(id));
                if (isAsseptDecision)
                {
                    htmlOrgFile.Replace("@@msgdecision@@", "ПРИЙНЯТО");
                }
                else
                {
                    htmlOrgFile.Replace("@@msgdecision@@", "НЕ ПРИЙНЯТО");
                }

                modelOrg.NotificationText = htmlOrgFile.ToString();

                modelOrg.IsSend = true;
                modelOrg.RecipientJsonList = JsonConvert.SerializeObject(orgRecipientList);

                Save(modelOrg);
            }

            if (modelPerson.IsSend == true)
            {
                var listPerson = JsonConvert.DeserializeObject<List<string>>(modelPerson.RecipientJsonList);
                await _sendEmailService.SendAsync(listPerson, modelPerson.NotificationText, "Держлікслужба - " + modelPerson.NotificationSubject);
            }

            if (modelOrg.IsSend == true)
            {
                var listOrg = JsonConvert.DeserializeObject<List<string>>(modelOrg.RecipientJsonList);
                await _sendEmailService.SendAsync(listOrg, modelOrg.NotificationText, "Держлікслужба - " + modelOrg.NotificationSubject);
            }

            return ((modelPerson != null) && (modelPerson.IsSend == true) && (modelOrg != null) && (modelOrg.IsSend == true));
        }

    }
}
