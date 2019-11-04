using App.Data.DTO.LIMS;
using App.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Contexts
{
    public class LimsDbContext: DbContext
    {
        public DbQuery<LimsCheck> LimsChecks { get; set; }
        public DbQuery<LimsEndLicCheck> LimsEndLicChecks { get; set; }
        public DbQuery<LimsNotice> LimsNotices { get; set; }
        public DbQuery<LimsApp> LimsApp { get; set; }
        public DbQuery<LimsProtocol> LimsProtocols { get; set; }
        public DbQuery<LimsPendingChanges> PendingChangeses { get; set; }
        public DbQuery<LicenseLIMS> Licenses { get; set; }
        public DbQuery<LimsOldRP> LimsOldRP { get; set; }
        public DbQuery<LimsSpodu> LimsSpodu { get; set; }
        public LimsDbContext(DbContextOptions<LimsDbContext> options): base(options) { }
    }
}
