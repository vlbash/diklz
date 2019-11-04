using System;
using App.Data.Contexts;
using App.Core.Utils.Settings.Configuration;
using App.PublicHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace App.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigStore.Configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true, reloadOnChange: true)
                        .Build();

            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(ConfigStore.Configuration)
              .CreateLogger();

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    if (bool.Parse(ConfigStore.Configuration.GetSection("SeedDB").Value))
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();
                        DbInitializer.InitializeDictionaries(context);
                        DbInitializer.Initialize(context);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
                  Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                      //.UseContentRoot(Directory.GetCurrentDirectory())
                      .UseStartup<Startup>()
                      .UseSerilog(Log.Logger)
                      .UseUrls(ConfigStore.Configuration.GetValue<string>("Urls"))
                      .Build();
    }
}
