using App.Core.Data.Entities.ORG;
using System;
using Xunit;
using Moq;
using App.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using App.Core.Common;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using App.Core.Business.Services;
using App.Core.Data.Helpers;
using System.Collections.Generic;
using App.Core.Data.Repositories;
using App.Core.Data.DTO.Org;

namespace App.Core.Tests
{
    public static class BaseServiceGlobalFixture
    {
        static BaseServiceGlobalFixture()
        {
            mockCfg = new Mock<IOptions<AppConfig>>();
            var ds = Path.DirectorySeparatorChar;
            var ac = new AppConfig();
            ac.CurrentPath = Environment.CurrentDirectory + "/../../../../../../src/App.WebHost";
            ac.AppDbPath = Path.GetDirectoryName(ac.CurrentPath) + ds + "App.DB" + ds;
            ac.CoreDbPath = Path.GetDirectoryName(Path.GetDirectoryName(ac.CurrentPath)) +
                             ds + "Astum.Core" + ds + "src" + ds + "App.Core.DB" + ds;
            mockCfg.Setup(x => x.Value).Returns(ac);

            var mockUserIdentInfo = new Mock<UserInfo>();
            mockUserIdentInfo.Setup(x => x.Id).Returns("xUnit");
            mockUserIdentInfo.Setup(x => x.LoginData).Returns(new Dictionary<string, string> { { "UserLogin", "xUnit" } });

            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            var connection = Configuration.GetConnectionString("DefaultConnection");
            var contextOption = new DbContextOptionsBuilder<CoreDbContext>().UseNpgsql(connection).Options;

            _context = new CoreDbContext(contextOption) {
                UserInfo = mockUserIdentInfo.Object
            };
            _context.Database.EnsureCreated();

            var conditionsHelper = new PostgresQueryConditionsBuilder();
            var helper = new SqlRepositoryHelper(conditionsHelper);
            _OrgDepartmentService = new BaseService<OrgDepartmentListDTO, OrgDepartmentDetailDTO, Department>(
                new DTOService<OrgDepartmentListDTO>(new DTORepository<OrgDepartmentListDTO>(_context, helper)),
                new DTOService<OrgDepartmentDetailDTO>(new DTORepository<OrgDepartmentDetailDTO>(_context, helper)),
                new EntityRepository<Department>(_context)
                );
            _OrgDepartmentService.GetEntity().Where(x => x.Name.Contains("BaseService")).ToList().ForEach(x => _OrgDepartmentService.Remove(x));
        }

        public static IConfigurationRoot Configuration { get; set; }

        public static Mock<IOptions<AppConfig>> mockCfg { get; set; }

        public static IBaseService<OrgDepartmentListDTO, OrgDepartmentDetailDTO, Department> _OrgDepartmentService { get; set; }
        public static CoreDbContext _context { get; set; }
        public static Guid _testDTO1Guid { get; set; }
        public static Guid _testDTO2Guid { get; set; }

        public static OrgDepartmentDetailDTO _testOrgDepartmentDetailDTO1 { get; set; }
    }

    public class BaseServiceTestFixture : IDisposable
    {
        public BaseServiceTestFixture()
        {
        }

        public void Dispose()
        {
            if (BaseServiceGlobalFixture._testDTO1Guid != Guid.Empty)
            {
                BaseServiceGlobalFixture._OrgDepartmentService.Remove(BaseServiceGlobalFixture._testDTO1Guid);
                BaseServiceGlobalFixture._testDTO1Guid = Guid.Empty;
                BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 = null;
            }

            if (BaseServiceGlobalFixture._testDTO2Guid != Guid.Empty)
            {
                BaseServiceGlobalFixture._OrgDepartmentService.Remove(BaseServiceGlobalFixture._testDTO2Guid);
                BaseServiceGlobalFixture._testDTO2Guid = Guid.Empty;
            }
        }
    }


