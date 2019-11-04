using System.Collections.Generic;
using App.Core.Business.Services;
using App.Core.Business.Services.DistributedCacheService;
using App.Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace App.Business.Services.UserServices
{
    public class UserDiklzInfoService: UserInfoService
    {
        public UserDiklzInfoService(ISecurityDbContext dbcontext, IDistributedCacheService cacheService, IHttpContextAccessor httpContextAccessor, IConfiguration confguration)
            :base(dbcontext, cacheService, httpContextAccessor, confguration){}

        protected override (string userId, Dictionary<string, string> loginData) GetCurrentLoginData()
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            var lastName = httpContext?.User?.FindFirst("lastname")?.Value;
            var inn = httpContext?.User?.FindFirst("drfocode")?.Value;
            var id = inn + lastName;

            return (id, new Dictionary<string, string> { { "LastName", lastName } });
        }
    }
}
