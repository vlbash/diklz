using App.Core.Data;
using App.Core.Eq.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Eq.Data.Contexts
{
    public class EqContext: CoreDbContext
    {
        public EqContext(DbContextOptions<EqContext> options) : base(options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
