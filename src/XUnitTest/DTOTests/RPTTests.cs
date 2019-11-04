using System;
using App.Data.DTO.RPT;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class RPTTests: BasicServiceTest
    {
        public RPTTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void ReportBranchFullDetailsDTO()
        {
            try
            {
                DataService.GetDto<ReportBranchFullDetailsDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - ReportBranchLiteDetailsDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - ReportBranchLiteDetailsDTO.................ошибка");
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
