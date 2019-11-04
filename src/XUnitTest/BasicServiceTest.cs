using System;
using System.IO;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Common;
using App.Core.Data;
using App.Core.Data.Helpers;
using App.Core.Data.Repositories;
using App.Data.Contexts;
using App.Data.DTO.LOG;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;
using Z.EntityFramework.Plus;

namespace App.xUnitTests
{
    public class BasicServiceTest
    {
        internal readonly ITestOutputHelper output;

        public static IConfigurationRoot Configuration { get; set; }
        public static Mock<IOptions<AppConfig>> mockCfg { get; set; }
        public static CoreDbContext _context { get; set; }
        public static ICommonDataService DataService { get; set; }

        public BasicServiceTest(ITestOutputHelper _output)
        {
            output = _output;

            mockCfg = new Mock<IOptions<AppConfig>>();

            var ds = Path.DirectorySeparatorChar;
            var ac = new AppConfig();

            ac.CurrentPath = Environment.CurrentDirectory + "/../../../../../../src/App.WebHost";
            ac.AppDbPath = Path.GetDirectoryName(ac.CurrentPath) + ds + "App.DB" + ds;
            ac.CoreDbPath = Path.GetDirectoryName(Path.GetDirectoryName(ac.CurrentPath)) +
                            ds + "Astum.Core" + ds + "src" + ds + "App.Core.DB" + ds;
            mockCfg.Setup(x => x.Value).Returns(ac);

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            var contextOption = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connection).Options;

            _context = new ApplicationDbContext(contextOption);
            var conditionsHelper = new PostgresQueryConditionsBuilder();
            var helper = new SqlRepositoryHelper(conditionsHelper);

            IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            IQueryableCacheService cacheService = new QueryableCacheService(memoryCache);
            var repo = new CommonRepository(_context,helper,cacheService);

            IObjectMapper mapper= new ObjectMapper();
            DataService = new CommonDataService(repo, mapper);
        }
    }
}
