using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data;
using App.Core.Data.DTO.Common;
using App.Core.Data.Helpers;
using App.Core.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace App.Core.Tests.CoreDTOModelsTests
{
    public class BaseTestCoreDTO
    {
        private readonly string _connection;
        private readonly UserInfo _userInfo;


        public BaseTestCoreDTO()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",optional: true, reloadOnChange: true);
               
            var configuration = builder.Build();

            _connection = configuration.GetConnectionString("DefaultConnection");

            //var context = new CoreDbContext(new DbContextOptionsBuilder<CoreDbContext>().UseNpgsql(_connection).Options);

            _userInfo = new UserInfo();

            //var personLog = context.Person.FirstOrDefault(x => x.UserId == "b70082cc-309d-4e2c-8456-30cefab089d9");
            //if (personLog != null)
            //{
            //    var employeeLog = context.Employee.FirstOrDefault(x => x.PersonId == personLog.Id);
            //    if (employeeLog != null)
            //    {
            //        var empProf = context.EmployeeProfiles.FirstOrDefault(x => x.EmployeeId == employeeLog.Id);
            //        if (empProf != null)
            //        {
            //            _userInfo.ProfileId = empProf.ProfileId;
            //            _userInfo.EmployeeId = employeeLog.Id;
            //            _userInfo.PersonId = personLog.Id;
            //            _userInfo.Rights = context.GetUserRights(_userInfo.ProfileId, _userInfo.EmployeeId);
            //        }
            //    }
            //}
        }


        public void CanReadDTO<TDTO>() where TDTO : CoreDTO
        {
            var userServiceMock = new Mock<IUserInfoService>();
            userServiceMock.Setup(meth => meth.GetCurrentUserInfo()).Returns(_userInfo);

            var contextOption = new DbContextOptionsBuilder<CoreDbContext>().UseNpgsql(_connection).Options;

            var saveRepo = new SafeDTORepository<TDTO>(new CoreDbContext(contextOption),
                new SqlRepositoryHelper(new PostgresQueryConditionsBuilder()));

            var service = new DTOService<TDTO>(saveRepo);
            try
            {
                var _ = service.GetDTO().FirstOrDefault();
            }
            catch (Exception)
            {
                Assert.False(true);
            }

            Assert.True(true);
        }
    }
}
