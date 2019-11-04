using System;
using System.Globalization;
using System.IO;
using System.Linq;
using App.Business.CryptoSignMiddleware;
using App.Business.Helpers;
using App.Business.Infrastructure.Providers;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.BranchService;
using App.Business.Services.Common;
using App.Business.Services.LimsService;
using App.Business.Services.OperationFormList;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Business.Services.EmailService;
using App.Business.Services.ImlServices;
using App.Core.Data;
using App.Data.Contexts;
using App.Business.Services.NotificationServices;
using App.Business.Services.P902Services;
using App.Core.Business;
using App.Core.Business.Services;
using App.Core.Common;
using App.Core.Data.Helpers;
using App.Core.Data.Repositories;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Core.Business.Filters;
using App.Business.Services.UserPresettings;
using App.Business.Services.Token;
using App.Business.Services.UserServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using App.Core.Security.Entities;
using App.Business.Services.RedisProvider;
using App.Business.Services.TrlServices;
using App.Core.Business.Providers;
using App.Core.Business.Services.DistributedCacheService;
using App.Core.Business.Services.ObjectMapper;
using App.Host.Filters;
using App.Core.Mvc.Helpers;
using App.Data.Repositories;

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

            // add IQueryConditionsHelper before DTORepository
            services.AddSingleton<IQueryConditionsHelper, PostgresQueryConditionsBuilder>();
            services.AddScoped(typeof(IEntityRepository<>),typeof(EntityRepository<>));
            services.AddScoped(typeof(IDTORepository<>), typeof(SafeDTORepository<>));
            services.AddScoped<ICommonRepository, SafeCommonRepository>();
            services.AddScoped<IObjectMapper, ObjectMapper>();

            services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
            services.AddScoped(typeof(IDTOService<>), typeof(DTOService<>));
            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
            services.AddScoped<ICommonDataService, CommonDataService>();

            #region ControllerServices

            services.AddScoped<AppAssigneeService>();
            services.AddScoped<PrlContractorService>();

            #endregion


            #region APP
            services.AddScoped(typeof(ITokenService), typeof(TokenService));

            #endregion

            services.AddSingleton<IQueryConditionsHelper, PostgresQueryConditionsBuilder>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SelectListHelper>();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            var limsConnection = Configuration.GetConnectionString("LimsConnection");

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<LimsDbContext>(builder => builder.UseSqlServer(limsConnection));

            services.AddScoped(typeof(ExtendedReport<,>));

            services.AddScoped<ProxyHandlerFilter>();

            services.AddScoped<UserInfo>();
            services.AddScoped<IUserInfoService, UserDiklzInfoService>();
            services.AddScoped<IUserPresettingsService, UserPresettingsService>();
            //services.AddScoped<ISearchFilterService, SearchFilterService>();
            services.AddSingleton<MemoryCacheHelper>();
            services.AddSingleton<IRedisdatabaseProvider, RedisDatabaseProvider>();
            services.AddScoped<IOperationFormListService, OperationFormListService>();

            var cacheSetting = Configuration["CacheProvider:Redis"];
            if (cacheSetting == "true")
            {
                services.AddScoped<IDistributedCacheService, DistributedCacheService>();
            }
            else
            {
                services.AddScoped<IDistributedCacheService, DistributedMemoryCacheService>();
            }
            services.AddSingleton<IRedisProviderService, RedisProviderService>();


            services.AddScoped<IPrlApplicationService, PrlApplicationService>();

            services.AddScoped<IPrlReportService, PrlReportService>();
            services.AddScoped<IMgsReportService, MsgReportService>();
            // services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ISendEmailService, SendEmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFeedBackService, FeedBackService>();

            services.AddScoped<ImlApplicationService>();
            services.AddScoped<ImlMedicineService>();
            services.AddScoped<PrlApplicationProcessService>();
            services.AddScoped<LicenseService>();
            services.AddScoped<IPrlLicenseService, PrlLicenseService>();
            services.AddScoped<IImlLicenseService, ImlLicenseService>();
            services.AddScoped<TrlLicenseService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IPrlApplicationAltService, PrlApplicationAltService>();
            services.AddScoped<ISqlRepositoryHelper, SqlRepositoryHelper>();
            services.AddScoped<CoreDbContext>(p => p.GetService<ApplicationDbContext>());
            services.AddScoped<MigrationDbContext>(p => p.GetService<ApplicationDbContext>());
            services.AddScoped<IQueryableCacheService, QueryableCacheService>();
            services.AddScoped<ISearchFilterSettingsService, SearchFilterSettingsService>();
            services.AddScoped<MessageService>();
            services.AddScoped<AuditService>();
            //services.AddDbContext<AdminToolsDbContext>(options => options.UseNpgsql(connection));
            //services.AddDbContext<SecurityDbContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<ISecurityDbContext, AppSecurityDbContext>(options => options.UseNpgsql(connection));
            services.AddScoped<IEntityStateHelper, EntityStateHelper>();
            services.AddScoped<ResultInputControlService>();
            services.AddScoped<AppConclusionServise>();
            services.AddScoped<IAtuAddressService, AtuAddressService>();
            services.AddScoped<IAtuImportService, AtuImportService>();
            services.AddScoped(typeof(ApplicationService<>));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<ImlApplicationAltService>();
            services.AddScoped<ImlReportService>();
            services.AddScoped<TrlReportService>();
            services.AddScoped<TrlApplicationService>();
            services.AddScoped<TrlApplicationAltService>();
            services.AddScoped<ITrlLicenseService, TrlLicenseService>();
            services.AddScoped<LimsExchangeService>();
            #region Lims services

            services.AddScoped<LimsRepository>();
            services.AddScoped<LimsExchangeService>();

            #endregion

            var scopes = Configuration.GetSection("Identity:Clients:Scopes")
                .GetChildren()
                .ToArray();

