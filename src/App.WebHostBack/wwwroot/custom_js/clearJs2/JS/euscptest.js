
//=============================================================================

var URL_EMAIL_PARAM = "@EMAIL_PARAM";
var URL_GET_KEP_CERTIFICATE_BY_EMAIL = "http://ca.iit.com.ua/services-cmp-getcert?eUserEMail=" +
    URL_EMAIL_PARAM + "&certType=2&respType=2";

var URL_GET_CERTIFICATES = "/custom_js/clearJs2/Data/CACertificates.p7b";
var URL_CAS = "/custom_js/clearJs2/Data/CAs.json";
var URL_XML_HTTP_PROXY_SERVICE = "/EuSign/ProxyMiddleware";

//=============================================================================

var SubjectCertTypes = [
    { "type": EU_SUBJECT_TYPE_UNDIFFERENCED, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_UNDIFFERENCED },
    { "type": EU_SUBJECT_TYPE_CA, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_UNDIFFERENCED },
    { "type": EU_SUBJECT_TYPE_CA_SERVER, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_UNDIFFERENCED },
    { "type": EU_SUBJECT_TYPE_CA_SERVER, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_CMP },
    { "type": EU_SUBJECT_TYPE_CA_SERVER, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_OCSP },
    { "type": EU_SUBJECT_TYPE_CA_SERVER, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_TSP },
    { "type": EU_SUBJECT_TYPE_END_USER, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_UNDIFFERENCED },
    { "type": EU_SUBJECT_TYPE_RA_ADMINISTRATOR, "subtype": EU_SUBJECT_CA_SERVER_SUB_TYPE_UNDIFFERENCED }
];

var CertKeyTypes = [
    EU_CERT_KEY_TYPE_UNKNOWN,
    EU_CERT_KEY_TYPE_DSTU4145,
    EU_CERT_KEY_TYPE_RSA
];

var KeyUsages = [
    EU_KEY_USAGE_UNKNOWN,
    EU_KEY_USAGE_DIGITAL_SIGNATURE,
    EU_KEY_USAGE_KEY_AGREEMENT
];

//=============================================================================

