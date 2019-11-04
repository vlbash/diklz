using App.Core.Data.Extensions;
using App.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Data.Contexts
{
    public class AppSecurityDbContext: SecurityDbContext
    {
        private readonly IConfiguration _configuration;

        public AppSecurityDbContext() : base() { }

        public AppSecurityDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var useSnakeCase = _configuration.GetValue<bool>("DataBaseOptions:UseSnakeCase");
            if (useSnakeCase)
            {
                builder.UseSnakeCaseNaming();
            }
        }
    }
}