//            services.AddAuthentication(options =>
//                {
//                    options.DefaultScheme = "Cookies";
//                    options.DefaultChallengeScheme = "oidc";
//                })
//                .AddCookie("Cookies")
//                .AddOpenIdConnect("oidc", options =>
//                {
//                    options.SignInScheme = "Cookies";
//                    options.UseTokenLifetime = true;
//
//                    options.Authority = Configuration["Identity:Authority"];
//                    options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["RequireHttpsMetadata"]);
//
//                    options.ClientId = Configuration["Identity:Clients:Id"];
//                    options.ClientSecret = Configuration["Identity:Clients:Secret"];
//                    options.ResponseType = Configuration["Identity:ResponseType"];
//                    options.Events = new OpenIdConnectEvents()
//                    {
//                        OnRedirectToIdentityProvider = p =>
//                        {
//                            p.Properties.RedirectUri = Configuration["Identity:RedirectUri"];
//                            return Task.FromResult(0);
//                        }
//                    };
//
//                    foreach (var scope in scopes) {
//                        options.Scope.Add(scope.Value);
//                    }
//
//                    options.SaveTokens = true;
//                    options.GetClaimsFromUserInfoEndpoint = true;
//                });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = new PathString("/home/info");
                        options.AccessDeniedPath = new PathString("/auth/denied");
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Registered",
                    policy => policy.Requirements.Add(new DiklzAuthorizeAttribute()));
            });

            services.AddMvc(option =>
            {
                option.Filters.Add<DateResultFilterAttribute>(); //TODO !!
                //option.RegisterDateTimeProvider(services)
                //    .ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
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
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            // Add support for node_modules but only during development **temporary**

            app.UseCookiePolicy();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                      name: "areas",
                      template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute("evaluation", "evaluation", new { controller = "UserArea", action = "SiteEvaluation" });
            });
        }
    }
}
