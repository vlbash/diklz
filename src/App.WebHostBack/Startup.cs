using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Helpers;
using App.Business.Infrastructure.Providers;
using App.Business.Infrastructure.Scheduler;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.BranchService;
using App.Business.Services.LimsService;
using App.Business.Services.Common;
using App.Business.Services.ControllerServices;
using App.Business.Services.OperationFormList;
using App.Business.Services.PrlServices;
using App.Business.Services.RedisProvider;
using App.Business.Services.RptServices;
using App.Business.Services.UserPresettings;
using App.Business.Services.UserServices;
using App.Business.Services.ImlServices;
using App.Core.Business;
using App.Core.Business.Extensions;
using App.Core.Business.Filters;
using App.Core.Business.Providers;
using App.Core.Business.Services;
using App.Core.Business.Services.DistributedCacheService;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Common;
using App.Core.Data;
using App.Core.Data.Helpers;
using App.Core.Data.Repositories;
using App.Core.Mvc.Helpers;
using App.Core.Security.Entities;
using App.Data.Contexts;
using App.Data.Repositories;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Business.Services.NotificationServices;
using App.Business.Services.EmailService;
using App.Business.Services.TrlServices;
using App.Core.Common.Services;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace App.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddResponseCaching();
            services.AddMemoryCache();

            // Redis
            services.AddSingleton<IRedisdatabaseProvider, RedisDatabaseProvider>();
            var cacheSetting = Configuration["CacheProvider:Redis"];
            if (cacheSetting == "true")
            {
                services.AddScoped<IDistributedCacheService, DistributedCacheService>();
            }
            else
            {
                services.AddScoped<IDistributedCacheService, DistributedMemoryCacheService>();
            }

            services.AddScoped<ISqlRepositoryHelper, SqlRepositoryHelper>();
            services.AddSingleton<IQueryConditionsHelper, PostgresQueryConditionsBuilder>();
            services.AddScoped(typeof(IEntityRepository<>), typeof(SafeEntityRepository<>));
            services.AddScoped(typeof(IDTORepository<>), typeof(SafeDTORepository<>));

            services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
            services.AddScoped(typeof(IDTOService<>), typeof(DTOService<>));
            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
            services.AddScoped<ICommonDataService, CommonDataService>();
            services.AddScoped<ICommonRepository, SafeCommonRepository>();
            services.AddScoped<IQueryableCacheService, QueryableCacheService>();
            services.AddScoped<IObjectMapper, ObjectMapper>();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SelectListHelper>();

            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped(typeof(ExtendedReport<,>));

            services.AddScoped<UserInfo>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<ApplicationRowLevelRightControllerService>();
            services.AddScoped<RightControllerService>();
            services.AddScoped<ReflectionService>();
            services.AddScoped<ProfileControllerService>();
            services.AddScoped<RoleControllerService>();
            services.AddScoped<IUserPresettingsService, UserPresettingsService>();
            //services.AddScoped<ISearchFilterService, SearchFilterService>();
            services.AddSingleton<MemoryCacheHelper>();

            services.AddSingleton<IRedisdatabaseProvider, RedisDatabaseProvider>();
            services.AddSingleton<IRedisProviderService, RedisProviderService>();

            services.AddScoped<IOperationFormListService, OperationFormListService>();

            services.AddScoped<LicenseService>();

            services.AddScoped<IPrlApplicationService, PrlApplicationService>();
            services.AddScoped<IPrlReportService, PrlReportService>();
            services.AddScoped<IPrlApplicationAltService, PrlApplicationAltService>();
            services.AddScoped<IPrlLicenseService, PrlLicenseService>();
            services.AddScoped<PrlApplicationProcessService>();
            services.AddScoped<PrlContractorService>();
            services.AddScoped<IPrlOrganizationService, PrlOrganizationService>();

            services.AddScoped<ImlApplicationAltService>();
            services.AddScoped<IImlLicenseService, ImlLicenseService>();
            services.AddScoped<IImlOrganizationService, ImlOrganizationService>();
            services.AddScoped<ImlMedicineService>();
            services.AddScoped<ImlApplicationService>();
            services.AddScoped<ImlApplicationProcessService>();

            services.AddScoped<TrlLicenseService>();
            services.AddScoped<ITrlOrganizationService, TrlOrganizationService>();
            services.AddScoped<TrlApplicationProcessService>();
            services.AddScoped<TrlApplicationService>();
            services.AddScoped<ITrlLicenseService,TrlLicenseService>();
            services.AddScoped<TrlApplicationAltService>();

            services.AddScoped<AppAssigneeService>();
            services.AddScoped<LimsExchangeService>();
            services.AddScoped<LimsRepository>();
            services.AddScoped<PrlApplicationProcessService>();
            services.AddScoped<ImlReportService>();
            services.AddScoped<TrlReportService>();
            services.AddScoped<ImlApplicationProcessService>();
            services.AddScoped<PrlContractorService>();
            services.AddScoped(typeof(ApplicationService<>));
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<ISqlRepositoryHelper, SqlRepositoryHelper>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
            services.AddScoped<CoreDbContext>(p => p.GetService<ApplicationDbContext>());
            services.AddScoped<MigrationDbContext>(p => p.GetService<ApplicationDbContext>());
            var limsConnection = Configuration.GetConnectionString("LimsConnection");
            services.AddDbContext<LimsDbContext>(builder => builder.UseSqlServer(limsConnection));
            services.AddScoped<IQueryableCacheService, QueryableCacheService>();
            services.AddScoped<ISearchFilterSettingsService, SearchFilterSettingsService>();
            services.AddScoped<MessageService>();            
            services.AddScoped<AuditService>();
            services.AddScoped<IEntityStateHelper, EntityStateHelper>();
            //services.AddDbContext<AdminToolsDbContext>(options => options.UseNpgsql(connection));
            //services.AddDbContext<SecurityDbContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<ISecurityDbContext, AppSecurityDbContext>(options => options.UseNpgsql(connection));

            services.AddScoped<IMgsReportService, MsgReportService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFeedBackService, FeedBackService>();
            services.AddScoped<ISendEmailService, SendEmailService>();

            services.AddScoped<BackOfficeUserService>();
            services.AddScoped<IAtuAddressService, AtuAddressService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            // Schedule
            services.AddSingleton<IHostedService, ChangesOfPendingRP>();
            services.AddSingleton<IHostedService, ChangesOfPendingSpodu>();

            var scopes = Configuration.GetSection("Identity:Clients:Scopes")
                .GetChildren()
                .ToArray();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.UseTokenLifetime = true;

                    options.Authority = Configuration["Identity:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["RequireHttpsMetadata"]);

                    options.ClientId = Configuration["Identity:Clients:Id"];
                    options.ClientSecret = Configuration["Identity:Clients:Secret"];
                    options.ResponseType = Configuration["Identity:ResponseType"];
                    options.Events = new OpenIdConnectEvents()
                    {
                        OnRedirectToIdentityProvider = p =>
                        {
                            p.Properties.RedirectUri = Configuration["Identity:RedirectUri"];
                            return Task.FromResult(0);
                        }
                    };

                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope.Value);
                    }

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            services.AddMvc(option =>
            {
                option.Filters.Add<DateResultFilterAttribute>();
                option.Filters.Add<AppAuthFilter>();
                option.RegisterDateTimeProvider(services)
                    .ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
                option.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            })
                   //.AddJsonOptions(jsonOption =>
                   //    jsonOption.RegisterJsonDateTimeConverter(services))
                   .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<AppConfig>(opt =>
            {
                var ds = Path.DirectorySeparatorChar;
                opt.CurrentPath = Environment.CurrentDirectory;
                opt.AppDbPath = Path.GetDirectoryName(opt.CurrentPath) + ds + "App.DB" + ds;
                opt.CoreDbPath = Path.GetDirectoryName(Path.GetDirectoryName(opt.CurrentPath)) +
                    ds + "Astum.Core" + ds + "src" + ds + "App.Core.DB" + ds;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("ru"), new CultureInfo("uk") };
                options.DefaultRequestCulture = new RequestCulture("uk", "uk");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Login/Error");
            }

            //app.UseExceptionHandler("/Login/Error");


            app.UseAuthentication();
            app.UseStaticFiles();
            // Add support for node_modules but only during development **temporary**

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                      name: "areas",
                      template: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
