using System;
using App.Data.DTO.ORG;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class ORGTests: BasicServiceTest
    {
        public ORGTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void EmployeeExtDetailDTO()
        {
            try
            {
                DataService.GetDto<EmployeeExtDetailDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EmployeeExtDetailDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EmployeeExtDetailDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
                // Assert.Throws<Exception>(() => DataService.GetDto<PrlAppDetailDTO>());
                //throw;
            }
        }

        [Fact]
        public void OrganizationExtDetailDTO()
        {
            try
            {
                DataService.GetDto<OrganizationExtDetailDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - OrganizationExtDetailDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - OrganizationExtDetailDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
                // Assert.Throws<Exception>(() => DataService.GetDto<PrlAppDetailDTO>());
                //throw;
            }
        }
    }
}
