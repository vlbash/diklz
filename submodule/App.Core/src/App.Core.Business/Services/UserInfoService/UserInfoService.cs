using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Core.Business.Services.DistributedCacheService;
using App.Core.Data.Helpers;
using App.Core.Security;
using App.Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace App.Core.Business.Services
{
    public class UserInfoService: IUserInfoService
    {
        protected readonly IDistributedCacheService CacheService;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly ISecurityDbContext DbContext;
        protected readonly IConfiguration Configuration;
        private UserInfo _userInfo;
        protected readonly string CacheSuffix = "_userInfo";
        protected readonly string CacheRightsSuffix = "_userRights";

        public UserInfoService(ISecurityDbContext dbcontext, IDistributedCacheService cacheService, IHttpContextAccessor httpContextAccessor, IConfiguration confguration)
        {
            CacheService = cacheService;
            HttpContextAccessor = httpContextAccessor;
            DbContext = dbcontext;
            Configuration = confguration;
        }

        public UserInfo GetUserInfo(string userId)
        {
            var userInfo = GetUserInfoFromCache(userId);
            if (userInfo == null)
            {
                return new UserInfo();
            }

            return Clone(userInfo);
        }

        public virtual async Task<UserInfo> GetUserInfoAsync(string userId)
        {
            return await Task.FromResult(GetUserInfo(userId));
        }

        public virtual UserInfo GetCurrentUserInfo()
        {
            var (userId, loginData) = GetCurrentLoginData();

            if (string.IsNullOrEmpty(userId))
            {
                return new UserInfo();
            }

            var userInfo = GetUserInfoFromCache(userId, true);
            if (string.IsNullOrEmpty(userInfo?.Id))
            {
                userInfo = new UserInfo
                {
                    Id = userId,
                    LoginData = loginData,
                    UserCultureInfo = new UserCultureInfo()
                };
            }

            return Clone(userInfo);
        }

        public virtual async Task<UserInfo> GetCurrentUserInfoAsync()
        {
            return await Task.FromResult(GetCurrentUserInfo());
        }

        public virtual bool UpdateUserInfo(UserInfo userInfo)
        {
            return SaveUserInfoToCache(userInfo);
        }

        public virtual async Task<bool> UpdateUserInfoAsync(UserInfo userInfo)
        {
            return await Task.FromResult(UpdateUserInfo(userInfo));
        }

        public virtual void DeleteCurrentUserInfo()
        {
            var (userId, _) = GetCurrentLoginData();
            if (!string.IsNullOrEmpty(userId))
            {
                var rightsCacheKey = userId + CacheRightsSuffix;
                CacheService.ClearKey(rightsCacheKey);

                var cacheKey = userId + CacheSuffix;
                CacheService.ClearKey(cacheKey);
            }
            _userInfo = null;
        }

        public virtual void DeleteUserInfo(string userId)
        {
            CacheService.ClearKey(userId);
        }

        protected virtual UserInfo GetUserInfoFromCache(string userId, bool isCurrentUser = false)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            UserInfo userInfo = null;
            if (isCurrentUser)
            {
                userInfo = _userInfo;
            }

            if (userInfo == null)
            {
                userInfo = CacheService.GetValue<UserInfo>(userId + CacheSuffix);
                if (isCurrentUser)
                {
                    _userInfo = userInfo;
                }
            }

            if (userInfo == null)
            {
                return userInfo;
            }

            // rights are stored separately from user
            if (userInfo?.Rights == null)
            {
                userInfo.Rights = CacheService.GetValue<UserApplicationRights>(userId + CacheRightsSuffix);
            }

            if (UpdateUserRights(userInfo))
            {
                UpdateUserInfo(userInfo);
            }

            return userInfo;
        }

        protected virtual bool SaveUserInfoToCache(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.Id))
            {
                return false;
            }

            var rightsCacheDuration = Configuration.GetValue<int?>("Caching:Rights");
            TimeSpan? expiry = null;
            if (rightsCacheDuration.HasValue)
            {
                expiry = TimeSpan.FromSeconds(rightsCacheDuration.Value);
            }

            var rightsCacheKey = userInfo.Id + CacheRightsSuffix;
            CacheService.SetValue(rightsCacheKey, userInfo.Rights, expiry);

            var userInfoCacheDuration = Configuration.GetValue<int?>("Caching:UserInfo");
            expiry = null;
            if (userInfoCacheDuration.HasValue)
            {
                expiry = TimeSpan.FromSeconds(userInfoCacheDuration.Value);
            }
            var cacheKey = userInfo.Id + CacheSuffix;
            // should save user without rights, because rights stored separately with own invalidation time
            var clone = Clone(userInfo);
            clone.Rights = null;
            CacheService.SetValue(cacheKey, clone, expiry);

            return true;
        }

        protected virtual (string userId, Dictionary<string, string> loginData) GetCurrentLoginData()
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            var userLogin = httpContext?.User?.FindFirst("name")?.Value;
            var id = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var loginData = new Dictionary<string, string>
            {
                ["UserLogin"] = userLogin
            };

            return (id, loginData);
        }

        private bool UpdateUserRights(UserInfo userInfo)
        {
            if (userInfo == null || userInfo.Rights != null || userInfo.ProfileId == Guid.Empty)
            {
                return false;
            }

            userInfo.Rights = DbContext?.GetUserRights(userInfo.ProfileId, userInfo.UserId);

            return true;
        }

        private UserInfo Clone(UserInfo userInfo)
        {
            var clone = new UserInfo
            {
                Id = userInfo.Id,
                LoginData = userInfo.LoginData,
                UserId = userInfo.UserId,
                PersonId = userInfo.PersonId,
                ProfileId = userInfo.ProfileId,
                UserCultureInfo = userInfo.UserCultureInfo
            };

            clone.Rights = userInfo.Rights;

            return clone;
        }
    }
}
