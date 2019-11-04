using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace App.WebApi.Services
{
    internal class PeriodBackgroundService: BackgroundService
    {
        private readonly IConfiguration _configuration;
        public IServiceScopeFactory _serviceScopeFactory;

        public PeriodBackgroundService(
            IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("PeriodBackgroundService is starting.");

            stoppingToken.Register(() =>
                Log.Information(" PeriodBackgroundService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mongoDbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();
                    IList<string> loggingList = new List<string>();
                    loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Update is starting");
                    try
                    {
                        Log.Information("Update started");

                        var licenseSerService =
                            scope.ServiceProvider.GetRequiredService<ILicenseJsonSerializeService>();

                        Log.Information("Delete started");
                        try
                        {
                            mongoDbService.DeleteLicenses();
                        }

                        catch
                        {
                            Log.Information("Delete failed");
                        }

                        var licenses = licenseSerService.GetLicenses(loggingList);

                        try
                        {
                            loggingList.Add(
                                $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Starting insert and update to MongoDB");
                            mongoDbService.SaveLicenses(licenses);
                            loggingList.Add(
                                $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Insert and update to MongoDB done");
                        }
                        catch (Exception e)
                        {
                            loggingList.Add(
                                $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Insert and update to MongoDB failed");
                            throw;
                        }
                        loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Update succeeded");
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Update failed!");
                        loggingList.Add($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: Licenses update failed");
                    }
                    finally
                    {
                        try
                        {
                            await mongoDbService.InsertLogs(loggingList);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Inserting logs in the MongoDb has failed!");
                        }
                    }
                }

                var frequency = int.TryParse(_configuration["UpdateFrequencyHours"], out var i) ? i : 1;

                await Task.Delay(TimeSpan.FromHours(frequency), stoppingToken);
            }
        }
    }
}
