using System;
using App.Data.DTO.MSG;
using Xunit;
using Xunit.Abstractions;

namespace App.xUnitTests.DTOTests
{
    public class MSGTests: BasicServiceTest
    {
        public MSGTests(ITestOutputHelper output) : base(output)
        {
            // Hello, im empty!
        }

        [Fact]
        public void AnotherEventMessageDTO()
        {
            try
            {
                DataService.GetDto<AnotherEventMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AnotherEventMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - AnotherEventMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void LeaseAgreementChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<LeaseAgreementChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LeaseAgreementChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - LeaseAgreementChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void MessageListDTO()
        {
            try
            {
                DataService.GetDto<MessageListDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MessageListDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MessageListDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        //[Fact]
        //public void MPDActivityRestoration()
        //{
        //    try
        //    {
        //        DataService.GetDto<MPDActivityRestoration>();
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivityRestoration.................выполнено без ошибок");
        //        Assert.True(true);
        //    }
        //    catch (Exception exception)
        //    {
        //        output.WriteLine("");
        //        output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivityRestoration.................ошибка");
        //        output.WriteLine("");
        //        output.WriteLine("Текст ошибки:");
        //        output.WriteLine(exception.Message);
        //        Assert.False(true);
        //    }
        //}

        [Fact]
        public void MPDActivityRestorationMessageDTO()
        {
            try
            {
                DataService.GetDto<MPDActivityRestorationMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivityRestorationMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivityRestorationMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void MPDActivitySuspensionMessageDTO()
        {
            try
            {
                DataService.GetDto<MPDActivitySuspensionMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivitySuspensionMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDActivitySuspensionMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void MPDClosingForSomeActivityMessageDTO()
        {
            try
            {
                DataService.GetDto<MPDClosingForSomeActivityMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDClosingForSomeActivityMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDClosingForSomeActivityMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void MPDLocationRatificationMessageDTO()
        {
            try
            {
                DataService.GetDto<MPDLocationRatificationMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDLocationRatificationMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDLocationRatificationMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void MPDRestorationAfterSomeActivityMessageDTO()
        {
            try
            {
                DataService.GetDto<MPDRestorationAfterSomeActivityMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDRestorationAfterSomeActivityMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - MPDRestorationAfterSomeActivityMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void OrgFopLocationChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<OrgFopLocationChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - OrgFopLocationChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - OrgFopLocationChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PharmacyAreaChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<PharmacyAreaChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyAreaChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyAreaChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PharmacyHeadReplacementMessageDTO()
        {
            try
            {
                DataService.GetDto<PharmacyHeadReplacementMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyHeadReplacementMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyHeadReplacementMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void PharmacyNameChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<PharmacyNameChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyNameChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - PharmacyNameChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void ProductionDossierChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<ProductionDossierChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - ProductionDossierChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - ProductionDossierChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void SgdChiefNameChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<SgdChiefNameChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SgdChiefNameChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SgdChiefNameChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void SgdNameChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<SgdNameChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SgdNameChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SgdNameChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }

        [Fact]
        public void SupplierChangeMessageDTO()
        {
            try
            {
                DataService.GetDto<SupplierChangeMessageDTO>();
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SupplierChangeMessageDTO.................выполнено без ошибок");
                Assert.True(true);
            }
            catch (Exception exception)
            {
                output.WriteLine("");
                output.WriteLine("Проверка возможности выполнения SQL запроса при создании DTO - SupplierChangeMessageDTO.................ошибка");
                output.WriteLine("");
                output.WriteLine("Текст ошибки:");
                output.WriteLine(exception.Message);
                Assert.False(true);
            }
        }
    }
}
