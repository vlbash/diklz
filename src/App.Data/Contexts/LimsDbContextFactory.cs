using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace App.Data.Contexts
{
    public class LimsDbContextFactory: IDesignTimeDbContextFactory<LimsDbContext>
    {
        public LimsDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var basePath = Environment.GetEnvironmentVariable("DEPLOY_LOCATION").Equals("Local")
                ? Directory.GetParent(Environment.CurrentDirectory) + "\\App.Host"
                : Environment.CurrentDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<LimsDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("LimsConnection"));

            return new LimsDbContext(builder.Options);
        }
    }
}
