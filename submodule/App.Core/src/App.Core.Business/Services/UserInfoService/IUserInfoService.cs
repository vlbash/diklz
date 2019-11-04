using System.Threading.Tasks;

namespace App.Core.Business.Services
{
    public interface IUserInfoService
    {
        UserInfo GetUserInfo(string userId);
        Task<UserInfo> GetUserInfoAsync(string userId);

        UserInfo GetCurrentUserInfo();
        Task<UserInfo> GetCurrentUserInfoAsync();

        bool UpdateUserInfo(UserInfo userInfo);

        Task<bool> UpdateUserInfoAsync(UserInfo userInfo);

        void DeleteCurrentUserInfo();

        void DeleteUserInfo(string userId);
    }
}
