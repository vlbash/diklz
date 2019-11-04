﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CryptoSignService.Tests
{
    public class CertificateTests
    {
        [Theory]
        [InlineData("Test string 123!", "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")]
        [InlineData("Піявка", "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")]
        [InlineData(" ", "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")]
        [InlineData("\n", "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")]
        public async void VerifyDataCertificateIsUpToDate(string originalData, string dataToVerify)
        {
            var service = new CertifiedElectronicSignatureService();
            var data64 = TestHelper.EncodeTo64(originalData);

            var cryptoResponse = await service.VerifyDataAttachAsync(dataToVerify);
            Assert.True(cryptoResponse.Code == OperationCodes.SUCCESS);
            Assert.True(cryptoResponse.Certificate.IsUpToDate);
        }
    }
}
