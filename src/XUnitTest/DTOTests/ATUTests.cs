using System;
using App.Data.DTO.ATU;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class ATUTests: BasicServiceTest
    {
        public ATUTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void AtuCityDTO()
        {
            try
            {
                DataService.GetDto<AtuCityDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuCityDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuCityDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void AtuStreetDTO()
        {
            try
            {
                DataService.GetDto<AtuStreetDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuStreetDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuStreetDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void AtuSubjectAddressDTO()
        {
            try
            {
                DataService.GetDto<AtuSubjectAddressDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuSubjectAddressDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AtuSubjectAddressDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }
    }
}
