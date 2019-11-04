describe('jks', function () {
    jksOpts = getJksOptions(false);
    loadCertificateAndKey(jksOpts);
    launchTests(jksOpts);
});

describe('pfx', function () {
    pfxOpts = getPfxOptions(false);
    loadCertificateAndKey(pfxOpts);
    launchTests(pfxOpts);
});

describe('zs2', function () {
    zs2Opts = getZs2Options(false);
    loadCertificateAndKey(zs2Opts);
    launchTests(zs2Opts);
});

function launchTests(testingOptions) {
    var numSuffix;
    var testNum;
    var signOptions = testingOptions.signOptions;
    var currentSignOptions;
    for (testNum = 0; testNum < signOptions.length; testNum++) {

        currentSignOptions = signOptions[testNum];
        if (testNum < 9) {
            numSuffix = "0" + (testNum + 1);
        } else {
            numSuffix = testNum + 1;
        }

        describe("#" + numSuffix + ": " + JSON.stringify(currentSignOptions), function () {
            testingOptions.signedData = null;
            sign(testingOptions, currentSignOptions);
            verify(testingOptions, currentSignOptions);
        });
    }
};

function sign(options, signOpts) {
    it('data is signed', function (done) {
        var lib = options.lib;
        var signOptions = Uac.UAC_OPTION_INCLUDE_SIGNED_TIME;
        if (signOpts.SignByKeyId) {
            signOptions |= Uac.UAC_OPTION_IDENT_BY_KEY_ID;
        } else {
            signOptions |= Uac.UAC_OPTION_IDENT_BY_SERIAL_NUMBER;
        }
        if (signOpts.StoreContentWhenSign) {
            signOptions |= Uac.UAC_OPTION_STORE_CONTENT;
        }
        if (signOpts.AddCertificateWhenSign) {
            signOptions |= Uac.UAC_OPTION_INCLUDE_CERT;
        }

        var keyForSign;
        for (var i = 0; i < options.keyStore.keys.length; i++) {
            var key = options.keyStore.keys[i];
            var info = key.info;
            if (key.isKeyPair() && info.forSign) {
                keyForSign = key;
            };
        };

        expect(keyForSign).toBeDefined();

        // function to be tested
        lib.sign(
            Utils.b64EncodeUnicode(options.textToSign),
            keyForSign,
            //keyStore.keys["1"],
            signOptions,
            signOpts.AddTimeStamp,
            onSignSuccess,
            onSignError
        );

        function onSignSuccess(signedValue) {
            options.signedData = signedValue;
            expect(false).toBe(false);
            done();
        };

        function onSignError(err, obj) {
            options.signedData = null;
            expect(true).toBe(false);
            done();
        };
    });
};

function verify(options, signOpts) {
    it('data is verified', function (done) {
        var lib = options.lib;
        lib.signedDataGetContent(
            options.signedData,
            onGetSignedDataContentSuccess,
            onGetSignedDataContentError);

        function onGetSignedDataContentSuccess(content) {
            if (content) {
                data = content;
            } else {
                data = Utils.b64EncodeUnicode(options.textToSign);
            };

            lib.verify(
                options.signedData,
                data,
                onVerifySuccess,
                onVerifyError);
        };

        function onVerifySuccess(signInfos) {
            expect(true).toBe(true); // test passed
            done();
        };

        function onVerifyError(err, obj) {
            expect(true).toBe(false); // test failed
            done();
        };

        function onGetSignedDataContentError(err, obj) {
            expect(true).toBe(false); // test failed
            done();
        }
    });
};

