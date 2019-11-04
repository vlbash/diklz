using App.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Contexts
{
    internal class APIContext : DbContext
    {
        public APIContext(DbContextOptions options) :base(options)
        {
        }

        public DbQuery<License> Licenses { get; set; }
    }
}
