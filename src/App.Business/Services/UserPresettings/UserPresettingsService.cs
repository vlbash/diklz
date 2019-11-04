using System;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Business.Services;
using Microsoft.Extensions.Configuration;




namespace App.Business.Services.UserPresettings
{
    using IUserPresettings = IEntityService<Data.Entities.Common.UserPresettings>;

    public class UserPresettingsService: IUserPresettingsService
    {
        private readonly IConfiguration configuration;
        private readonly IUserPresettings presettingsServ;
        private readonly UserInfo userIdentInfo;

        public UserPresettingsService(IConfiguration configuration, 
            UserInfo userIdentInfo,
            IUserPresettings presettingsServ)
        {
            this.configuration = configuration;
            this.presettingsServ = presettingsServ;
            this.userIdentInfo = userIdentInfo;
        }

        public async Task<string> GetUserPresettings(string journalName)
        {
            var userId = Guid.Parse(userIdentInfo.Id);

            return await Task.Run(() => presettingsServ.GetEntity().SingleOrDefault(x => x.UserId == userId && x.JournalName == journalName)?.PresettingsJson);
        }

        public async Task SetUserPresettings(string journalName, string presettingsJson)
        {
            var userId = Guid.Parse(userIdentInfo.Id);
            var item = presettingsServ.GetEntity().SingleOrDefault(x => x.UserId == userId && x.JournalName == journalName);

            if (item == null)
            {
                item = new Data.Entities.Common.UserPresettings
                    { JournalName = journalName, UserId = userId, PresettingsJson = presettingsJson };
            }
            else
            {
                item.PresettingsJson = presettingsJson;
            }
            await presettingsServ.SaveAsync(item);
        }
    }
}
