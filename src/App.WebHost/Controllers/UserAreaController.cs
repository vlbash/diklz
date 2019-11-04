using System;
using System.Linq;
using App.Business.Services.Common;
using App.Core.Business.Services;
using Microsoft.AspNetCore.Mvc;
using App.Data.DTO.USER;
using App.Data.DTO.FDB;
using App.Business.Services.NotificationServices;
using Microsoft.AspNetCore.Authorization;
using App.Core.Data.Entities.Common;
using App.Data.Models.ORG;
using System.Threading.Tasks;

namespace App.Host.Controllers
{    
    public class UserAreaController: Controller
    {
        private readonly ICommonDataService _commonDataService;
        private readonly IUserInfoService _userInfoService;
        private readonly IFeedBackService _feedBackService;

        public UserAreaController(ICommonDataService commonDataService, IUserInfoService userInfoService, INotificationService notificationService, IFeedBackService feedBackService)
        {
            _commonDataService = commonDataService;
            _userInfoService = userInfoService;
            _feedBackService = feedBackService;
        }

        [Authorize]
        public IActionResult ModalUser()
        {
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userAreaEmployeeDetailDTO = new UserAreaEmployeeDetailDTO();
            var userAreaPersonDetailDTO = new UserAreaPersonDetailDTO();
            var userAreaOrgDetailDTO = new UserAreaOrgDetailDTO();

            try
            {
                // TODO переделать в один запрос
                Guid personGuid = userInfo.PersonId;
                userAreaEmployeeDetailDTO = _commonDataService.GetDto<UserAreaEmployeeDetailDTO>(org_employee => (org_employee.PersonId == userInfo.PersonId)).FirstOrDefault();
                userAreaPersonDetailDTO = _commonDataService.GetDto<UserAreaPersonDetailDTO>(person => (person.Id == userInfo.PersonId)).FirstOrDefault();
                userAreaOrgDetailDTO = _commonDataService.GetDto<UserAreaOrgDetailDTO>(org_organization => (org_organization.Id == userAreaEmployeeDetailDTO.OrgId)).FirstOrDefault();
            }
            catch
            {
                return NotFound();
            }

            var model = new UserAreaDetailDTO
            {
                PIB = userAreaPersonDetailDTO.LastName + " " + userAreaPersonDetailDTO.Name + " " + userAreaPersonDetailDTO.MiddleName,
                Position = userAreaEmployeeDetailDTO.Position,
                UserEmail = userAreaEmployeeDetailDTO.UserEmail,
                OrgEmail = userAreaOrgDetailDTO.Email,
                Phone = userAreaPersonDetailDTO.Phone,

                INN = userAreaPersonDetailDTO.IPN,

                ReceiveOnChangeAllApplication = userAreaEmployeeDetailDTO.ReceiveOnChangeAllApplication,
                ReceiveOnChangeAllMessage = userAreaEmployeeDetailDTO.ReceiveOnChangeAllMessage,
                ReceiveOnChangeOwnApplication = userAreaEmployeeDetailDTO.ReceiveOnChangeOwnApplication,
                ReceiveOnChangeOwnMessage = userAreaEmployeeDetailDTO.ReceiveOnChangeOwnMessage,
                PersonalCabinetStatus = userAreaEmployeeDetailDTO.PersonalCabinetStatus,
                ReceiveOnChangeOrgInfo = userAreaEmployeeDetailDTO.ReceiveOnChangeOrgInfo,
                ReceiveOnOverduePayment = userAreaEmployeeDetailDTO.ReceiveOnOverduePayment
            };

            if (userAreaOrgDetailDTO.Edrpou != null)
            {
                model.EDRPOU = userAreaOrgDetailDTO.Edrpou;
            }
            else
            {
                model.EDRPOU = "Фізична особа-підприємець (РНОКПП (Індивідуальний податковий номер): " + userAreaPersonDetailDTO.IPN + ")";
            }

            return PartialView("_ModalUserArea", model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult InfoUpdate(UserAreaDetailDTO model)
        {
            var isChangedOrg = false;
            var isChangedEmployee = false;
            var isChangedPerson = false;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userEmployeeEntity = new EmployeeExt();
            var userPersonEntity = new Person();
            var userOrgEntity = new OrganizationExt();

            try
            {
                Guid personGuid = userInfo.PersonId;
                userEmployeeEntity = _commonDataService.GetEntity<EmployeeExt>(org_employee => (org_employee.PersonId == userInfo.PersonId)).FirstOrDefault();
                userPersonEntity = _commonDataService.GetEntity<Person>(person => (person.Id == userInfo.PersonId)).FirstOrDefault();
                userOrgEntity = _commonDataService.GetEntity<OrganizationExt>(org_organization => (org_organization.Id == userEmployeeEntity.OrganizationId)).FirstOrDefault();
            }
            catch
            {
                return NotFound();
            }

            // должность
            if (!string.IsNullOrEmpty(model.Position) && (model.Position != userEmployeeEntity.Position))
            {
                isChangedEmployee = true;
                userEmployeeEntity.Position = model.Position;
            }

            // мейл пользователя
            if (!string.IsNullOrEmpty(model.UserEmail) && (model.UserEmail != userEmployeeEntity.UserEmail))
            {
                isChangedEmployee = true;
                userEmployeeEntity.UserEmail = model.UserEmail;
            }

            // мейл компании
            if (!string.IsNullOrEmpty(model.OrgEmail) && (model.OrgEmail != userOrgEntity.EMail))
            {
                isChangedOrg = true;
                userOrgEntity.EMail = model.OrgEmail;
            }

            // телефон пользователя
            if (!string.IsNullOrEmpty(model.Phone) && (model.Phone != userPersonEntity.Phone))
            {
                isChangedPerson = true;
                userPersonEntity.Phone = model.Phone;
            }

            // оповещение ReceiveOnChangeAllApplication
            if (model.ReceiveOnChangeAllApplication != userEmployeeEntity.ReceiveOnChangeAllApplication)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnChangeAllApplication = model.ReceiveOnChangeAllApplication;
            }

            // оповещение ReceiveOnChangeAllMessage
            if (model.ReceiveOnChangeAllMessage != userEmployeeEntity.ReceiveOnChangeAllMessage)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnChangeAllMessage = model.ReceiveOnChangeAllMessage;
            }

            // оповещение ReceiveOnChangeOwnApplication
            if (model.ReceiveOnChangeOwnApplication != userEmployeeEntity.ReceiveOnChangeOwnApplication)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnChangeOwnApplication = model.ReceiveOnChangeOwnApplication;
            }

