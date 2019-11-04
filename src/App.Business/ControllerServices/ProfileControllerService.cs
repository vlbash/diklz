using System;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Common.Services;
using App.Core.Data;
using App.Core.Security;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;

namespace App.Business.Services.ControllerServices
{
    public class ProfileControllerService
    {
        private readonly ReflectionService _reflectionService;
        private readonly CoreDbContext _context;
        public ICommonDataService DataService { get; }

        public ProfileControllerService(ICommonDataService dataService, CoreDbContext context,
            ReflectionService reflectionService)
        {
            DataService = dataService;
            _context = context;
            _reflectionService = reflectionService;
        }

        public async Task<ProfileDetailDTO> MakeCopyAsync(ProfileDetailDTO model)
        {
            var newModel = new ProfileDetailDTO
            {
                Caption = model.Caption,
                IsActive = model.IsActive
            };
            var newId = DataService.Add<Profile>(newModel);
            var profRoleList = await DataService.GetDtoAsync<ProfileRoleListDTO>(x => x.ProfileId == model.Id);

            foreach (var profRole in profRoleList)
            {
                DataService.Add<ProfileRole>(new ProfileRoleListDTO
                {
                    ProfileId = newId,
                    RoleId = profRole.RoleId
                });
            }

            await DataService.SaveChangesAsync();

            var profRightList = await DataService.GetDtoAsync<ProfileRightListDTO>(c => c.ProfileId == model.Id);

            foreach (var profRight in profRightList)
            {
                DataService.Add<ProfileRight>(new ProfileRightListDTO()
                {
                    ProfileId = newId,
                    RightId = profRight.RightId
                });
            }

            await DataService.SaveChangesAsync();

            var rowLevelRightList =
                await DataService.GetDtoAsync<ProfileRowLevelRightDTO>(x => x.ProfileId == model.Id);

            foreach (var rowLevelRight in rowLevelRightList)
            {
                var rowLvlRightId = DataService.Add<RowLevelRight>(new ProfileRowLevelRightDTO

                {
                    ProfileId = newId,
                    EntityName = rowLevelRight.EntityName,
                    AccessType = rowLevelRight.AccessType
                });
                var rowLvlSecureObjList = await DataService.GetDtoAsync<ProfileRowLevelRightListDTO>(x =>
                    x.ProfileId == rowLevelRight.ProfileId
                    && x.EntityName == rowLevelRight.EntityName);

                foreach (var rowLvlSecureObj in rowLvlSecureObjList)
                {
                    if (rowLvlSecureObj.EntityId != null && rowLvlSecureObj.SecurId != null)
                    {
                        DataService.Add(new RowLevelSecurityObject
                        {
                            EntityId = rowLvlSecureObj.EntityId.Value,
                            RowLevelRightId = rowLvlRightId
                        });
                    }
                }
            }

            await DataService.SaveChangesAsync();
            var savedModel = (await DataService.GetDtoAsync<ProfileDetailDTO>(x => x.Id == newId)).FirstOrDefault();
            return savedModel;
        }

        public async Task<ProfileRowLevelRightDTO> GetOrAddEntityRlsAsync(Guid profileId, string entityName)
        {
            var rowLvl = Guid.Empty;
            var entityRls = (await DataService.GetDtoAsync<ProfileRowLevelRightDTO>(
                        x => x.ProfileId == profileId && x.EntityName == entityName))
                    .FirstOrDefault();

            if (entityRls == null)
            {
                entityRls = new ProfileRowLevelRightDTO
                {
                    ProfileId = profileId,
                    EntityName = entityName,
                    AccessType = RowLevelAccessType.Default
                };
                entityRls.Id = DataService.Add<RowLevelRight>(entityRls);
                await DataService.SaveChangesAsync();
            }

            return entityRls;
        }

        public async Task UpdateEntityRowLevelAccessType(Guid profileId, string access, string entityName)
        {
            var row = DataService
                .GetEntity<RowLevelRight>(x => x.ProfileId == profileId && x.EntityName.Equals(entityName))
                .FirstOrDefault();
            if (row != null)
            {
                DataService.Add(row);

                if (access == "Specified")
                {
                    row.AccessType = RowLevelAccessType.Specified;
                }
                if (access == "All")
                {
                    row.AccessType = RowLevelAccessType.All;
                }
                if (access == "Default")
                {
                    row.AccessType = RowLevelAccessType.Default;
                }

                await DataService.SaveChangesAsync();
            }
        }

    }
}
