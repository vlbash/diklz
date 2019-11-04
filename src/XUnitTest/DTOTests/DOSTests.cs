using System;
using App.Data.DTO.DOS;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class DOSTests: BasicServiceTest
    {
        public DOSTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty! 
        }

        [Fact]
        public void EDocumentDetailsDTO()
        {
            try
            {
                DataService.GetDto<EDocumentDetailsDTO>(extraParameters: new object[] { $"\'{Guid.Empty}\'" });
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EDocumentDetailsDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EDocumentDetailsDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void EDocumentListDTO()
        {
            try
            {
                DataService.GetDto<EDocumentListDTO>(extraParameters: new object[] { $"\'{Guid.Empty}\'" });
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EDocumentListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - EDocumentListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void RegisterEDocumentListDTO()
        {
            try
            {
                DataService.GetDto<RegisterEDocumentListDTO>(extraParameters: new object[] { $"\'{Guid.Empty}\'" });
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - RegisterEDocumentListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - RegisterEDocumentListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }
    }
}