var EUSignCPTest = NewClass({
    "Vendor": "JSC IIT",
    "ClassVersion": "1.0.0",
    "ClassName": "EUSignCPTest",
    "CertsLocalStorageName": "Certificates",
    "CRLsLocalStorageName": "CRLs",
    "recepientsCertsIssuers": null,
    "recepientsCertsSerials": null,
    "PrivateKeyNameSessionStorageName": "PrivateKeyName",
    "PrivateKeySessionStorageName": "PrivateKey",
    "PrivateKeyPasswordSessionStorageName": "PrivateKeyPassword",
    "PrivateKeyCertificatesSessionStorageName": "PrivateKeyCertificates",
    "PrivateKeyCertificatesChainSessionStorageName": "PrivateKeyCertificatesChain",
    "CACertificatesSessionStorageName": "CACertificates",
    "CAServerIndexSessionStorageName": "CAServerIndex",
    "CAsServers": null,
    "CAServer": null,
    "offline": false,
    "useCMP": false,
    "loadPKCertsFromFile": false,
    "privateKeyCerts": null
},
    function () {
    },
    {
        initialize: function () {

            var _onSuccess = function () {
                try {
                    euSign.Initialize();
                    euSign.SetJavaStringCompliant(true);
                    euSign.SetCharset("UTF-16LE");

                    if (euSign.DoesNeedSetSettings()) {
                        euSignTest.setDefaultSettings();

                        if (utils.IsStorageSupported()) {
                            euSignTest.loadCertsAndCRLsFromLocalStorage();
                        }
                    }

                    euSignTest.loadCertsFromServer();
                    euSignTest.setCASettings(0);
                    euSignTest.setSelectPKCertificatesEvents();

                    if (utils.IsSessionStorageSupported()) {
                        var _readPrivateKeyAsStoredFile = function () {
                            euSignTest.readPrivateKeyAsStoredFile();
                        }
                        setTimeout(_readPrivateKeyAsStoredFile, 10);
                    }

                    euSignTest.updateCertList();

                } catch (e) {
                    alert(e);
                }
            };

            var _onError = function () {
                alert('Виникла помилка ' +
                    'при завантаженні криптографічної бібліотеки');
            };

            euSignTest.loadCAsSettings(_onSuccess, _onError);
        },
        loadCAsSettings: function (onSuccess, onError) {
            var pThis = this;

            var _onSuccess = function (casResponse) {
                try {
                    var servers = JSON.parse(casResponse.replace(/\\'/g, "'"));
                    for (var i = 0; i < servers.length; i++) {
                        var option = document.createElement("option");
                        option.text = servers[i].issuerCNs[0];
                    }

                    var option = document.createElement("option");
                    option.text = "інший";

                    pThis.CAsServers = servers;

                    onSuccess();
                } catch (e) {
                    onError();
                }
            };

            euSign.LoadDataFromServer(URL_CAS, _onSuccess, onError, false);
        },
        loadCertsAndCRLsFromLocalStorage: function () {
            try {
                var files = euSignTest.loadFilesFromLocalStorage(
                    euSignTest.CertsLocalStorageName,
                    function (fileName, fileData) {
                        if (fileName.indexOf(".cer") >= 0)
                            euSign.SaveCertificate(fileData);
                        else if (fileName.indexOf(".p7b") >= 0)
                            euSign.SaveCertificates(fileData);
                    });
                if (files != null && files.length > 0)
                    euSignTest.setItemsToList('SelectedCertsList', files);
            } catch (e) {
            }

            try {
                var files = euSignTest.loadFilesFromLocalStorage(
                    euSignTest.CRLsLocalStorageName,
                    function (fileName, fileData) {
                        if (fileName.indexOf(".crl") >= 0) {
                            try {
                                euSign.SaveCRL(true, fileData);
                            } catch (e) {
                                euSign.SaveCRL(false, fileData);
                            }
                        }
                    });
                if (files != null && files.length > 0)
                    euSignTest.setItemsToList('SelectedCRLsList', files);
            } catch (e) {
            }
        },
        loadCertsFromServer: function () {
            var certificates = utils.GetSessionStorageItem(
                euSignTest.CACertificatesSessionStorageName, true, false);
            if (certificates != null) {
                try {
                    euSign.SaveCertificates(certificates);
                    euSignTest.updateCertList();
                    return;
                } catch (e) {
                    alert("Виникла помилка при імпорті " +
                        "завантажених з сервера сертифікатів " +
                        "до файлового сховища");
                }
            }

            var _onSuccess = function (certificates) {
                try {
                    euSign.SaveCertificates(certificates);
                    utils.SetSessionStorageItem(
                        euSignTest.CACertificatesSessionStorageName,
                        certificates, false);
                    euSignTest.updateCertList();
                } catch (e) {
                    alert("Виникла помилка при імпорті " +
                        "завантажених з сервера сертифікатів " +
                        "до файлового сховища");
                }
            };

            var _onFail = function (errorCode) {
                console.log("Виникла помилка при завантаженні сертифікатів з сервера. " +
                    "(HTTP статус " + errorCode + ")");
            };

            utils.GetDataFromServerAsync(URL_GET_CERTIFICATES, _onSuccess, _onFail, true);
        },
        setDefaultSettings: function () {
            try {
                euSign.SetXMLHTTPProxyService(URL_XML_HTTP_PROXY_SERVICE);

                var settings = euSign.CreateFileStoreSettings();
                settings.SetPath("/certificates");
                settings.SetSaveLoadedCerts(true);
                euSign.SetFileStoreSettings(settings);

                settings = euSign.CreateProxySettings();
                euSign.SetProxySettings(settings);

                settings = euSign.CreateTSPSettings();
                euSign.SetTSPSettings(settings);

                settings = euSign.CreateOCSPSettings();
                euSign.SetOCSPSettings(settings);

                settings = euSign.CreateCMPSettings();
                euSign.SetCMPSettings(settings);

                settings = euSign.CreateLDAPSettings();
                euSign.SetLDAPSettings(settings);

                settings = euSign.CreateOCSPAccessInfoModeSettings();
                settings.SetEnabled(true);
                euSign.SetOCSPAccessInfoModeSettings(settings);

                var CAs = this.CAsServers;
                settings = euSign.CreateOCSPAccessInfoSettings();
                for (var i = 0; i < CAs.length; i++) {
                    settings.SetAddress(CAs[i].ocspAccessPointAddress);
                    settings.SetPort(CAs[i].ocspAccessPointPort);

                    for (var j = 0; j < CAs[i].issuerCNs.length; j++) {
                        settings.SetIssuerCN(CAs[i].issuerCNs[j]);
                        euSign.SetOCSPAccessInfoSettings(settings);
                    }
                }
            } catch (e) {
                alert("Виникла помилка при встановленні налашувань: " + e);
            }
        },
        setCASettings: function (caIndex) {
            try {
                var caServer = (caIndex < this.CAsServers.length) ?
                    this.CAsServers[caIndex] : null;
                var offline = ((caServer == null) ||
                    (caServer.address == "")) ?
                    true : false;
                var useCMP = (!offline && (caServer.cmpAddress != ""));
                var loadPKCertsFromFile = (caServer == null) ||
                    (!useCMP && !caServer.certsInKey);

                euSignTest.CAServer = caServer;
                euSignTest.offline = offline;
                euSignTest.useCMP = useCMP;
                euSignTest.loadPKCertsFromFile = loadPKCertsFromFile;

                var settings;

                euSignTest.clearPrivateKeyCertificatesList();

                settings = euSign.CreateTSPSettings();
                if (!offline) {
                    settings.SetGetStamps(true);
                    if (caServer.tspAddress != "") {
                        settings.SetAddress(caServer.tspAddress);
                        settings.SetPort(caServer.tspAddressPort);
                    } else {
                        settings.SetAddress('acskidd.gov.ua');
                        settings.SetPort('80');
                    }
                }
                euSign.SetTSPSettings(settings);

                settings = euSign.CreateOCSPSettings();
                if (!offline) {
                    settings.SetUseOCSP(true);
                    settings.SetBeforeStore(true);
                    settings.SetAddress(caServer.ocspAccessPointAddress);
                    settings.SetPort(caServer.ocspAccessPointPort);
                }
                euSign.SetOCSPSettings(settings);

                settings = euSign.CreateCMPSettings();
                settings.SetUseCMP(useCMP);
                if (useCMP) {
                    settings.SetAddress(caServer.cmpAddress);
                    settings.SetPort("80");
                }
                euSign.SetCMPSettings(settings);

                settings = euSign.CreateLDAPSettings();
                euSign.SetLDAPSettings(settings);
            } catch (e) {
                alert("Виникла помилка при встановленні налашувань: " + e);
            }
        },
        //-----------------------------------------------------------------------------
        chooseCertsAndCRLs: function (event) {
            var files = event.target.files;
            var certsFiles = [];
            var crlsFiles = [];

            if (utils.IsStorageSupported()) {
                utils.ClearFolder(euSignTest.CertsLocalStorageName);
                utils.ClearFolder(euSignTest.CRLsLocalStorageName);
            }

            for (var i = 0, file; file = files[i]; i++) {
                if (euSignTest.isCertificateExtension(file.name))
                    certsFiles.push(file);
                else if (euSignTest.isCRLExtension(file.name))
                    crlsFiles.push(file);
                else
                    continue;

                var fileReader = new FileReader();
                fileReader.onloadend = (function (fileName) {
                    return function (evt) {
                        if (evt.target.readyState == FileReader.DONE) {
                            euSignTest.saveFileToModuleFileStorage(fileName,
                                evt.target.result);
                        }
                    };
                })(file.name);

                fileReader.readAsArrayBuffer(file);
            }

            if (certsFiles.length > 0) {
                euSignTest.setFileItemsToList('SelectedCertsList', certsFiles);
            } else {
                document.getElementById('SelectedCertsList').innerHTML =
                    "Не обрано жодного сертифіката";
            }

            if (crlsFiles.length > 0) {
                euSignTest.setFileItemsToList('SelectedCRLsList', crlsFiles);
            } else {
                document.getElementById('SelectedCRLsList').innerHTML =
                    "Не обрано жодного СВС";
            }
        },
        updateCertList: function () {
            var certSubjType = SubjectCertTypes[5];
            var certKeyType = CertKeyTypes[0];
            var keyUsage = KeyUsages[0];

            try {
                var index = 0;
                var cert;
                var certs = [];

                while (true) {
                    cert = euSign.EnumCertificatesEx(
                        certSubjType.type, certSubjType.subtype,
                        certKeyType, keyUsage, index);
                    if (cert == null)
                        break;

                    certs.push(cert);
                    index++;
                };

                if (certs.length == 0) {
                    return;
                }

                var _makeCertField = function (name, value, addNewLine) {
                    return name + ': ' +
                        value +
                        (addNewLine ? '<br>' : '');
                }

                var certInfos = [];

                for (var i = 0; i < certs.length; i++) {
                    var certInfoStr = '';
                    var certInfo = certs[i].GetInfoEx();
                    var publicKeyType = '';
                    switch (certInfo.GetPublicKeyType()) {
                        case EU_CERT_KEY_TYPE_DSTU4145:
                            publicKeyType += 'ДСТУ-4145';
                            break;
                        case EU_CERT_KEY_TYPE_RSA:
                            publicKeyType += 'RSA';
                            break;
                        default:
                            publicKeyType = 'Невизначено';
                            break;
                    }

                    certInfoStr += _makeCertField('Власник', certInfo.GetSubjCN(), true);
                    certInfoStr += _makeCertField('ЦСК', certInfo.GetIssuerCN(), true);
                    certInfoStr += _makeCertField('Серійний номер', certInfo.GetSerial(), true);
                    certInfoStr += _makeCertField('Тип', publicKeyType, true);
                    certInfoStr += _makeCertField('Призначення', certInfo.GetKeyUsage(), false);

                    certInfos.push(certInfoStr);
                }
            } catch (e) {
                alert("Виникла помилка при " +
                    "отриманні сертифікатів з файлового сховища: " + e);
            }
        },
        //-----------------------------------------------------------------------------
        getCAServer: function () {
            var index = document.getElementById("CAsServersSelect").selectedIndex;

            if (index < euSignTest.CAsServers.length)
                return euSignTest.CAsServers[index];

            return null;
        },
        loadCAServer: function () {
            var index = utils.GetSessionStorageItem(
                euSignTest.CAServerIndexSessionStorageName, false, false);
            if (index != null) {
                document.getElementById("CAsServersSelect").selectedIndex =
                    parseInt(index);
                euSignTest.setCASettings(parseInt(index));
            }
        },
        storeCAServer: function () {
            var index = document.getElementById("CAsServersSelect").selectedIndex;
            return utils.SetSessionStorageItem(
                euSignTest.CAServerIndexSessionStorageName, index.toString(), false);
        },
        removeCAServer: function () {
            utils.RemoveSessionStorageItem(
                euSignTest.CAServerIndexSessionStorageName);
        },
        //-----------------------------------------------------------------------------
        storePrivateKey: function (keyName, key, password, certificates) {
            if (!utils.SetSessionStorageItem(
                euSignTest.PrivateKeyNameSessionStorageName, keyName, false) ||
                !utils.SetSessionStorageItem(
                    euSignTest.PrivateKeySessionStorageName, key, false) ||
                !utils.SetSessionStorageItem(
                    euSignTest.PrivateKeyPasswordSessionStorageName, password, true) ||
                !euSignTest.storeCAServer()) {
                return false;
            }

            if (Array.isArray(certificates)) {
                if (!utils.SetSessionStorageItems(
                    euSignTest.PrivateKeyCertificatesSessionStorageName,
                    certificates, false)) {
                    return false;
                }
            } else {
                if (!utils.SetSessionStorageItem(
                    euSignTest.PrivateKeyCertificatesChainSessionStorageName,
                    certificates, false)) {
                    return false;
                }
            }

            return true;
        },
        removeStoredPrivateKey: function () {
            utils.RemoveSessionStorageItem(
                euSignTest.PrivateKeyNameSessionStorageName);
            utils.RemoveSessionStorageItem(
                euSignTest.PrivateKeySessionStorageName);
            utils.RemoveSessionStorageItem(
                euSignTest.PrivateKeyPasswordSessionStorageName);
            utils.RemoveSessionStorageItem(
                euSignTest.PrivateKeyCertificatesChainSessionStorageName);
            utils.RemoveSessionStorageItem(
                euSignTest.PrivateKeyCertificatesSessionStorageName);

            euSignTest.removeCAServer();
        },
        //-----------------------------------------------------------------------------
        selectPrivateKeyFile: function (event) {
            var enable = (event.target.files.length == 1);

            setPointerEvents(document.getElementById('PKeyReadButton'), enable);
            document.getElementById('PKeyPassword').disabled =
                enable ? '' : 'disabled';
            document.getElementById('PKeyFileName').value =
                enable ? event.target.files[0].name : '';
            document.getElementById('PKeyPassword').value = '';
        },
        //-----------------------------------------------------------------------------
        getPrivateKeyCertificatesByCMP: function (key, password, onSuccess, onError) {
            try {
                var cmpAddress = euSignTest.getCAServer().cmpAddress + ":80";
                var keyInfo = euSign.GetKeyInfoBinary(key, password);
                onSuccess(euSign.GetCertificatesByKeyInfo(keyInfo, [cmpAddress]));
            } catch (e) {
                onError(e);
            }
        },
        getPrivateKeyCertificates: function (key, password, fromCache, onSuccess, onError) {
            var certificates;

            if (euSignTest.CAServer != null &&
                euSignTest.CAServer.certsInKey) {
                onSuccess([]);
                return;
            }

            if (fromCache) {
                if (euSignTest.useCMP) {
                    certificates = utils.GetSessionStorageItem(
                        euSignTest.PrivateKeyCertificatesChainSessionStorageName, true, false);
                } else if (euSignTest.loadPKCertsFromFile) {
                    certificates = utils.GetSessionStorageItems(
                        euSignTest.PrivateKeyCertificatesSessionStorageName, true, false)
                }

                onSuccess(certificates);
            } else if (euSignTest.useCMP) {
                euSignTest.getPrivateKeyCertificatesByCMP(
                    key, password, onSuccess, onError);
            } else if (euSignTest.loadPKCertsFromFile) {
                var _onSuccess = function (files) {
                    var certificates = [];
                    for (var i = 0; i < files.length; i++) {
                        certificates.push(files[i].data);
                    }

                    onSuccess(certificates);
                };

                euSign.ReadFiles(
                    euSignTest.privateKeyCerts,
                    _onSuccess, onError);
            }
        },
        readPrivateKey: function (keyName, key, password, certificates, fromCache) {
            var _onError = function (e) {

                if (fromCache) {
                    euSignTest.removeStoredPrivateKey();
                    euSignTest.privateKeyReaded(false);
                } else {
                    alert(e);
                }

                if (e.GetErrorCode != null &&
                    e.GetErrorCode() == EU_ERROR_CERT_NOT_FOUND) {
                }
            };

            if (certificates == null) {
                var _onGetCertificates = function (certs) {
                    if (certs == null) {
                        _onError(euSign.MakeError(EU_ERROR_CERT_NOT_FOUND));
                        return;
                    }

                    euSignTest.readPrivateKey(keyName, key, password, certs, fromCache);
                }

                euSignTest.getPrivateKeyCertificates(
                    key, password, fromCache, _onGetCertificates, _onError);
                return;
            }

            try {
                if (Array.isArray(certificates)) {
                    for (var i = 0; i < certificates.length; i++) {
                        euSign.SaveCertificate(certificates[i]);
                    }
                } else {
                    euSign.SaveCertificates(certificates);
                }

                euSign.ReadPrivateKeyBinary(key, password);

                if (!fromCache && utils.IsSessionStorageSupported()) {
                    if (!euSignTest.storePrivateKey(
                        keyName, key, password, certificates)) {
                        euSignTest.removeStoredPrivateKey();
                    }
                }

                euSignTest.privateKeyReaded(true);

                euSignTest.updateCertList();

                if (!fromCache)
                    euSignTest.showOwnerInfo();
            } catch (e) {
                _onError(e);
            }
        },
        readPrivateKeyAsImage: function (file, onSuccess, onError) {
            var image = new Image();
            image.onload = function () {
                try {
                    var qr = new QRCodeDecode();

                    var canvas = document.createElement('canvas');
                    var context = canvas.getContext('2d');

                    canvas.width = image.width;
                    canvas.height = image.height;

                    context.drawImage(image, 0, 0, canvas.width, canvas.height);
                    var imagedata = context.getImageData(0, 0, canvas.width, canvas.height);
                    var decoded = qr.decodeImageData(imagedata, canvas.width, canvas.height);
                    var arr = [];
                    for (var i = 0; i < decoded.length; i++)
                        arr.push(decoded.charCodeAt(i));
                    onSuccess(file.name, arr);
                } catch (e) {
                    onError();
                }
            }

            image.src = utils.CreateObjectURL(file);
        },
        readPrivateKeyAsStoredFile: function () {
            var keyName = utils.GetSessionStorageItem(
                euSignTest.PrivateKeyNameSessionStorageName, false, false);
            var key = utils.GetSessionStorageItem(
                euSignTest.PrivateKeySessionStorageName, true, false);
            var password = utils.GetSessionStorageItem(
                euSignTest.PrivateKeyPasswordSessionStorageName, false, true);
            if (keyName == null || key == null || password == null)
                return;

            euSignTest.loadCAServer();

            setPointerEvents(document.getElementById('PKeyReadButton'), true);
            document.getElementById('PKeyFileName').value = keyName;
            document.getElementById('PKeyPassword').value = password;
            var _readPK = function () {
                euSignTest.readPrivateKey(keyName, key, password, null, true);
                euSignTest.showOwnerInfo();
            }
            setTimeout(_readPK, 10);

            return;
        },
        readPrivateKeyButtonClick: function () {
            var passwordTextField = document.getElementById('PKeyPassword');

            var _onError = function (e) {
                alert(e);
            };

            var _onSuccess = function (keyName, key) {
                euSignTest.readPrivateKey(keyName, new Uint8Array(key),
                    passwordTextField.value, null, false);
            }

            try {
                if (document.getElementById('PKeyReadButton').title == 'Зчитати') {

                    var files = document.getElementById('PKeyFileInput').files;

                    if (files.length != 1) {
                        _onError("Виникла помилка при зчитуванні особистого ключа. " +
                            "Опис помилки: файл з особистим ключем не обрано");
                        return;
                    }

                    if (passwordTextField.value == "") {
                        passwordTextField.focus();
                        _onError("Виникла помилка при зчитуванні особистого ключа. " +
                            "Опис помилки: не вказано пароль доступу до особистого ключа");
                        return;
                    }

                    if (euSignTest.loadPKCertsFromFile &&
                        (certificatesFiles == null ||
                            certificatesFiles.length <= 0)) {
                        _onError("Виникла помилка при зчитуванні особистого ключа. " +
                            "Опис помилки: не обрано жодного сертифіката відкритого ключа");
                        return;
                    }

                    if (utils.IsFileImage(files[0])) {
                        euSignTest.readPrivateKeyAsImage(files[0], _onSuccess, _onError);
                    }
                    else {
                        var _onFileRead = function (readedFile) {
                            _onSuccess(readedFile.file.name, readedFile.data);
                        };

                        euSign.ReadFile(files[0], _onFileRead, _onError);
                    }
                } else {
                    euSignTest.removeStoredPrivateKey();
                    euSign.ResetPrivateKey();
                    euSignTest.privateKeyReaded(false);
                    passwordTextField.value = "";
                    document.getElementById("subjCn").innerHTML = "";
                    document.getElementById("issuerCn").innerHTML = "";
                    document.getElementById("serialNumber").innerHTML = "";
                    euSignTest.clearPrivateKeyCertificatesList();
                }
            } catch (e) {
                _onError(e);
            }
        },
        showOwnerInfo: function () {
            try {
                var ownerInfo = euSign.GetPrivateKeyOwnerInfo();
                document.getElementById("subjCn").innerHTML = ownerInfo.GetSubjCN();
                document.getElementById("issuerCn").innerHTML = ownerInfo.GetIssuerCN();
                document.getElementById("serialNumber").innerHTML = ownerInfo.GetSerial();
            } catch (e) {
                alert(e);
            }
        },
        showOwnCertificates: function () {
            try {
                var splitLine = "--------------------------------------------------";
                var message = "Інформація про сертифікат(и) користувача:\n";
                var i = 0;
                while (true) {
                    var info = euSign.EnumOwnCertificates(i);
                    if (info == null)
                        break;

                    var isNationalAlgs =
                        (info.GetPublicKeyType() == EU_CERT_KEY_TYPE_DSTU4145);

                    message += splitLine + "\n";
                    message += "Сертифікат № " + (i + 1) + "\n" +
                        "Власник: " + info.GetSubjCN() + "\n" +
                        "ЦСК: " + info.GetIssuerCN() + "\n" +
                        "Серійний номер: " + info.GetSerial() + "\n" +
                        "Призначення: " + info.GetKeyUsage() +
                        (isNationalAlgs ? " в державних " : " в міжнародних ") +
                        "алгоритмах та протоколах" + "\n";
                    message += splitLine + "\n";

                    i++;
                }

                if (i == 0)
                    message += "Відсутня";

                alert(message);

            } catch (e) {
                alert(e);
            }
        },
        blockOwnCertificates: function () {
            if (!confirm('Після блокування сертифікатів ос. ключа ' +
                'їх розблокування можливе лише при особистому ' +
                'зверненні до АЦСК. Продовжити?')) {
                return;
            }

            try {
                euSign.ChangeOwnCertificatesStatus(
                    EU_CCS_TYPE_HOLD, EU_REVOCATION_REASON_UNKNOWN);
                alert('Сертифікати ос. ключа успішно заблоковано');
            } catch (e) {
                alert(e);
            }
        },
        revokeOwnCertificates: function () {
            if (!confirm('Після скасування сертифікатів ос. ключа ' +
                'використання ос. ключа буде не можливе. Продовжити?')) {
                return;
            }

            try {
                var revocationReason = parseInt(
                    document.getElementById("PKeyRevokationReasonSelect").value);

                euSign.ChangeOwnCertificatesStatus(
                    EU_CCS_TYPE_REVOKE, revocationReason);
                alert('Сертифікати ос. ключа успішно скасовано');
            } catch (e) {
                alert(e);
            }
        },
        //-----------------------------------------------------------------------------
        changePrivKeyType: function () {
            var useUA = document.getElementById('ChooseKeysUARadioBtn').checked;
            var useRSA = document.getElementById('ChooseKeysRSARadioBtn').checked;

            if (document.getElementById('ChooseKeysUARSARadioBtn').checked)
                useUA = useRSA = true;

            document.getElementById('UAPrivKeyParams').style.display =
                useUA ? 'block' : 'none';
            document.getElementById('InternationalPrivKeyParams').style.display =
                useRSA ? 'block' : 'none';
        },
        generatePK: function () {
            var pkPassword = document.getElementById('PGenKeyPassword').value;

            if (pkPassword == "") {
                alert("Пароль особистого ключа не вказано");
                document.getElementById('PGenKeyPassword').focus();
                return;
            }

            var useUA = document.getElementById('ChooseKeysUARadioBtn').checked;
            var useRSA = document.getElementById('ChooseKeysRSARadioBtn').checked;

            if (document.getElementById('ChooseKeysUARSARadioBtn').checked)
                useUA = useRSA = true;

            var uaKeysType = useUA ?
                EU_KEYS_TYPE_DSTU_AND_ECDH_WITH_GOST : EU_KEYS_TYPE_NONE;
            var uaDSKeysSpec = useUA ?
                parseInt(document.getElementById("UAKeySpecSelect").value) : 0;
            var uaKEPSpec = useUA ?
                parseInt(document.getElementById("UAKEPKeySpecSelect").value) : 0;

            var intKeysType = useRSA ?
                EU_KEYS_TYPE_RSA_WITH_SHA : EU_KEYS_TYPE_NONE;

            var intKeysSpec = useRSA ?
                parseInt(document.getElementById("InternationalKeySpecSelect").value) : 0;

            var userInfo = EndUserInfo();
            userInfo.commonName = "User 1";
            userInfo.locality = "Kharkov";
            userInfo.state = "Kharkovska";

            var _generatePKFunction = function () {
                try {
                    euSign.SetRuntimeParameter(
                        EU_MAKE_PKEY_PFX_CONTAINER_PARAMETER,
                        document.getElementById('PKPFXContainerCheckbox').checked);

                    var privKey = euSign.GeneratePrivateKey(
                        pkPassword, uaKeysType, uaDSKeysSpec, false, uaKEPSpec,
                        intKeysType, intKeysSpec, null, null);

                    saveFile(privKey.privateKeyName, privKey.privateKey);

                    if (useUA) {
                        saveFile(privKey.uaRequestName, privKey.uaRequest);
                        saveFile(privKey.uaKEPRequestName, privKey.uaKEPRequest);
                    }

                    if (useRSA) {
                        saveFile(privKey.internationalRequestName,
                            privKey.internationalRequest);
                    }
                } catch (e) {
                    alert("Виникла помилка при генерації особистого ключа. " +
                        "Опис помилки: " + e);
                }
            };

            setTimeout(_generatePKFunction, 10);
        },
        //-----------------------------------------------------------------------------
        signData: function () {
            var signedFileModel = {};
            signedFileModel.id = FileModel.id;
            signedFileModel.files = [];
            var _signDataFunction = function () {
                FileModel.files.forEach(function (item, i, files) {
                    try {
                        var sign = "";
                        if (item.id == '00000000-0000-0000-0000-000000000000') {
                            item.file = item.file;
                            sign = euSign.SignDataInternal(true, item.file, true);
                        } else {
                            sign = euSign.SignData(item.file, true);
                        }
                        var obj = item;
                        obj.file = sign;
                        signedFileModel.files.push(obj);
                        var statusName = 'підпис данних  ' + obj.name;
                    } catch (e) {
                        alert(e);
                    }
                });
                $.ajax({
                    url: '/PRL/PrlApp/SaveSignedFiles',
                    type: 'POST',
                    dataType: 'json',
                    data: signedFileModel,
                    success: function (data) {
                        if (data.success == false)
                            alert(data.errorMessage);
                        window.location.replace(data.returnUrl);
                    }

                });
            };

            setTimeout(_signDataFunction, 10);
        },
        verifyData: function (signedData, i, isLast) {
            var verifiedDataText = "";

            var _verifyDataFunction = function () {
                try {
                    var info = "";
                    info = euSign.VerifyDataInternal(signedData);
                    var message = "<span style='color:green;'>Підпис успішно перевірено</span>";
                    var ownerInfo = info.GetOwnerInfo();
                    var timeInfo = info.GetTimeInfo();
                    message += "<br/>";;
                    message += "Підписувач: " +
                        ownerInfo.GetSubjCN() +
                        "<br/>" +
                        "ЦСК: " +
                        ownerInfo.GetIssuerCN() +
                        "<br/>" +
                        "Серійний номер: " +
                        ownerInfo.GetSerial() +
                        "<br/>";
                    if (timeInfo.IsTimeAvail()) {
                        message += (timeInfo.IsTimeStamp() ? "Мітка часу: " : "Час підпису: ") +
                            formatDate(timeInfo.GetTime());
                    } else {
                        message += "Час підпису відсутній";
                    }
                    FileModel.files[i].file = euSign.Base64Encode(info.GetData());
                    document.getElementById("signInfo" + i).innerHTML = message;
                    
                } catch (e) {
                    alert(e);
                    var message = "<span style='color: red'>Помилка перевірки файла</span>";
                    document.getElementById("signInfo" + i).innerHTML = message;
                }
                mt.findGrids($("#result"));
                if (isLast) {
                    mt.hideLoader(".wrapper-content");
                }
            }
            setTimeout(_verifyDataFunction, 2000);
        },
//-----------------------------------------------------------------------------
chooseFileToSign: function (event) {
    var enable = (event.target.files.length == 1);

    setPointerEvents(document.getElementById('SignFileButton'), enable);
},
chooseFileToVerify: function (event) {
    var enable = (document.getElementById('FileToVerify').files.length == 1) &&
        (document.getElementById("InternalSignCheckbox").checked ||
            document.getElementById('FileWithSign').files.length == 1)

    setPointerEvents(document.getElementById('VerifyFileButton'), enable);
},
signFile: function () {
    var file = document.getElementById('FileToSign').files[0];

    if (file.size > Module.MAX_DATA_SIZE) {
        alert("Розмір файлу для піпису занадто великий. Оберіть файл меншого розміру");
        return;
    }

    var fileReader = new FileReader();

    fileReader.onloadend = (function (fileName) {
        return function (evt) {
            if (evt.target.readyState != FileReader.DONE)
                return;

            var isInternalSign =
                document.getElementById("InternalSignCheckbox").checked;
            var isAddCert = document.getElementById(
                "AddCertToInternalSignCheckbox").checked;
            var dsAlgType = parseInt(
                document.getElementById("DSAlgTypeSelect").value);

            var data = new Uint8Array(evt.target.result);

            try {
                var sign;

                if (dsAlgType == 1) {
                    if (isInternalSign)
                        sign = euSign.SignDataInternal(isAddCert, data, false);
                    else
                        sign = euSign.SignData(data, false);
                } else {
                    sign = euSign.SignDataRSA(data, isAddCert,
                        !isInternalSign, false);
                }

                saveFile(fileName + ".p7s", sign);


                alert("Файл успішно підписано");
            } catch (e) {

                alert(e);
            }
        };
    })(file.name);


    fileReader.readAsArrayBuffer(file);
},
verifyFile: function () {
    var isInternalSign =
        document.getElementById("InternalSignCheckbox").checked;
    var isGetSignerInfo =
        document.getElementById("GetSignInfoCheckbox").checked;
    var files = [];

    files.push(document.getElementById('FileToVerify').files[0]);
    if (!isInternalSign)
        files.push(document.getElementById('FileWithSign').files[0]);

    if ((files[0].size > (Module.MAX_DATA_SIZE + EU_MAX_P7S_CONTAINER_SIZE)) ||
        (!isInternalSign && (files[1].size > Module.MAX_DATA_SIZE))) {
        alert("Розмір файлу для перевірки підпису занадто великий. Оберіть файл меншого розміру");
        return;
    }

    var _onSuccess = function (files) {
        try {
            var info = "";
            if (isInternalSign) {
                info = euSign.VerifyDataInternal(files[0].data);
            } else {
                info = euSign.VerifyData(files[0].data, files[1].data);
            }

            var message = "Підпис успішно перевірено";

            if (isGetSignerInfo) {
                var ownerInfo = info.GetOwnerInfo();
                var timeInfo = info.GetTimeInfo();

                message += "\n";
                message += "Підписувач: " + ownerInfo.GetSubjCN() + "\n" +
                    "ЦСК: " + ownerInfo.GetIssuerCN() + "\n" +
                    "Серійний номер: " + ownerInfo.GetSerial() + "\n";
                if (timeInfo.IsTimeAvail()) {
                    message += (timeInfo.IsTimeStamp() ?
                        "Мітка часу:" : "Час підпису: ") + timeInfo.GetTime();
                } else {
                    message += "Час підпису відсутній";
                }
            }

            if (isInternalSign) {
                saveFile(files[0].name.substring(0,
                    files[0].name.length - 4), info.GetData());
            }

            alert(message);

        } catch (e) {
            alert(e);

        }
    }

    var _onFail = function (files) {

        alert("Виникла помилка при зчитуванні файлів для перевірки підпису");
    }


    utils.LoadFilesToArray(files, _onSuccess, _onFail);
},
//-----------------------------------------------------------------------------
envelopData: function () {
    var issuers = euSignTest.recepientsCertsIssuers;
    var serials = euSignTest.recepientsCertsSerials;

    if (issuers == null || serials == null ||
        issuers.length <= 0 || serials.length <= 0) {
        alert("Не обрано жодного сертифіката отримувача");
        return;
    }

    var isAddSign = document.getElementById("AddSignCheckbox").checked;
    var data = document.getElementById("DataToEnvelopTextEdit").value;
    var envelopedText = document.getElementById("EnvelopedDataText");
    var developedText = document.getElementById("DevelopedDataText");
    var kepAlgType = parseInt(document.getElementById("KEPAlgTypeSelect").value);

    envelopedText.value = "";
    developedText.value = "";

    var _envelopDataFunction = function () {
        try {
            if (kepAlgType == 1) {
                envelopedText.value = euSign.EnvelopDataEx(
                    issuers, serials, isAddSign, data, true);
            } else {
                envelopedText.value = euSign.EnvelopDataRSAEx(
                    kepAlgType, issuers, serials, isAddSign, data, true);
            }

        } catch (e) {
            alert(e);
        }
    };

    setTimeout(_envelopDataFunction, 10);
},
developData: function () {
    var envelopedText = document.getElementById("EnvelopedDataText");
    var developedText = document.getElementById("DevelopedDataText");

    developedText.value = "";

    var _developDataFunction = function () {
        try {
            var info = euSign.DevelopData(envelopedText.value);
            var ownerInfo = info.GetOwnerInfo();
            var timeInfo = info.GetTimeInfo();

            var message = "Дані успішно розшифровано";
            message += "\n";
            message += "Відправник: " + ownerInfo.GetSubjCN() + "\n" +
                "ЦСК: " + ownerInfo.GetIssuerCN() + "\n" +
                "Серійний номер: " + ownerInfo.GetSerial() + "\n";
            if (timeInfo.IsTimeAvail()) {
                message += (timeInfo.IsTimeStamp() ?
                    "Мітка часу:" : "Час підпису: ") + timeInfo.GetTime();
            } else {
                message += "Підпис відсутній";
            }

            developedText.value = euSign.ArrayToString(info.GetData());

            alert(message);
        } catch (e) {
            alert(e);
        }
    };

    setTimeout(_developDataFunction, 10);
},
//-----------------------------------------------------------------------------
chooseEnvelopFile: function (event) {
    var enable = (event.target.files.length == 1);

    setPointerEvents(document.getElementById('EnvelopFileButton'), enable);
    setPointerEvents(document.getElementById('DevelopedFileButton'), enable);
},
envelopFile: function () {
    var issuers = euSignTest.recepientsCertsIssuers;
    var serials = euSignTest.recepientsCertsSerials;

    if (issuers == null || serials == null ||
        issuers.length <= 0 || serials.length <= 0) {
        alert("Не обрано жодного сертифіката отримувача");
        return;
    }

    var file = document.getElementById('EnvelopFiles').files[0];
    var fileReader = new FileReader();

    fileReader.onloadend = (function (fileName) {
        return function (evt) {
            if (evt.target.readyState != FileReader.DONE)
                return;

            var fileData = new Uint8Array(evt.target.result);
            var isAddSign = document.getElementById("AddSignCheckbox").checked;
            var kepAlgType = parseInt(document.getElementById("KEPAlgTypeSelect").value);
            var envelopedFileData;
            try {
                if (kepAlgType == 1) {
                    envelopedFileData = euSign.EnvelopDataEx(
                        issuers, serials, isAddSign, fileData, false);
                } else {
                    envelopedFileData = euSign.EnvelopDataRSAEx(
                        kepAlgType, issuers, serials, isAddSign, fileData, false);
                }
                saveFile(fileName + ".p7e", envelopedFileData);

                alert("Файл успішно зашифровано");
            } catch (e) {
                alert(e);
            }
        };
    })(file.name);

    fileReader.readAsArrayBuffer(file);
},
developFile: function () {
    var file = document.getElementById('EnvelopFiles').files[0];
    var fileReader = new FileReader();

    if (file.size > (Module.MAX_DATA_SIZE + EU_MAX_P7E_CONTAINER_SIZE)) {
        alert("Розмір файлу для розшифрування занадто великий. Оберіть файл меншого розміру");
        return;
    }

    fileReader.onloadend = (function (fileName) {
        return function (evt) {
            if (evt.target.readyState != FileReader.DONE)
                return;

            var fileData = new Uint8Array(evt.target.result);

            try {
                var info = euSign.DevelopData(fileData);
                var ownerInfo = info.GetOwnerInfo();
                var timeInfo = info.GetTimeInfo();

                var message = "Файл успішно розшифровано";
                message += "\n";
                message += "Відправник: " + ownerInfo.GetSubjCN() + "\n" +
                    "ЦСК: " + ownerInfo.GetIssuerCN() + "\n" +
                    "Серійний номер: " + ownerInfo.GetSerial() + "\n";
                if (timeInfo.IsTimeAvail()) {
                    message += (timeInfo.IsTimeStamp() ?
                        "Мітка часу:" : "Час підпису: ") + timeInfo.GetTime();
                } else {
                    message += "Підпис відсутній";
                }

                alert(message);

                saveFile(fileName.substring(0, fileName.length - 4), info.GetData());
            } catch (e) {
                alert(e);
            }
        };
    })(file.name);

    fileReader.readAsArrayBuffer(file);
},
//-----------------------------------------------------------------------------
getOwnCertificateInfo: function (keyType, keyUsage) {
    try {
        var index = 0;
        while (true) {
            var info = euSign.EnumOwnCertificates(index);
            if (info == null)
                return null;

            if ((info.GetPublicKeyType() == keyType) &&
                ((info.GetKeyUsageType() & keyUsage) == keyUsage)) {
                return info;
            }

            index++;
        }
    } catch (e) {
        alert(e);
    }

    return null;
},
getOwnCertificate: function (keyType, keyUsage) {
    try {
        var info = euSignTest.getOwnCertificateInfo(
            keyType, keyUsage);
        if (info == null)
            return null;

        return euSign.GetCertificate(
            info.GetIssuer(), info.GetSerial());
    } catch (e) {
        alert(e);
    }

    return null;
},
recepientCertLoaded: function (files, curIndex, processedFiles) {
    return function (evt) {
        if (evt.target.readyState != FileReader.DONE)
            return;

        var file = new Object();
        file.name = files[curIndex].name;
        file.isCertificate =
            euSignTest.isCertificateExtension(file.name);
        if (file.isCertificate) {
            file.data = new Uint8Array(evt.target.result);
        }

        processedFiles.push(file);
        curIndex++;

        if (curIndex < files.length) {
            var fileReader = new FileReader();
            fileReader.onloadend = euSignTest.recepientCertLoaded(
                files, curIndex, processedFiles);
            fileReader.readAsArrayBuffer(files[curIndex]);
            return;
        }

        euSignTest.recepientCertsLoaded(processedFiles);
    };
},
recepientCertsLoaded: function (processedFiles) {
    var loadedFiles = [];
    var issuers = [];
    var serials = [];

    for (var i = 0; i < processedFiles.length; i++) {
        var file = processedFiles[i];
        var fileInfo = file.name;
        if (!file.isCertificate) {
            fileInfo += "<br>(Не завантажено: " +
                "не вірне розширення файлу '.cer')";
        } else {
            try {
                var certInfo = euSign.ParseCertificate(file.data);
                fileInfo += "<br>Власник: " + certInfo.subjCN + "<br>" +
                    "ЦСК: " + certInfo.issuerCN + "<br>" +
                    "Серійний номер: " + certInfo.serial;
                issuers.push(certInfo.issuer);
                serials.push(certInfo.serial);
                euSign.SaveCertificate(file.data);
            } catch (e) {
                fileInfo += "<br>(Не завантажено: " + e.toString() + ")";
            }
        }

        loadedFiles.push(fileInfo);
    }

    euSignTest.setItemsToList(
        'SelectedRecipientsCertsList', loadedFiles);

    euSignTest.recepientsCertsIssuers = issuers;
    euSignTest.recepientsCertsSerials = serials;
},
chooseRecepientsCertificates: function (event) {
    euSignTest.recepientsCertsIssuers = [];
    euSignTest.recepientsCertsSerials = [];

    var files = event.target.files;
    if (files.length <= 0) {
        document.getElementById('SelectedRecipientsCertsList').innerHTML =
            "Не обрано жодного сертифіката";
        return;
    }

    document.getElementById('SelectedRecipientsCertsList').innerHTML = "";

    var fileReader = new FileReader();
    fileReader.onloadend = euSignTest.recepientCertLoaded(
        files, 0, []);
    fileReader.readAsArrayBuffer(files[0]);
},
testSignature: function () {
    var string = "Data to sign, 1234567890";
    var array = new Uint8Array([0, 1, 2, 3, 4, 5, 6, 7, 8]);

    var sign, hash, info;

    var resultText = document.getElementById('TestSignText');
    resultText.value = "Тестування функцій підпису...\r\n";

    try {
        resultText.value +=
            "Тестування функцій зовнішнього підпису даних...\r\n";
        sign = euSign.SignData(string, true);
        info = euSign.VerifyData(string, sign);
        info = euSign.VerifyDataOnTimeEx(string, 0, sign,
            new Date(), true, true);

        sign = euSign.SignData(array, false);
        info = euSign.VerifyData(array, sign);
        info = euSign.VerifyDataOnTimeEx(array, 0, sign,
            null, true, true);

        sign = euSign.SignData(array, true);
        info = euSign.VerifyData(array, sign);

        resultText.value +=
            "Тестування функцій внутрішнього підпису даних...\r\n";
        sign = euSign.SignDataInternal(true, string, true);
        info = euSign.VerifyDataInternal(sign);
        if (euSign.ArrayToString(info.GetData()) != string) {
            resultText.value += "Тестування функцій внутрішнього підпису " +
                "завершилося з помилкою: " +
                "Перевірені дані не співпадають з даними, що підписувалися";
            return false;
        }
        info = euSign.VerifyDataInternalOnTimeEx(0, sign,
            new Date(), false, true);

        sign = euSign.SignDataInternal(false, string, false);
        info = euSign.VerifyDataInternal(sign);
        if (euSign.ArrayToString(info.GetData()) != string) {
            resultText.value += "Тестування функцій внутрішнього підпису " +
                "завершилося з помилкою: " +
                "Перевірені дані не співпадають з даними, що підписувалися";
            return false;
        }

        sign = euSign.SignDataInternal(false, array, false);
        info = euSign.VerifyDataInternal(sign);
        if (!utils.CompareArrays(info.GetData(), array)) {
            resultText.value += "Тестування функцій внутрішнього підпису " +
                "завершилося з помилкою: " +
                "Перевірені дані не співпадають з даними, що підписувалися";
            return false;
        }

        resultText.value += "Тестування функцій підпису гещ...\r\n";
        hash = euSign.HashData(string, true);
        sign = euSign.SignHash(hash, true);
        info = euSign.VerifyHash(hash, sign);
        info = euSign.VerifyHashOnTimeEx(hash, 0, sign,
            new Date(), true, true);

        hash = euSign.HashData(string, false);
        sign = euSign.SignHash(hash, true);
        info = euSign.VerifyHash(hash, sign);
        info = euSign.VerifyHashOnTimeEx(hash, 0, sign,
            null, true, true);

        hash = euSign.HashData(array, false);
        sign = euSign.SignHash(hash, true);
        info = euSign.VerifyHash(hash, sign);
    } catch (e) {
        resultText.value += "Тестування функцій підпису " +
            "завершилося з помилкою: " + e.toString();
        return false;
    }

    if (!euSignTest.testRemoteSign() ||
        !euSignTest.testRawSign()) {
        return false;
    }

    resultText.value += "Тестування функцій підпису " +
        "завершилося успішно";

    return true;
},
testRemoteSign: function () {
    var string = "Data to sign, 1234567890";
    var array = new Uint8Array([0, 1, 2, 3, 4, 5, 6, 7, 8]);

    var ownCert, hash;
    var emptySign, signer, sign;
    var info;

    var resultText = document.getElementById('TestSignText');

    resultText.value += "Тестування функцій додавання підпису...\r\n";
    try {
        ownCert = euSignTest.getOwnCertificate(
            EU_CERT_KEY_TYPE_DSTU4145, EU_KEY_USAGE_DIGITAL_SIGNATURE);
        if (ownCert == null) {
            resultText.value += "Тестування функцій додавання підпису " +
                "завершилося з помилкою: " +
                "Сертифікат користувача для підпису за " +
                "алгоритмом ДСТУ-4145 не знайдено";
            return false;
        }

        hash = euSign.HashData(string);
        emptySign = euSign.CreateEmptySign(null, true);
        signer = euSign.CreateSigner(hash, true);
        sign = euSign.AppendSigner(signer, null, emptySign, true);
        info = euSign.VerifyHash(hash, sign);

        hash = euSign.HashData(string);
        emptySign = euSign.CreateEmptySign(string);
        signer = euSign.CreateSigner(hash);
        sign = euSign.AppendSigner(signer, ownCert, emptySign);
        info = euSign.VerifyDataInternal(sign);
        if (euSign.ArrayToString(info.GetData()) != string) {
            resultText.value += "Тестування функцій додавання підпису " +
                "завершилося з помилкою: " +
                "Перевірені дані не співпадають з даними, " +
                "що підписувалися";
            return false;
        }

        hash = euSign.HashData(array);
        emptySign = euSign.CreateEmptySign(array);
        signer = euSign.CreateSigner(hash);
        sign = euSign.AppendSigner(signer, ownCert, emptySign);
        info = euSign.VerifyDataInternal(sign);
        if (!utils.CompareArrays(info.GetData(), array)) {
            resultText.value += "Тестування функцій додавання підпису " +
                "завершилося з помилкою: " +
                "Перевірені дані не співпадають з даними, " +
                "що підписувалися";
            return false;
        }

    } catch (e) {
        resultText.value += "Тестування функцій додавання підпису " +
            "завершилося з помилкою: " + e.toString();

        return false;
    }

    return true;
},
testRawSign: function () {
    var string = "Data to sign, 1234567890";
    var array = new Uint8Array([0, 1, 2, 3, 4, 5, 6, 7, 8]);

    var ownCert, hash;
    var sign;
    var info;

    var resultText = document.getElementById('TestSignText');

    resultText.value += "Тестування функцій спрощеного підпису...\r\n";
    try {
        ownCert = euSignTest.getOwnCertificate(
            EU_CERT_KEY_TYPE_DSTU4145, EU_KEY_USAGE_DIGITAL_SIGNATURE);
        if (ownCert == null) {
            resultText.value += "Тестування функцій спрощеного підпису " +
                "завершилося з помилкою: " +
                "Сертифікат користувача для підпису за " +
                "алгоритмом ДСТУ-4145 не знайдено";
            return false;
        }

        hash = euSign.HashData(array);
        sign = euSign.RawSignHash(hash);
        info = euSign.RawVerifyHash(hash, sign);

        hash = euSign.HashData(string, true);
        sign = euSign.RawSignHash(hash, true);
        info = euSign.RawVerifyHash(hash, sign);

        sign = euSign.RawSignData(array);
        info = euSign.RawVerifyDataEx(null, array, sign);

        sign = euSign.RawSignData(string);
        info = euSign.RawVerifyDataEx(ownCert, string, sign);
    } catch (e) {
        resultText.value += "Тестування функцій спрощеного підпису " +
            "завершилося з помилкою: " + e.toString();

        return false;
    }

    return true;
},
testEnvelop: function () {
    var string = "Data to envelop, 1234567890";
    var array = new Uint8Array([0, 1, 2, 3, 4, 5, 6, 7, 8]);

    var ownCertInfo;
    var envelopedData, developedData;
    var info;

    var resultText = document.getElementById('TestEnvelopText');
    resultText.value = "Тестування функцій шифрування даних...\r\n";

    try {
        ownCertInfo = euSignTest.getOwnCertificateInfo(
            EU_CERT_KEY_TYPE_DSTU4145, EU_KEY_USAGE_KEY_AGREEMENT);
        if (ownCertInfo == null) {
            resultText.value += "Тестування функцій шифрування даних " +
                "завершилося з помилкою: " +
                "Сертифікат користувача для направленого шифрування не знайдено";
            return false;
        }

        envelopedData = euSign.EnvelopDataEx(
            [ownCertInfo.issuer], [ownCertInfo.serial],
            false, string, true);
        info = euSign.DevelopData(envelopedData);
        if (euSign.ArrayToString(info.GetData()) != string) {
            resultText.value += "Тестування функцій шифрування даних " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають з даними, що зашифровувалися";
            return false;
        }

        envelopedData = euSign.EnvelopDataEx(
            [ownCertInfo.issuer], [ownCertInfo.serial],
            true, array, false);
        info = euSign.DevelopData(envelopedData);
        if (!utils.CompareArrays(info.GetData(), array)) {
            resultText.value += "Тестування функцій шифрування даних " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають з даними, що зашифровувалися";
            return false;
        }

        resultText.value += "Тестування функцій щифрування " +
            "завершилося успішно\r\n";
    } catch (e) {
        resultText.value += "Тестування функцій щифрування " +
            "завершилося з помилкою: " + e.toString();

        return false;
    }

    if (!this.testSession())
        return false;

    return true;
},
testSession: function () {
    var string = "Data to envelop, 1234567890";
    var array = new Uint8Array([0, 1, 2, 3, 4, 5, 6, 7, 8]);

    var ownCert, ownCertInfo;

    var userSession = null, serverSession = null;
    var userCert, serverCert;
    var savedUserSession, savedServerSession;

    var resultText = document.getElementById('TestEnvelopText');
    resultText.value += "Тестування функцій захищеної сесії...\r\n";

    try {
        ownCert = euSignTest.getOwnCertificate(
            EU_CERT_KEY_TYPE_DSTU4145, EU_KEY_USAGE_KEY_AGREEMENT);
        ownCertInfo = euSignTest.getOwnCertificateInfo(
            EU_CERT_KEY_TYPE_DSTU4145, EU_KEY_USAGE_KEY_AGREEMENT);
        if (ownCert == null || ownCertInfo == null) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Сертифікат користувача для направленого шифрування не знайдено";
            return false;
        }

        userSession = euSign.ClientSessionCreateStep1(3600);
        serverSession = euSign.ServerSessionCreateStep1(
            3600, userSession.GetData());
        euSign.ClientSessionCreateStep2(userSession,
            serverSession.GetData());
        euSign.ServerSessionCreateStep2(serverSession,
            userSession.GetData());

        if (!euSign.SessionIsInitialized(userSession) ||
            !euSign.SessionIsInitialized(serverSession) ||
            !euSign.SessionCheckCertificates(userSession)) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Сесію не ініціалізовано";
            return false;
        }

        serverCert = euSign.SessionGetPeerCertificateInfo(
            userSession);
        userCert = euSign.SessionGetPeerCertificateInfo(
            serverSession);
        if (serverCert.GetIssuer() != userCert.GetIssuer() ||
            serverCert.GetIssuer() != ownCertInfo.GetIssuer() ||
            serverCert.GetSerial() != ownCertInfo.GetSerial()) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Сертифікати сервера та клієнта не співпадає";
            return false;
        }

        var _testEncryption = function (_ses1, _ses2, _testData) {
            var _isStr = ((typeof _testData) == 'string');
            var _envData, _devData;

            _envData = euSign.SessionEncrypt(
                _ses1, _testData, _isStr);
            _devData = euSign.SessionDecrypt(
                _ses2, _envData);
            if (_isStr) {
                if (_testData != euSign.ArrayToString(_devData))
                    return false;
            } else {
                if (!utils.CompareArrays(_testData, _devData))
                    return false;
            }

            _envData = euSign.SessionEncryptContinue(
                _ses1, _testData, _isStr);
            _devData = euSign.SessionDecryptContinue(
                _ses2, _envData);
            if (_isStr) {
                if (_testData != euSign.ArrayToString(_devData))
                    return false;
            } else {
                if (!utils.CompareArrays(_testData, _devData))
                    return false;
            }

            return true;
        }

        if (!_testEncryption(userSession, serverSession, string) ||
            !_testEncryption(userSession, serverSession, array)) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають";
            return false;
        }

        savedUserSession = euSign.SessionSave(userSession);
        savedServerSession = euSign.SessionSave(serverSession);

        euSign.SessionClose(userSession);
        userSession = null;
        euSign.SessionClose(serverSession);
        serverSession = null;

        userSession = euSign.SessionLoad(savedUserSession);
        serverSession = euSign.SessionLoad(savedServerSession);

        if (!_testEncryption(userSession, serverSession, string) ||
            !_testEncryption(userSession, serverSession, array)) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають";
            return false;
        }

        euSign.SessionClose(userSession);
        userSession = null;
        euSign.SessionClose(serverSession);
        serverSession = null;

        userSession = euSign.ClientDynamicKeySessionCreate(3600,
            ownCertInfo.GetIssuer(), ownCertInfo.GetSerial());
        serverSession = euSign.ServerDynamicKeySessionCreate(
            3600, userSession.GetData());

        if (!_testEncryption(userSession, serverSession, string) ||
            !_testEncryption(userSession, serverSession, array)) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають";
            return false;
        }

        euSign.SessionClose(userSession);
        userSession = null;
        euSign.SessionClose(serverSession);
        serverSession = null;

        userSession = euSign.ClientDynamicKeySessionCreate(3600, ownCert);
        serverSession = euSign.ServerDynamicKeySessionCreate(
            3600, userSession.GetData());

        if (!_testEncryption(userSession, serverSession, string) ||
            !_testEncryption(userSession, serverSession, array)) {
            resultText.value += "Тестування функцій захищеної сесії " +
                "завершилося з помилкою: " +
                "Розшифровані дані не співпадають";
            return false;
        }

        resultText.value += "Тестування функцій захищеної сесії " +
            "завершилося успішно\r\n";
    } catch (e) {
        resultText.value += "Тестування функцій захищеної сесії " +
            "завершилося з помилкою: " + e.toString();
        return false;
    } finally {
        try {
            if (userSession != null)
                euSign.SessionClose(userSession);
            if (serverSession != null)
                euSign.SessionClose(serverSession);
        } catch (e) {
        }
    }

    return true;
},
//-----------------------------------------------------------------------------
loadFilesFromLocalStorage: function (localStorageFolder, loadFunc) {
    if (!utils.IsStorageSupported())
        euSign.RaiseError(EU_ERROR_NOT_SUPPORTED);

    if (utils.IsFolderExists(localStorageFolder)) {
        var files = utils.GetFiles(localStorageFolder);
        for (var i = 0; i < files.length; i++) {
            var file = utils.ReadFile(
                localStorageFolder, files[i]);
            loadFunc(files[i], file);
        }
        return files;
    }
    else {
        utils.CreateFolder(localStorageFolder);
        return null;
    }
},
saveFileToModuleFileStorage: function (fileName, fileData) {
    var filesListName = null;
    try {
        var array = new Uint8Array(fileData);
        var folderName = null;

        if (fileName.indexOf(".cer") >= 0) {
            filesListName = 'SelectedCertsList';
            euSign.SaveCertificate(array);
            folderName = euSignTest.CertsLocalStorageName;
        } else if (fileName.indexOf(".p7b") >= 0) {
            euSign.SaveCertificates(array);
            folderName = euSignTest.CertsLocalStorageName;
        } else if (fileName.indexOf(".crl") >= 0) {
            filesListName = 'SelectedCRLsList';
            try {
                euSign.SaveCRL(true, array);
            } catch (e) {
                euSign.SaveCRL(false, array);
            }
            folderName = euSignTest.CRLsLocalStorageName;
        }

        if (folderName != null && utils.IsStorageSupported()) {
            utils.WriteFile(folderName, fileName, array);
        }
    } catch (e) {
        if (filesListName != null) {
            var filesList = document.getElementById(
                filesListName).getElementsByTagName("li");
            var filesNames = [];
            for (var i = 0; i < filesList.length; i++) {
                var fileNameInList = filesList[i].innerText;
                if (fileNameInList == fileName)
                    fileNameInList += ' (Не завантажено)'

                filesNames.push(fileNameInList);
            }

            euSignTest.setItemsToList(
                filesListName, filesNames);
        }
        alert(e);
    }

    euSignTest.updateCertList();
},
isCertificateExtension: function (fileName) {
    if ((fileName.indexOf(".cer") >= 0) ||
        (fileName.indexOf(".p7b") >= 0))
        return true;
    return false;
},
isCRLExtension: function (fileName) {
    if ((fileName.indexOf(".crl") >= 0))
        return true;
    return false;
},
//-----------------------------------------------------------------------------
mainMenuItemClicked: function (tab, pageId) {
    var selectedTab = document.getElementsByClassName(
        'MainPageMenuSelectedTab')[0];
    if (selectedTab == tab)
        return false;

    selectedTab.className = 'MainPageMenuTab';
    tab.className = 'MainPageMenuSelectedTab';
    tab.href = 'Tab';

    var selectedPage = document.getElementsByClassName(
        'MainPageMenuPanelSelected')[0];
    selectedPage.className = 'MainPageMenuPanel';

    selectedPage = document.getElementById(pageId);
    selectedPage.className = 'MainPageMenuPanelSelected';

    return false;
},
useInternalSignCheckBoxClick: function () {
    var intSignCheckbox =
        document.getElementById("InternalSignCheckbox");
    var addCertToIntSignCheckbox =
        document.getElementById("AddCertToInternalSignCheckbox");
    var signHashCheckbox =
        document.getElementById("SignHashCheckbox");
    var verifiedDataText =
        document.getElementById("VerifiedDataText");
    var fileWithSignSelectFile =
        document.getElementById("FileWithSign");

    if (intSignCheckbox.checked) {
        addCertToIntSignCheckbox.disabled = '';
        verifiedDataText.disabled = '';
        signHashCheckbox.disabled = 'disabled';
        fileWithSignSelectFile.disabled = 'disabled';
    } else {
        addCertToIntSignCheckbox.disabled = 'disabled';
        verifiedDataText.disabled = 'disabled';
        signHashCheckbox.disabled = '';
        fileWithSignSelectFile.disabled = '';
    }
},
signHashCheckBoxClick: function () {
    var intSignCheckbox =
        document.getElementById("InternalSignCheckbox");
    var signHashCheckbox =
        document.getElementById("SignHashCheckbox");
    if (signHashCheckbox.checked) {
        intSignCheckbox.disabled = 'disabled';
    } else {
        intSignCheckbox.disabled = '';
    }
},
privateKeyReaded: function (isReaded) {
    var enabled = '';
    var disabled = 'disabled';

    if (!isReaded) {
        enabled = 'disabled';
        disabled = '';
    }


    document.getElementById('CAsServersSelect').disabled = disabled;
    document.getElementById('PKeyFileName').disabled = disabled;

    document.getElementById('PKeyReadButton').title =
        isReaded ? 'Стерти' : 'Зчитати';
    document.getElementById('PKeyReadButton').innerHTML =
        isReaded ? 'Стерти' : 'Зчитати';

    document.getElementById('KeyReadedImage').style.display = isReaded ?
        "inline" : 'none';

    setPointerEvents(document.getElementById('PKeyShowOwnerInfoButton'), isReaded);//TUT
    setPointerEvents(document.getElementById('PKeyShowCertsInfoButton'), isReaded);//TUT

    document.getElementById('PKeyPassword').disabled = disabled;
    if (!isReaded) {
        document.getElementById('PKeyPassword').value = '';
        document.getElementById('PKeyPassword').disabled = 'disabled';
        document.getElementById('PKeyFileName').value = '';
        document.getElementById('PKeyFileInput').value = null;
        setPointerEvents(document.getElementById('PKeyReadButton'), false);
    }

    setPointerEvents(document.getElementById('SignDataButton'), isReaded);
},
setSelectPKCertificatesEvents: function () {
},
clearPrivateKeyCertificatesList: function () {
    euSignTest.privateKeyCerts = null;
},
setItemsToList: function (listId, items) {
    var output = [];
    for (var i = 0, item; item = items[i]; i++) {
        output.push('<li><strong>', item, '</strong></li>');
    }

    document.getElementById(listId).innerHTML =
        '<ul>' + output.join('') + '</ul>';
},
setFileItemsToList: function (listId, items) {
    var output = [];
    for (var i = 0, item; item = items[i]; i++) {
        output.push('<li><strong>', item.name, '</strong></li>');
    }

    document.getElementById(listId).innerHTML =
        '<ul>' + output.join('') + '</ul>';
}
    });

