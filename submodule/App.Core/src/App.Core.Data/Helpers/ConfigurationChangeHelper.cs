using System.Threading;
using Microsoft.Extensions.Configuration;

namespace App.Core.Data.Helpers
{
    public static class ConfigurationChangeHelper
    {
        #region ApplicationDbSettingsToken

        private static ConfigurationReloadToken _applicationDbSettingsToken = new ConfigurationReloadToken();

        public static ConfigurationReloadToken ApplicationDbSettingsToken => _applicationDbSettingsToken;

        public static void ApplicationDbSettingsTokenOnReload()
        {
            var previousToken = Interlocked.Exchange(ref _applicationDbSettingsToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        #endregion
    }
}
