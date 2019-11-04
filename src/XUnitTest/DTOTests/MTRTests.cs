using System;
using App.Data.DTO.MTR;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class MTRTests: BasicServiceTest
    {

        public MTRTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void MtrAppListDTO()
        {
            try
            {
                DataService.GetDto<MtrAppListDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MtrAppListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MtrAppListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }
    }
}