//=============================================================================

var euSignTest = EUSignCPTest();
var euSign = EUSignCP();
var utils = Utils(euSign);

//=============================================================================

function setPointerEvents(element, enable) {
    element.style.pointerEvents = enable ? "auto" : "none";
}

function setStatus(message) {
    if (message != '')
        message = '(' + message + '...)';
    document.getElementById('status').innerHTML = message;
}

function saveFile(fileName, array) {
    var blob = new Blob([array], { type: "application/octet-stream" });
    saveAs(blob, fileName);
}

function pageLoaded(obj) {
    FileModel = obj;
    var html = createTable();
}

function createTable() {
    var len = FileModel.files.length;
    FileModel.files.forEach(function (item, i, model) {
        var html = '<div class="content-list">';
        html += '<div class="content-list-edit">' +
            '<a data-title="Вивантажити"' +
            'data-tooltipright="1"' +
            'data-fileId ="' + i + '"' +
            'target = "_blank"' +
            'class="btn btn-icon btn-square icon-lg icon-import-down downloadFile">' +
            '</a>' + 
            '</div>';
            html += '<div class="content-list-item grid-container-nested">' +
            '<div class="grid">' +
            '<div class="grid-inner">' +
            '<p>';
        var names = '';
        if (item.isSystemFile == true) {
            names += "Файли досьє:<br/>";
        }
        item.names.forEach(function (fileName, j, files) {
            names += fileName + '<br/>';
        });
        html += names;
        html += '</p>' +
            '</div>' +
            '</div>' +
            '<div class="grid">' +
            '<div class="grid-inner">' +
            '<p id="signInfo' + i + '">' +
            '</p>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>';
        $('#result').append(html);
        euSignTest.verifyData(item.file, i, len == i + 1);
    });
    
}