function loadCertificateAndKey(options) {
    it('certificate and key are loaded', function (done) {
        var lib = options.lib;
        var messageCertificatesLoadedIncorrectly = "Certificates were loaded incorrectly";
        var messageCertificatesNotLoaded = "Certificates were not loaded";
        var messageKeyWithoutCertificateNotLoaded = "Key without certificate was not loaded";
        var messageKeyWithCertificateIncludedNotLoaded = "Key with certificates included were not loaded";
        var messageSuccess = "Success";
        var message = messageSuccess;
        var keyData = { data: options.fileKeyData, name: options.fileName }; 

        if (options.certIncluded) {
            certAndKeyLoadingError = true;

            lib.loadKeyStore(
                keyData,
                window.btoa(options.filePass),
                onLoadCertWithKeyIncludedSucces,
                onLoadCertWithKeyIncludedError
            );

            expect(certAndKeyLoadingError).toBe(false); // test passed
            expect(message).not.toContain(messageKeyWithCertificateIncludedNotLoaded); // test passed
            expect(message).toContain(messageSuccess); // test passed
            done();

        } else {
            keyLoadingError = true;
            certLoadingError = true;

            lib.loadKeyStore(
                keyData,
                window.btoa(options.filePass),
                onLoadKeyWithoutCertSuccess,
                onLoadKeyWithoutCertError
            );

            expect(keyLoadingError).toBeFalsy(); // test passed
            expect(certLoadingError).toBeFalsy(); // test passed
            expect(message).not.toContain(messageKeyWithoutCertificateNotLoaded); // test passed
            expect(message).not.toContain(messageCertificatesNotLoaded); // test passed
            expect(message).not.toContain(messageCertificatesLoadedIncorrectly); // test passed
            expect(message).toContain(messageSuccess);

            done();
        };

        function onLoadKeyWithoutCertSuccess(store) {

            keyLoadingError = false;
            options.keyStore = store;

            var certs = [];

            var info1 = lib.loadCert(options.certData1);
            if (info1) {
                certs.push(info1);
            };

            var info2 = lib.loadCert(options.certData2);
            if (info2) {
                certs.push(info2);
            };

            // function to be tested
            lib.keyStoreAddCerts(store, certs, options.certSaveAll,
                onLoadCertSuccess,
                onLoadCertError
            );
        };

        function onLoadCertSuccess(store, changesQty) {
            if (changesQty) {
                certLoadingError = false;
                options.keyStore = store;
            } else {
                certLoadingError = true;
                message = messageCertificatesLoadedIncorrectly;
            }
        };

        function onLoadKeyWithoutCertError(err, obj) {
            keyLoadingError = true;
            message = messageKeyWithoutCertificateNotLoaded;
        };

        function onLoadCertError(err, obj) {
            certLoadingError = true;
            message = messageCertificatesNotLoaded;
        };

        function onLoadCertWithKeyIncludedSucces(store) {
            certAndKeyLoadingError = false;
            options.keyStore = store;
        };

        function onLoadCertWithKeyIncludedError(err, obj) {
            certAndKeyLoadingError = true;
            message = messageKeyWithCertificateIncludedNotLoaded;
        };
    });
};

function getJksOptions(addCertificateWhenSign) {
    return getTestingOptions('jks', addCertificateWhenSign);
}

function getPfxOptions(addCertificateWhenSign) {
    return getTestingOptions('pfx', addCertificateWhenSign);
};

function getZs2Options(addCertificateWhenSign) {
    return getTestingOptions('zs2', addCertificateWhenSign);
};

