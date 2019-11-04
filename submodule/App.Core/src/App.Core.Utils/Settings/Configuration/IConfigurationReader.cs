using Serilog.Configuration;

namespace App.Core.Utils.Settings.Configuration
{
    interface IConfigurationReader : ILoggerSettings
    {
        void ApplySinks(LoggerSinkConfiguration loggerSinkConfiguration);
    }
}