    [Collection("BaseServiceTest")]
    [TestCaseOrderer("App.Core.Tests.TestCaseOrderer", "App.Core.Tests")]
    public class BaseServiceTest : TestCaseOrderer, IClassFixture<BaseServiceTestFixture>
    {
        [Fact, TestPriority(0)]
        public void Create_OrgDepartment()
        {
            var _OrgDepartmentService = BaseServiceGlobalFixture._OrgDepartmentService;

            var dep1 = new OrgDepartmentDetailDTO() { Name = "xUnit BaseService dto1 " + DateTime.Now.ToString() };
            var dep2 = new OrgDepartmentDetailDTO() { Name = "xUnit BaseService dto2 " + DateTime.Now.ToString() };
            Assert.Equal(dep1.Id, Guid.Empty);
            Assert.Equal(dep2.Id, Guid.Empty);

            _OrgDepartmentService.SaveAsync(dep1).Wait();
            _OrgDepartmentService.SaveAsync(dep2).Wait();

            Assert.NotEqual(dep1.Id, Guid.Empty);
            Assert.NotEqual(dep2.Id, Guid.Empty);

            BaseServiceGlobalFixture._testDTO1Guid = dep1.Id;
            BaseServiceGlobalFixture._testDTO2Guid = dep2.Id;
        }


        [Fact, TestPriority(1)]
        public void Read_List_OrgDepartment()
        {
            var _OrgDepartmentService = BaseServiceGlobalFixture._OrgDepartmentService;

            if (BaseServiceGlobalFixture._testDTO1Guid == Guid.Empty || BaseServiceGlobalFixture._testDTO2Guid == Guid.Empty)
                Create_OrgDepartment();

            IQueryable<OrgDepartmentListDTO> queryDep = _OrgDepartmentService.GetListDTO()
                .Where(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid || x.Id == BaseServiceGlobalFixture._testDTO2Guid);
            Assert.NotEmpty(queryDep);
            Assert.Equal(2, queryDep.Count());

            var dto1 = queryDep.SingleOrDefault(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid);
            var dto2 = queryDep.SingleOrDefault(x => x.Id == BaseServiceGlobalFixture._testDTO2Guid);
            Assert.NotNull(dto1);
            Assert.NotNull(dto2);

            Assert.StartsWith("xUnit BaseService dto1", dto1.Name);
            Assert.StartsWith("xUnit BaseService dto2", dto2.Name);
        }


        [Fact, TestPriority(2)]
        public void Read_Detail_OrgDepartment()
        {
            var _OrgDepartmentService = BaseServiceGlobalFixture._OrgDepartmentService;

            if (BaseServiceGlobalFixture._testDTO1Guid == Guid.Empty)
                Create_OrgDepartment();

            IQueryable<OrgDepartmentDetailDTO> queryDep = _OrgDepartmentService.GetDetailDTO()
                .Where(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid);
            Assert.NotEmpty(queryDep);

            var dto1 = queryDep.SingleOrDefault(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid);
            Assert.NotNull(dto1);
            Assert.StartsWith("xUnit BaseService dto1", dto1.Name);

            BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 = dto1;
        }


        [Fact, TestPriority(3)]
        public void Update_OrgDepartment()
        {
            var _OrgDepartmentService = BaseServiceGlobalFixture._OrgDepartmentService;

            if (BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 == null)
                Read_Detail_OrgDepartment();

            BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1.Name = "xUnit BaseService dto1 MODIFIED " + DateTime.Now.ToString();
            _OrgDepartmentService.SaveAsync(BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1).Wait();

            IQueryable<OrgDepartmentDetailDTO> queryDep = _OrgDepartmentService.GetDetailDTO()
                .Where(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid);
            Assert.NotEmpty(queryDep);
            Assert.StartsWith("xUnit BaseService dto1 MODIFIED", queryDep.SingleOrDefault().Name);

            BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 = queryDep.SingleOrDefault();
        }


        [Fact, TestPriority(4)]
        public void Delete_OrgDepartment()
        {
            var _OrgDepartmentService = BaseServiceGlobalFixture._OrgDepartmentService;

            if (BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 == null)
                Read_Detail_OrgDepartment();

            _OrgDepartmentService.Remove(BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1.Id);

            IQueryable<OrgDepartmentDetailDTO> queryDep = _OrgDepartmentService.GetDetailDTO()
                .Where(x => x.Id == BaseServiceGlobalFixture._testDTO1Guid);
            Assert.Empty(queryDep);

            if (queryDep.FirstOrDefault() == null)
            {
                BaseServiceGlobalFixture._testOrgDepartmentDetailDTO1 = null;
                BaseServiceGlobalFixture._testDTO1Guid = Guid.Empty;
            }
        }
    }
}
