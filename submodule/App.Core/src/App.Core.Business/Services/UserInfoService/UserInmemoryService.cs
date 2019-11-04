using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Core.Data.Helpers;
using App.Core.Security;
using App.Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace App.Core.Business.Services
{
    public class UserInMemoryService: IUserInfoService
    {
        protected readonly IMemoryCache _cache;
        protected readonly HttpContext _httpContext;
        protected readonly ISecurityDbContext _dbContext;
        protected readonly string _cacheSuffix = "_userInfo";
        protected readonly string _cacheRightsSuffix = "_userRights";
        public int RightsInvalidationTime { get; } = 0;

        public UserInMemoryService(ISecurityDbContext dbcontext, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration confguration)
        {
            _cache = cache;
            _httpContext = httpContextAccessor?.HttpContext;
            _dbContext = dbcontext;

            var rightsInvalidationTime = confguration.GetValue<int>("Rights:InvalidationTime");
            if (rightsInvalidationTime > 0)
            {
                RightsInvalidationTime = rightsInvalidationTime;
            }
        }

        public UserInfo GetUserInfo(string userId)
        {
            var userInfo = GetUserInfoFromCache(userId);
            if (userInfo == null)
            {
                userInfo = new UserInfo
                {
                    Id = userId,
                    UserCultureInfo = new UserCultureInfo()
                };
            }

            if (UpdateUserRights(userInfo))
            {
                UpdateUserInfo(userInfo);
            }

            return userInfo;
        }

        public async virtual Task<UserInfo> GetUserInfoAsync(string userId)
        {
            return await Task.FromResult(GetUserInfo(userId));
        }

        public virtual UserInfo GetCurrentUserInfo()
        {
            var userLogin = _httpContext?.User?.FindFirst("name")?.Value;
            var userId = _httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new UserInfo();
            }

            var userInfo = GetUserInfoFromCache(userId);
            if (userInfo == null)
            {
                userInfo = new UserInfo
                {
                    Id = userId,
                    LoginData = new Dictionary<string, string> { { "UserLogin", userLogin } },
                    UserCultureInfo = new UserCultureInfo()
                };
                SaveUserInfoToCache(userInfo);
            }

            if (UpdateUserRights(userInfo))
            {
                UpdateUserInfo(userInfo);
            }

            return userInfo;
        }

        public async Task<UserInfo> GetCurrentUserInfoAsync()
        {
            return await Task.FromResult(GetCurrentUserInfo());
        }

        public virtual bool UpdateUserInfo(UserInfo userInfo)
        {
            return SaveUserInfoToCache(userInfo);
        }

        public async Task<bool> UpdateUserInfoAsync(UserInfo userInfo)
        {
            return await Task.FromResult(UpdateUserInfo(userInfo));
        }

        protected virtual UserInfo GetUserInfoFromCache(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            UserInfo cachedUserInfo = null;
            var cacheKey = userId + _cacheSuffix;
            if (_cache != null && _cache.TryGetValue(cacheKey, out var userInfoObj))
            {
                cachedUserInfo = userInfoObj as UserInfo;
            }

            // rights are stored separately from user
            if (cachedUserInfo != null)
            {
                var rightsCacheKey = userId + _cacheRightsSuffix;
                _cache.TryGetValue<UserApplicationRights>(rightsCacheKey, out UserApplicationRights rights);
                cachedUserInfo.Rights = rights;
            }

            return cachedUserInfo;
        }

        protected virtual bool SaveUserInfoToCache(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.Id))
            {
                return false;
            }

            var rightsCacheKey = userInfo.Id + _cacheRightsSuffix;
            if (RightsInvalidationTime > 0)
            {
                _cache.Set(rightsCacheKey, userInfo.Rights,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(RightsInvalidationTime)));
            } else
            {
                _cache.Set(rightsCacheKey, userInfo.Rights);
            }
            
            var user = new UserInfo
            {
                Id = userInfo.Id,
                LoginData = userInfo.LoginData,
                UserId = userInfo.UserId,
                PersonId = userInfo.PersonId,
                ProfileId = userInfo.ProfileId,
                UserCultureInfo = userInfo.UserCultureInfo
            };

            var cacheKey = userInfo.Id + _cacheSuffix;
            _cache.Set(cacheKey, user);

            return true;
        }

        private bool UpdateUserRights(UserInfo userInfo)
        {
            if (userInfo?.Rights != null || userInfo.ProfileId == Guid.Empty)
            {
                return false;
            }

            userInfo.Rights = _dbContext?.GetUserRights(userInfo.ProfileId, userInfo.UserId);
            //if (userInfo.Id == "b70082cc-309d-4e2c-8456-30cefab089d9")
            //{
            //    userInfo.Rights.HasFullRights = true;
            //}

            return true;
        }

        public void DeleteCurrentUserInfo()
        {
            throw new NotImplementedException();
        }

        public void DeleteUserInfo(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
