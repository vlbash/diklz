using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class LOGTests: BasicServiceTest
    {
        public LOGTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        //[Fact]
        //public void LogAuditEntryListDTO()
        //{
        //    try
        //    {                
        //        DataService.GetDto<LogAuditEntryListDTO>();
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LogAuditEntryListDTO.................выполнено без ошибок");
        //        Assert.True(true);
        //    }
        //    catch (Exception exception)
        //    {
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LogAuditEntryListDTO.................ошибка");
        //        output.WriteLine("");
        //        output.WriteLine("Текст ошибки:");
        //        output.WriteLine(exception.Message);
        //        Assert.False(true);
        //    }
        //}

        //[Fact]
        //public void LogAuditListOfChangesDTO()
        //{
        //    try
        //    {
        //        DataService.GetDto<LogAuditListOfChangesDTO>();
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LogAuditListOfChangesDTO.................выполнено без ошибок");
        //        Assert.True(true);
        //    }
        //    catch (Exception exception)
        //    {
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LogAuditListOfChangesDTO.................ошибка");
        //        output.WriteLine("");
        //        output.WriteLine("Текст ошибки:");
        //        output.WriteLine(exception.Message);
        //        Assert.False(true);
        //    }
        //}
    }
}
