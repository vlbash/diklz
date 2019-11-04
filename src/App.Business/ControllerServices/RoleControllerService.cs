using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;

namespace App.Business.Services.ControllerServices
{
    public class RoleControllerService
    {
        public ICommonDataService DataService { get; }

        public RoleControllerService(ICommonDataService dataService)
        {
            DataService = dataService;
        }

        public async Task<RoleListDTO> GetSavedModelAsync(RoleListDTO model)
        {
            var newModel = new RoleListDTO
            {
                Caption = model.Caption,
                IsActive = model.IsActive
            };
            var newId = DataService.Add<Role>(newModel);
            //await DataService.SaveChangesAsync();

            var roleRightList = await DataService.GetDtoAsync<RoleRightListDTO>(x => x.RoleId == model.Id);

            foreach (var roleRight in roleRightList)
            {
                DataService.Add<RoleRight>(new RoleRightListDTO()
                {
                    RoleId = newId,
                    RightId = roleRight.RightId
                });
            }

            await DataService.SaveChangesAsync();

            var savedModel = (await DataService.GetDtoAsync<RoleListDTO>(x => x.Id == newId)).FirstOrDefault();

            return savedModel;
        }
    }
}
