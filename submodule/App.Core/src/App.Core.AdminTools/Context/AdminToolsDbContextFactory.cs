using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace App.Core.AdminTools.Context
{
    public class AdminToolsDbContextFactory : IDesignTimeDbContextFactory<AdminToolsDbContext>
    {
        public AdminToolsDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var basePath = Environment.GetEnvironmentVariable("DEPLOY_LOCATION").Equals("Local") ?
                Directory.GetParent(Environment.CurrentDirectory) + "\\App.WebHost" :
                Environment.CurrentDirectory;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AdminToolsDbContext>();

            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            return new AdminToolsDbContext(builder.Options, null, null);
        }
    }

    public static class AdminToolsDbInitializer
    {
        public static void InitializeEq(AdminToolsDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