$(document).on("click", ".downloadFile",
    function () {
        debugger;
        var fileId = $(this).attr("data-fileId");
        var file = FileModel.files[fileId];
        if (file.isSystemFile == true) {
            $.ajax({
                url: "/FileStore/DownloadArchive",
                data: { fileId: file.idFileStore },
                success: function(data) {
                    if (data.success == false)
                        alert("Виникла помилка серверу, спробуйте пізніше.");
                    else {
                        download('Досьє', data.file, 'data:application/zip;base64,');
                    }
                }
            });
        }
        else
            download(file.names[0].replace('.p7s', ''), file.file, 'data:application/pdf;base64,');
    });

function FileModel(responseString) {
    FileModel = JSON.parse(responseString);
}

function SignedFileModel() {
    var id;
    var files = [];
};

function loadFiles(url) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, false);
    xhr.send();

    if (xhr.status != 200) {
        alert("Помилка серверу. Перезавантажте сторінку");
    } else {
        FileModel(xhr.response);
    }
}


function EUSignCPModuleInitialized(isInitialized) {
    if (isInitialized)
        euSignTest.initialize();
    else
        alert("Криптографічну бібліотеку не ініціалізовано");
}

function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes;
    return date.getDate() + "." + (date.getMonth()+1) + "." + date.getFullYear() + "  " + strTime;
}


function download(filename, text, mimeType) {
    var element = document.createElement('a');
    element.setAttribute('href', mimeType + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}