            // оповещение ReceiveOnChangeOwnMessage
            if (model.ReceiveOnChangeOwnMessage != userEmployeeEntity.ReceiveOnChangeOwnMessage)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnChangeOwnMessage = model.ReceiveOnChangeOwnMessage;
            }

            // оповещение PersonalCabinetStatus - TODO - переименовать
            if (model.PersonalCabinetStatus != userEmployeeEntity.PersonalCabinetStatus)
            {
                isChangedEmployee = true;
                userEmployeeEntity.PersonalCabinetStatus = model.PersonalCabinetStatus;
            }

            // оповещение ReceiveOnChangeOrgInfo - TODO - добавить в модель
            if (model.ReceiveOnChangeOrgInfo != userEmployeeEntity.ReceiveOnChangeOrgInfo)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnChangeOrgInfo = model.ReceiveOnChangeOrgInfo;
            }

            // оповещение ReceiveOnOverduePayment - TODO - добавить в модель
            if (model.ReceiveOnOverduePayment != userEmployeeEntity.ReceiveOnOverduePayment)
            {
                isChangedEmployee = true;
                userEmployeeEntity.ReceiveOnOverduePayment = model.ReceiveOnOverduePayment;
            }

            try
            {
                if (isChangedPerson)
                {
                    _commonDataService.Add(userPersonEntity, true);
                }
            }
            catch
            {
                return Json(new { success = false, errortext = "Помилка збереження інформації в базу даних (персона)!" });
            }

            try
            {
                if (isChangedEmployee)
                {
                    _commonDataService.Add(userEmployeeEntity, true);
                }
            }
            catch
            {
                return Json(new { success = false, errortext = "Помилка збереження інформації в базу даних (співробітник)!" });
            }

            try
            {
                if (isChangedOrg)
                {
                    _commonDataService.Add(userOrgEntity, true);
                }
            }
            catch
            {
                return Json(new { success = false, errortext = "Помилка збереження інформації в базу даних (організація)!" });
            }            

            if (isChangedPerson || isChangedEmployee || isChangedOrg)
            {
                _commonDataService.SaveChanges();
            }

            return Json(new { success = true, errortext = "Зміна даних користувача пройшла успішно!" });
        }

        [AllowAnonymous]
        public async Task<IActionResult> SiteEvaluation(Guid? id)
        {
            if ((id == null) || (id == Guid.Empty))
            {
                return View("SiteEvaluationError");
            }

            var model = await _feedBackService.GetFeedback(Guid.Parse(id.ToString()));

            if ((model == null) || (model.Id == Guid.Empty))
            {
                return View("SiteEvaluationError");
            }

            if (model.IsRated == false)
            {
                return View("SiteEvaluation", new FeedBackDetailsDTO());
            }
            else
            {
                return View("SiteEvaluated");
            }            
        }

        [AllowAnonymous]
        public async Task<IActionResult> SetRating(FeedBackDetailsDTO feedback)
        {
            var isHappy = await _feedBackService.CloseFeedback(feedback.Id, feedback.Comment, feedback.Rating);

            if (isHappy)
            {
                return View("SiteEvaluated");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
