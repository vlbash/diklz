using System;
using System.Globalization;
using System.IO;
using App.Core.Business.Filters;
using App.Core.Common;
using App.WebApi.Contexts;
using App.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace App.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddMemoryCache();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("ru"), new CultureInfo("uk") };
                options.DefaultRequestCulture = new RequestCulture("uk", "uk");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddDbContext<APIContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IHostedService, PeriodBackgroundService>();
            services.AddTransient<ILicenseJsonSerializeService, LicenseJsonSerializeService>();
            
            services.Configure<AppConfig>(opt =>
            {
                var ds = Path.DirectorySeparatorChar;
                opt.CurrentPath = Environment.CurrentDirectory;
                opt.AppDbPath = Path.GetDirectoryName(opt.CurrentPath) + ds + "App.DB" + ds;
                opt.CoreDbPath = Path.GetDirectoryName(Path.GetDirectoryName(opt.CurrentPath)) +
                                 ds + "Astum.Core" + ds + "src" + ds + "App.Core.DB" + ds;
            });
            
            services.AddMvc(option =>
            {
                option.Filters.Add<DateResultFilterAttribute>(); //TODO !!
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IMongoRepository, MongoRepository>();
            services.AddScoped<MongoDbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
