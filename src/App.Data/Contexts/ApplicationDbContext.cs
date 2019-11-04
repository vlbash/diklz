using App.Core.Business.Services;
using App.Core.Data.Helpers;
using App.Core.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace App.Data.Contexts
{
    public class ApplicationDbContext: MigrationDbContext
    {
        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IUserInfoService userInfoService,
            MemoryCacheHelper cacheHelper,
            IQueryConditionsHelper condtionsHelper,
            IMemoryCache memoryCache) : base(options)
        {
            UserInfo = userInfoService.GetCurrentUserInfo();
            //CacheHelper = cacheHelper;
            //QueryConditionsHelper = condtionsHelper;
           // InitializeRights();
        }

        private void InitializeRights()
        {
            UserInfo.Rights = new UserApplicationRights
            {
                HasFullRights = true
            };
        }
    }
}
