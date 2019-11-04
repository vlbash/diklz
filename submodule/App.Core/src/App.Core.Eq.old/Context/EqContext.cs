using System;
using System.IO;
using App.Core.Eq.Entities;
using App.Core.Eq.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace App.Core.Eq.Context
{
    public class EqContext : DbContext
    {
        #region EQ - Генерація слотів прийомів
        public DbSet<ScheduleTime> ScheduleTime { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleProperty> ScheduleProperties { get; set; }
        public DbSet<ScheduleSlots> Slots { get; set; }
        public DbSet<ScheduleResource> Resources { get; set; }
        public DbSet<ScheduleAppointment> Appointments { get; set; }
        public DbSet<HolidaysDictionary> HolidaysDictionary { get; set; }
        #endregion


        public EqContext(DbContextOptions<EqContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public void ClearTimeTable(EqContext context)
        {
            //var recToDel = context.ScheduleTime.ToList();
            //if(recToDel == null || recToDel.Count == 0) {
            //    return;
            //}
            //context.ScheduleTime.RemoveRange(recToDel);
            //context.SaveChanges();

            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE ScheduleTime");
            //context.SaveChanges();
        }

        public void ClearSlots(EqContext context)
        {
            //var recToDel = context.Slots.ToList();
            //if (recToDel == null || recToDel.Count == 0) {
            //    return;
            //}
            //context.Slots.RemoveRange(recToDel);
            //context.SaveChanges();

            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE Slots");
            //context.SaveChanges();
        }
        public static IConfiguration Configuration { get; set; }

    }

    public class EqContextFactory : IDesignTimeDbContextFactory<EqContext>
    {
        public EqContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string basePath = Environment.GetEnvironmentVariable("DEPLOY_LOCATION") == "Local" ?
                Directory.GetParent(Environment.CurrentDirectory) + "\\App.Host" : Environment.CurrentDirectory;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            var builder = new DbContextOptionsBuilder<EqContext>();

            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            return new EqContext(builder.Options);
        }
    }
}
