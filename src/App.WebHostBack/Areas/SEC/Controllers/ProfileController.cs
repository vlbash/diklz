using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.ControllerServices;
using App.Core.Business.Services;
using App.Core.Data.DTO.ATU;
using App.Core.Data.DTO.Org;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.ORG;
using App.Core.Mvc.Controllers;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace App.Host.Areas.SEC.Controllers
{
    [Area("Sec")]
    [Authorize]
    public class ProfileController: CommonController<ProfileListDTO, ProfileDetailDTO, Profile>

    {
        private readonly ProfileControllerService _service;

        public ProfileController(ProfileControllerService controllerService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService) : base(controllerService.DataService, configuration, searchFilterSettingsService)
        {
            _service = controllerService;
        }

        #region ProfileEmployees

        public async Task<IActionResult> ProfileEmployees(Guid profileId)
        {
            ViewBag.ProfileId = profileId;

            var employeeList = (await DataService.GetDtoAsync<OrgEmployeeMinDTO>()).Select(x => new {userId = x.Id, FIO = x.Name});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.EmployeeList = JsonConvert.SerializeObject(employeeList, serializerSettings);

            return PartialView("ProfileEmployeesPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ProfileEmployeeListDTO>> GetProfileEmployees(Guid profileId)
        {
            var data = await DataService.GetDtoAsync<ProfileEmployeeListDTO>(fr => fr.ProfileId == profileId);
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertProfileEmployee([FromBody] ProfileEmployeeListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            var employee = (await DataService.GetDtoAsync<ProfileEmployeeListDTO>(x => x.Id == item.UserId)).FirstOrDefault();
           
            try
            {
                item.Id = DataService.Add<UserProfile>(item);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            if (employee != null)
            {
                item.Organization = employee.Organization;
            }

            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> MakeCopy(Guid id)
        {
            var model = (await DataService.GetDtoAsync<ProfileDetailDTO>(x => x.Id == id)).FirstOrDefault();

            if (model == null)
            {
                return NotFound();
            }
            model.Caption = $"{model.Caption} (Copy)";
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeCopy(ProfileDetailDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }
            var savedModel = await _service.MakeCopyAsync(model);
            return View("Details", savedModel);
        }

        [HttpDelete]
        public async Task DeleteProfileEmployee(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<UserProfile>(id);
                await DataService.SaveChangesAsync();
            }
        }

        #endregion

        #region ProfileRoles

        public async Task<IActionResult> ProfileRoles(Guid profileId)
        {
            ViewBag.ProfileId = profileId;

            var roleList = (await DataService.GetDtoAsync<RoleListDTO>()).Select(x => new {roleId = x.Id, Name = x.Caption});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.RoleList = JsonConvert.SerializeObject(roleList, serializerSettings);

            return PartialView("ProfileRolesPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ProfileRoleListDTO>> GetProfileRoles(Guid profileId)
        {
            var data = await DataService.GetDtoAsync<ProfileRoleListDTO>(fr => fr.ProfileId == profileId);
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertProfileRole([FromBody] ProfileRoleListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            var role = (await DataService.GetDtoAsync<RoleListDTO>(x => x.Id == item.RoleId)).FirstOrDefault();

            try
            {
                item.Id = DataService.Add<ProfileRole>(item);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            if (role != null)
            {
                item.IsActive = role.IsActive;
            }

            return Ok(item);
        }

        [HttpDelete]
        public async Task DeleteProfileRole(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<ProfileRole>(id);
                await DataService.SaveChangesAsync();
            }
        }

        #endregion

        #region ProfileRights

        public async Task<IActionResult> ProfileRights(Guid profileId)
        {
            ViewBag.ProfileId = profileId;

            var rightList = (await DataService.GetDtoAsync<RightListDTO>()).Select(x => new {rightId = x.Id, Name = x.Caption});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.RightList = JsonConvert.SerializeObject(rightList, serializerSettings);

            return PartialView("ProfileRightsPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ProfileRightListDTO>> GetProfileRights(Guid profileId)
        {
            var data = await DataService.GetDtoAsync<ProfileRightListDTO>(fr => fr.ProfileId == profileId);
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertProfileRight([FromBody] ProfileRightListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            var right = (await DataService.GetDtoAsync<RightListDTO>(x => x.Id == item.RightId)).FirstOrDefault();

            try
            {
                item.Id = DataService.Add<ProfileRight>(item);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            if (right != null)
            {
                item.IsActive = right.IsActive;
            }

            return Ok(item);
        }

        [HttpDelete]
        public async Task DeleteProfileRight(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<ProfileRight>(id);
                await DataService.SaveChangesAsync();
            }
        }

        #endregion

        #region ProfileOrganizations

        public async Task<IActionResult> ProfileOrganizations(Guid profileId)
        {
            ViewBag.ProfileId = profileId;
            var organizationList = (await DataService.GetDtoAsync<OrgOrganizationListDTO>())
                .Select(x => new {organizationId = x.Id, name = x.Name});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.OrganizationList = JsonConvert.SerializeObject(organizationList, serializerSettings);

            ProfileRowLevelRightDTO entityRls = null;
            {
                try
                {
                    entityRls = await _service.GetOrAddEntityRlsAsync(profileId, nameof(Organization));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewBag.RowLvl = entityRls.Id;
            ViewBag.OrgAccessLevel = entityRls.AccessType;

            return PartialView("ProfileOrganizationsPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ProfileRowLevelRightListDTO>> GetProfileOrganizations(Guid profileId)
        {
            var data = await DataService.GetDtoAsync<ProfileRowLevelRightListDTO>(fr =>
                fr.ProfileId == profileId && fr.EntityName == nameof(Organization));
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertProfileOrganization([FromBody] ProfileRowLevelRightListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            if (item.EntityId == null)
            {
                return BadRequest("Невказано регіон");
            }

            var entitySecure = new RowLevelSecurityObject
            {
                RowLevelRightId = item.Id,
                EntityId = item.EntityId.Value
            };
            try
            {
                item.Id = DataService.Add(entitySecure);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task UpdateLevelOrganizations(Guid profileId, string access)
        {
            await _service.UpdateEntityRowLevelAccessType(profileId, access, "Organization");
        }

        [HttpDelete]
        public void DeleteProfileOrganization(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<RowLevelSecurityObject>(id);
                DataService.SaveChangesAsync();
            }
        }

        #endregion

        #region ProfileRegions

        public async Task<IActionResult> ProfileRegions(Guid profileId)
        {
            ViewBag.ProfileId = profileId;
            var regionList = DataService.GetDto<AtuRegionListDTO>().Select(x => new {regionId = x.Id, name = x.Name});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.RegionList = JsonConvert.SerializeObject(regionList, serializerSettings);

            ProfileRowLevelRightDTO entityRls = null;
            {
                try
                {
                    entityRls = await _service.GetOrAddEntityRlsAsync(profileId, nameof(Region));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewBag.RowLvl = entityRls.Id;
            ViewBag.RegAccessLevel = entityRls.AccessType;

            return PartialView("ProfileRegionsPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<ProfileRowLevelRightListDTO>> GetProfileRegions(Guid profileId)
        {
            var data = await DataService.GetDtoAsync<ProfileRowLevelRightListDTO>(fr =>
                fr.ProfileId == profileId && fr.EntityName == nameof(Region));
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertProfileRegion([FromBody] ProfileRowLevelRightListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            if (item.EntityId==null)
            {
                return BadRequest("Невказано регіон");
            }

            var entitySecure = new RowLevelSecurityObject
            {
                RowLevelRightId = item.Id,
                EntityId = item.EntityId.Value
            };
            try
            {
                item.Id = DataService.Add(entitySecure);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task UpdateLevelRegions(Guid profileId, string access)
        {
            await _service.UpdateEntityRowLevelAccessType(profileId, access, nameof(Region));
        }

        [HttpDelete]
        public async Task DeleteProfileRegion(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<RowLevelSecurityObject>(id);
                await DataService.SaveChangesAsync();
            }
        }
        #endregion
    }
}
