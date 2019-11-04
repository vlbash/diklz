using System;
using App.Data.DTO.PRL;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class PRLTests: BasicServiceTest
    {
        public PRLTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void PrlAppDetailDTO()
        {
            try
            {
                DataService.GetDto<PrlAppDetailDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlAppDetailDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlAppDetailDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
                // Assert.Throws<Exception>(() => DataService.GetDto<PrlAppDetailDTO>());
                //throw;
            }
        }

        [Fact]
        public void PrlAppListDTO()
        {
            try
            {
                DataService.GetDto<PrlAppListDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlAppListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlAppListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
                // Assert.Throws<Exception>(() => DataService.GetDto<PrlAppDetailDTO>());
                //throw;
            }
        }

        [Fact]
        public void PrlContractorDetailDTO()
        {
            try
            {
                DataService.GetDto<PrlContractorDetailDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlContractorDetailDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlContractorDetailDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PrlContractorListDTO()
        {
            try
            {
                DataService.GetDto<PrlContractorListDTO>(extraParameters: new object[] { $"\'{Guid.Empty}\'" });
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlContractorListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlContractorListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PrlLicenseDetailDTO()
        {
            try
            {
                DataService.GetDto<PrlLicenseDetailDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlLicenseDetailDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlLicenseDetailDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PrlOrganizationExtFullDTO()
        {
            try
            {
                DataService.GetDto<PrlOrganizationExtFullDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtFullDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtFullDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PrlOrganizationExtMediumDTO()
        {
            try
            {
                DataService.GetDto<PrlOrganizationExtMediumDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtMediumDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtMediumDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PrlOrganizationExtShortDTO()
        {
            try
            {
                DataService.GetDto<PrlOrganizationExtShortDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtShortDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PrlOrganizationExtShortDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }
    }
}
