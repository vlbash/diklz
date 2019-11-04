using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace App.Data.Contexts
{
    public class MigrationDbContextFactory: IDesignTimeDbContextFactory<MigrationDbContext>
    {
        public MigrationDbContext CreateDbContext(string[] args) {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var basePath = Environment.GetEnvironmentVariable("DEPLOY_LOCATION").Equals("Local") ?
                Directory.GetParent(Environment.CurrentDirectory).ToString() + Path.DirectorySeparatorChar + "App.WebHost" :
                Environment.CurrentDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MigrationDbContext>();
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new MigrationDbContext(builder.Options);
        }
    }
}