function getTestingOptions(keyExtension, addCertificateWhenSign) {

    var options = {
        "lib": new UacPlugin("http://cala.it-engineering.com.ua/service/"), // main lib to test 
        "textToSign": "S1gn Їё, пож@луйста!", // test text to sign
        "keyStore": null, // key store that is created by main library
        "signedData": null // data with sign information in string64, that is returned by sign method
    };

    if (keyExtension === 'zs2') {
        options.fileKeyData = "MIIDxAIBAzCCA2EGCSqGSIb3DQEHAaCCA1IEggNOMIIDSjCCAZUGCSqGSIb3DQEHAaCCAYYEggGCMIIBfjCCAXoGCyqGSIb3DQEMCgECoIIBNjCCATIwgbMGCSqGSIb3DQEFDTCBpTBGBgkqhkiG9w0BBQwwOQQg+YlRANT9v7jC+MeU/PeNO8xJxJ20aFI0w6+YSDzE4UcCAgPoAgEgMA4GCiqGJAIBAQEBAQIFADBbBgsqhiQCAQEBAQEBAzBMBAi1o52kk7FMogRAqdbrRfE8cIKAxJZ7Ix9erfZY66TANykdONlr8CXKThf46XINxhW0OiiXXwvB3qNkOLVk6iwXn9ASPm24+sV5BAR65O//0L6O9lGwYIkzRfubNMaKUii66lQbVoyrNqAYdAHnkKfgMxrRWIZzsDOA3Wd+dQovUAHzQsZD5hU3sbUCYS6aJHaxJfuLoNEbIHT351pQ14JVhjN7DMlDo5/dJi3y0m10eVM740vW0G33u5UYrJLZpNHZ7+oDiv8xMTAvBgkqhkiG9w0BCRUxIgQgK6GeqfCHTnRRjbMYjHWujOUzlQ4+72U9NxmL7zBj4QYwggGtBgkqhkiG9w0BBwGgggGeBIIBmjCCAZYwggGSBgsqhkiG9w0BDAoBAqCCAU4wggFKMIGzBgkqhkiG9w0BBQ0wgaUwRgYJKoZIhvcNAQUMMDkEIEMBWPivR6E2KDcdJuGGnJ0ZNkZ7YxQJ01uDEQpyLsKXAgID6AIBIDAOBgoqhiQCAQEBAQECBQAwWwYLKoYkAgEBAQEBAQMwTAQIZmRKftjI1CkEQKnW60XxPHCCgMSWeyMfXq32WOukwDcpHTjZa/Alyk4X+OlyDcYVtDool18Lwd6jZDi1ZOosF5/QEj5tuPrFeQQEgZEZl+AmRTBEMRdMsj84c5k3UufKpiR/oZnsICBg/qy6TzTzXlyyXGTj5tZw5j5KgYSblua9XwNxqVlRiF7Uo7g+0puVqWSRny8Ito3OfjTsicxOKs1kKyttdoi/fgXNsYbS8zlg8mUw875w56Ue5gw77YV/nMkZ6qRk/y83NQXAqcbwYo7MDvxf+cSJAXxUe4uEMTEwLwYJKoZIhvcNAQkVMSIEINK+M7Y31RgbU1dGXfmt+uj3uTeKJJyQkeCwCcuoH2F6MFowMjAOBgoqhiQCAQEBAQIBBQAEIK+YeEFdY6gXeooDU0EtTurysmNcOcWT0ImiO0RhYfLdBCDDAzJQwoFIFdD87Z2+H1R/F/UCL+V/6dQsIGHHWh9CIAICA+g=";
        options.filePass = "111";
        options.fileName = "87654321_87654321_DU180904160002.ZS2";
        options.certName1 = "cl_0100000000000000000000000000000006e40b12.crt";
        options.certData1 = "MIIFKjCCBNKgAwIBAgIUEgvkBgAAAAAAAAAAAAAAAAAAAAEwDQYLKoYkAgEBAQEDAQEwggEcMVswWQYDVQQDDFLQkNCm0KHQmiDQotCe0JIgItCm0LXQvdGC0YAg0YHQtdGA0YLQuNGE0ZbQutCw0YbRltGXINC60LvRjtGH0ZbQsiAi0KPQutGA0LDRl9C90LAiMREwDwYDVQQHDAjQmtC40ZfQsjEZMBcGA1UEBQwQVUEtMzY4NjU3NTMtMDExNzFSMFAGA1UECgxJ0KLQntCSICLQptC10L3RgtGAINGB0LXRgNGC0LjRhNGW0LrQsNGG0ZbRlyDQutC70Y7Rh9GW0LIgItCj0LrRgNCw0ZfQvdCwIjEuMCwGA1UECwwl0JLRltC00LTRltC7INGB0LXRgNGC0LjRhNGW0LrQsNGG0ZbRlzELMAkGA1UEBgwCVUEwHhcNMTgwOTA0MTMwNzQ5WhcNMjAwOTAzMjA1OTU5WjCB5zEvMC0GA1UEAwwm0KLQtdGB0YIg0KLQtdGB0YLQvtCyINCi0LXRgdGC0L7QstC40YcxETAPBgNVBAcMCNCa0JjQh9CSMQ4wDAYDVQQRDAUwMTAwMTEZMBcGA1UEDAwQ0JrQtdGA0ZbQstC90LjQujERMA8GA1UEBAwI0KLQtdGB0YIxJjAkBgNVBCoMHdCi0LXRgdGC0L7QsiDQotC10YHRgtC+0LLQuNGHMRIwEAYDVQQFDAk4NzY1NDMyMUQxGjAYBgNVBAoMEdCi0J7QkiAi0KLQtdGB0YIiMQswCQYDVQQGDAJVQTBGMB4GCyqGJAIBAQEBAwEBMA8GDSqGJAIBAQEBAwEBAgYDJAAEIUc1rvGYJ3e84NFQGaX0Ga7w5aiSWfab+jGGyntZmzkIAaOCAjIwggIuMEEGA1UdCQQ6MDgwGgYMKoYkAgEBAQsBBAIBMQoTCDg3NjU0MzIxMBoGDCqGJAIBAQELAQQBATEKEwg4NzY1NDMyMTAaBgNVHREEEzARgQ9hY3NrQG1pbi5nb3YudWEwKQYDVR0OBCIEICuhnqnwh050UY2zGIx1rozlM5UOPu9lPTcZi+8wY+EGMCsGA1UdIwQkMCKAIJTGV3dyTasMIotrbbod3WJeB3bvWpFKyeW4w3SyexheMA4GA1UdDwEB/wQEAwIAwDAYBgNVHSUEETAPBg0qhiQCAQEBC5HKjVkBMBkGA1UdIAEB/wQPMA0wCwYJKoYkAgEBAQICMB4GCCsGAQUFBwEDAQH/BA8wDTALBgkqhiQCAQEBAgEwMwYDVR0uBCwwKjAooCagJIYiaHR0cDovL3Vha2V5LmNvbS51YS9saXN0LWRlbHRhLmNybDAtBgNVHR8EJjAkMCKgIKAehhxodHRwOi8vdWFrZXkuY29tLnVhL2xpc3QuY3JsMD0GCCsGAQUFBwELBDEwLzAtBggrBgEFBQcwA4YhaHR0cDovL3Vha2V5LmNvbS51YS9zZXJ2aWNlcy90c3AvMG0GCCsGAQUFBwEBBGEwXzAtBggrBgEFBQcwAoYhaHR0cDovL3Vha2V5LmNvbS51YS91YWtleWNlcnQucDdiMC4GCCsGAQUFBzABhiJodHRwOi8vdWFrZXkuY29tLnVhL3NlcnZpY2VzL29jc3AvMA0GCyqGJAIBAQEBAwEBA0MABECUxUbSmcDyzJtvy1zzgnpZUmZAXR2CaF37mjfWPhyjQoAv9WBEbS0e/JFMl/EMIE+GZ/0rx1Hzi77qwKz4bnxU";
        options.certName2 = "cl_0100000000000000000000000000000006e40b13.crt";
        options.certData2 = "MIIFPzCCBOegAwIBAgIUEwvkBgAAAAAAAAAAAAAAAAAAAAEwDQYLKoYkAgEBAQEDAQEwggEcMVswWQYDVQQDDFLQkNCm0KHQmiDQotCe0JIgItCm0LXQvdGC0YAg0YHQtdGA0YLQuNGE0ZbQutCw0YbRltGXINC60LvRjtGH0ZbQsiAi0KPQutGA0LDRl9C90LAiMREwDwYDVQQHDAjQmtC40ZfQsjEZMBcGA1UEBQwQVUEtMzY4NjU3NTMtMDExNzFSMFAGA1UECgxJ0KLQntCSICLQptC10L3RgtGAINGB0LXRgNGC0LjRhNGW0LrQsNGG0ZbRlyDQutC70Y7Rh9GW0LIgItCj0LrRgNCw0ZfQvdCwIjEuMCwGA1UECwwl0JLRltC00LTRltC7INGB0LXRgNGC0LjRhNGW0LrQsNGG0ZbRlzELMAkGA1UEBgwCVUEwHhcNMTgwOTA0MTMwNzUwWhcNMjAwOTAzMjA1OTU5WjCB5zEvMC0GA1UEAwwm0KLQtdGB0YIg0KLQtdGB0YLQvtCyINCi0LXRgdGC0L7QstC40YcxETAPBgNVBAcMCNCa0JjQh9CSMQ4wDAYDVQQRDAUwMTAwMTEZMBcGA1UEDAwQ0JrQtdGA0ZbQstC90LjQujERMA8GA1UEBAwI0KLQtdGB0YIxJjAkBgNVBCoMHdCi0LXRgdGC0L7QsiDQotC10YHRgtC+0LLQuNGHMRIwEAYDVQQFDAk4NzY1NDMyMUQxGjAYBgNVBAoMEdCi0J7QkiAi0KLQtdGB0YIiMQswCQYDVQQGDAJVQTBbMB4GCyqGJAIBAQEBAwEBMA8GDSqGJAIBAQEBAwEBAgkDOQAENsnCzsg8qJCbylWaYmtnHfRNg30pK3p9Y2KOcaeyJaFpDFRCJM2rqIhPhxw2mGjJEl90UzUeXqOCAjIwggIuMEEGA1UdCQQ6MDgwGgYMKoYkAgEBAQsBBAIBMQoTCDg3NjU0MzIxMBoGDCqGJAIBAQELAQQBATEKEwg4NzY1NDMyMTAaBgNVHREEEzARgQ9hY3NrQG1pbi5nb3YudWEwKQYDVR0OBCIEINK+M7Y31RgbU1dGXfmt+uj3uTeKJJyQkeCwCcuoH2F6MCsGA1UdIwQkMCKAIJTGV3dyTasMIotrbbod3WJeB3bvWpFKyeW4w3SyexheMA4GA1UdDwEB/wQEAwIACDAYBgNVHSUEETAPBg0qhiQCAQEBC5HKjVkBMBkGA1UdIAEB/wQPMA0wCwYJKoYkAgEBAQICMB4GCCsGAQUFBwEDAQH/BA8wDTALBgkqhiQCAQEBAgEwMwYDVR0uBCwwKjAooCagJIYiaHR0cDovL3Vha2V5LmNvbS51YS9saXN0LWRlbHRhLmNybDAtBgNVHR8EJjAkMCKgIKAehhxodHRwOi8vdWFrZXkuY29tLnVhL2xpc3QuY3JsMD0GCCsGAQUFBwELBDEwLzAtBggrBgEFBQcwA4YhaHR0cDovL3Vha2V5LmNvbS51YS9zZXJ2aWNlcy90c3AvMG0GCCsGAQUFBwEBBGEwXzAtBggrBgEFBQcwAoYhaHR0cDovL3Vha2V5LmNvbS51YS91YWtleWNlcnQucDdiMC4GCCsGAQUFBzABhiJodHRwOi8vdWFrZXkuY29tLnVhL3NlcnZpY2VzL29jc3AvMA0GCyqGJAIBAQEBAwEBA0MABEC3deh8sIe67Em8w2aopBy6Li6aSnDeiSZ4xanGYhojDQVoLpwY4/iic7OPK99U5UnIE37ZzeMgMFvnyEfsVYRR";
        options.certIncluded = false;
        options.certSaveAll = true;
    } else if (keyExtension === 'jks') {
        options.fileKeyData = "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";
        options.fileName = "pb_2880405538.jks";
        options.filePass = "QAZwsx12";
        options.certIncluded = true;
    } else if (keyExtension === 'pfx') {
        options.fileKeyData = "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";
        options.fileName = "Key-6.pfx";
        options.filePass = "1234";
        options.certIncluded = true;
    } else {
        // TODO: something wrong
    }

    var testingOpt01 = { "AddTimeStamp": true, "SignByKeyId": false, "StoreContentWhenSign": true, "AddCertificateWhenSign": true };
    var testingOpt02 = { "AddTimeStamp": false, "SignByKeyId": false, "StoreContentWhenSign": true, "AddCertificateWhenSign": true };
    var testingOpt03 = { "AddTimeStamp": true, "SignByKeyId": true, "StoreContentWhenSign": true, "AddCertificateWhenSign": true };
    var testingOpt04 = { "AddTimeStamp": false, "SignByKeyId": true, "StoreContentWhenSign": true, "AddCertificateWhenSign": true };
    var testingOpt05 = { "AddTimeStamp": true, "SignByKeyId": false, "StoreContentWhenSign": false, "AddCertificateWhenSign": true };
    var testingOpt06 = { "AddTimeStamp": false, "SignByKeyId": false, "StoreContentWhenSign": false, "AddCertificateWhenSign": true };
    var testingOpt07 = { "AddTimeStamp": true, "SignByKeyId": true, "StoreContentWhenSign": false, "AddCertificateWhenSign": true };
    var testingOpt08 = { "AddTimeStamp": false, "SignByKeyId": true, "StoreContentWhenSign": false, "AddCertificateWhenSign": true };
    var testingOpt09 = { "AddTimeStamp": true, "SignByKeyId": false, "StoreContentWhenSign": true, "AddCertificateWhenSign": false };
    var testingOpt10 = { "AddTimeStamp": false, "SignByKeyId": false, "StoreContentWhenSign": true, "AddCertificateWhenSign": false };
    var testingOpt11 = { "AddTimeStamp": true, "SignByKeyId": true, "StoreContentWhenSign": true, "AddCertificateWhenSign": false };
    var testingOpt12 = { "AddTimeStamp": false, "SignByKeyId": true, "StoreContentWhenSign": true, "AddCertificateWhenSign": false };
    var testingOpt13 = { "AddTimeStamp": true, "SignByKeyId": false, "StoreContentWhenSign": false, "AddCertificateWhenSign": false };
    var testingOpt14 = { "AddTimeStamp": false, "SignByKeyId": false, "StoreContentWhenSign": false, "AddCertificateWhenSign": false };
    var testingOpt15 = { "AddTimeStamp": true, "SignByKeyId": true, "StoreContentWhenSign": false, "AddCertificateWhenSign": false };
    var testingOpt16 = { "AddTimeStamp": false, "SignByKeyId": true, "StoreContentWhenSign": false, "AddCertificateWhenSign": false };

    options.signOptions = [testingOpt01, testingOpt02, testingOpt03, testingOpt04, testingOpt05, testingOpt06, testingOpt07, testingOpt08];
    var optsWithoutCertificate = [testingOpt09, testingOpt10, testingOpt11, testingOpt12, testingOpt13, testingOpt14, testingOpt15, testingOpt16];

    if (addCertificateWhenSign) {
        options.signOptions = options.signOptions.concat(optsWithoutCertificate);
    };

    return options;
}
