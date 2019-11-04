using App.Core.Utils.Settings.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace App.WebApi
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            ConfigStore.Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json",
                    true,
                    true)
                .Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(ConfigStore.Configuration)
                .CreateLogger();

            var host = BuildWebHost(args);
            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(Log.Logger)
                .UseUrls(ConfigStore.Configuration.GetValue<string>("Urls"))
                .Build();
        }
    }
}
