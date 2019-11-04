using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace App.Core.Utils.Logger
{
    public static class SerilogManager
    {
        public static Serilog.Core.Logger Config()
        {
//            //var logName = $"{DateTime.Now:yyyy-MM-dd}"; //TODO: add custom file name for log
//            var minLogLevel = LogEventLevel.Debug;
//            try
//            {
//                minLogLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), ConfigManager.Values["MinimumLogLevel"]);
//            }
//            catch (Exception ex)
//            {
//                Serilog.Log.Logger.Error($"Getting MinimumLogLevel error : {ex.Message}");
//            }
//            var levelSwitch = new LoggingLevelSwitch { MinimumLevel = minLogLevel};
//            var logger = new LoggerConfiguration()
//                .Enrich.FromLogContext()
//                .MinimumLevel.ControlledBy(levelSwitch)
//                //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // TODO : !
//                //.MinimumLevel.Override("System", LogEventLevel.Error)
//#if DEBUG
//                .WriteTo.Console()
//#endif
//                .WriteTo.RollingFile($"{ConfigManager.Values["LogPath"]}\\{ConfigManager.Values["AppName"]}.log")
//                .CreateLogger();


            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Console()
#endif
      //          .WriteTo.RollingFile($"{ConfigManager.Values["LogPath"]}\\{ConfigManager.Values["AppName"]}.log")
                .CreateLogger();

            return logger;
        }
    }
}
