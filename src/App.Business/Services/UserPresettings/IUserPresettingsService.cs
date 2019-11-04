using System.Threading.Tasks;

namespace App.Business.Services.UserPresettings
{
    public interface IUserPresettingsService
    {
        Task<string> GetUserPresettings(string journalName);
        Task SetUserPresettings(string journalName, string presettingsJson);
    }
}
