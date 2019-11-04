
var UacKey = function(key, pass, keyId, cert, info) {
    this.key = key;
    this.pass = pass;
    this.keyId = keyId;
    this.cert = cert;
    this.info = info;
};
UacKey.prototype.isKeyPair = function () { return this.key && this.cert; };
UacKey.prototype.isCertOnly = function () { return this.сert && !this.key; };
UacKey.prototype.isKeyOnly = function () { return !this.сert && this.key; };


var UacKeyStore = function(data, pass) {
    this.data = data;
    this.pass = pass;
    this.keys = [];
};

UacKeyStore.prototype.indexOfCert = function(cert) {
    var len = this.keys.length; 
    for(var i=0; i<len; i++) {
        if (this.keys[i].cert && this.keys[i].cert === cert) {
            return i;
        }
    }
    return -1;
};

var UacSignInfo = function(signerInfo, signedTime, timestamp)
{
    this.signerInfo = signerInfo;
    this.signedTime = signedTime;
    this.timestamp = timestamp;
};

var UacCodes = {
    success             : 0,
    generalError        : -1, 
    libError            : 1,  
    keyStroreError      : 2,  
    certStatusError     : 3,
    
    notFoundSigner      : 4,
    verifyError         : 5,
    notFoundRecipient   : 6,
    notFoundOriginator  : 7,
    
    connError           : 10,
    connResponseError   : 11
};

var UacPlugin = function(url) {
    
    function _request(cmd, params, success, error) {
        if (url) {
            var XHR = ("onload" in new XMLHttpRequest()) ? XMLHttpRequest : XDomainRequest;
            var xhr = new XHR();
            xhr.open('POST', url, true);
            xhr.onreadystatechange = function() {
                if (xhr.readyState === xhr.DONE) {
                    if (xhr.status === 200) {
                        var res = JSON.parse(this.responseText);
                        if (res.code===0) {
                            success(res.data);
                        } else {
                            error(UacCodes.connResponseError, res);
                        }
                    } else {
                        console.log("Error:" + xhr.status + " " + xhr.statusText);
                        error(UacCodes.connError, xhr);
                    }
                }
            };
            var req = 'cmd=' + encodeURIComponent(cmd);
            for(var key in params) {
                req += '&' + key + "=" + encodeURIComponent(params[key]);
            }
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr.send(req);
        } else {
            error(UacCodes.connError, "Server URL is not defined!");
        }
    }
      
    function getCertInfo(cert) {
        var ci = exec( Uac.certLoad(cert) );
        var info = {
            issuer: ci.getIssuer(),
            subject: ci.getSubject().getCommonName(),
            serial: ci.getSerialNumber(),
            keyId: ci.getSubjectKeyIdentifier(),
            urlOCSP: ci.getAccessOCSP(),
            keyAlg: ci.getPkAlg(),
            validity: {
                notBefore: _strToDate(ci.getValidBefore()),
                notAfter: _strToDate(ci.getValidAfter())
            }
        };
        function mask(v) {
            this.isContain = function (f) {
                return (v & f) !== 0;
            };
        };
        var ku = new mask(ci.getKeyUsage());
        info.forSign = ku.isContain(Uac.KeyUsage.kuDigitalSignature);
        info.forEncrypt =  
                ku.isContain(Uac.KeyUsage.kuKeyAgreement) ||
                ku.isContain(Uac.KeyUsage.kuKeyEncipherment) ||
                ku.isContain(Uac.KeyUsage.kuDataEncipherment);
        var eku = new mask(ci.getExtKeyUsage());
        info.isCA = ku.isContain(Uac.KeyUsage.kuKeyCertSign) || ku.isContain(Uac.KeyUsage.kuCrlSign);
        info.isOCSP = eku.isContain(Uac.ExtKeyUsage.ekuOCSP);
        info.isTSP = eku.isContain(Uac.ExtKeyUsage.ekuTSP);
        info.isUser = !info.isCA && !info.isOCSP && !info.isTSP;
        return info;
    }
    
    function _strToDate(s) {
        if (s) {
            var t = Uac.stringToTime(s);
            if (t) {
                return new Date(t);
            }
        }
        return null;
    };
   
    function CryptoError(code, message) {
        Error.call(this, message);
        this.name = "CryptoError";
        this.code = code;
        this.message = message;
        if (Error.captureStackTrace) {
            Error.captureStackTrace(this, CryptoError);
        } else {
            this.stack = (new Error()).stack;
        }
    };

    function exec(res) {
        var lastError = Uac.getLastError();
        var code = lastError.getCode();
        if (code === 0) {
            return res;
        } else {
            var msg = lastError.getMessage();
            console.log("last error: ", lastError);
            lastError.reset();
            throw new CryptoError(code, msg);
        }
    };

    function procError(e, onerror) {
        console.log("Error:", e);
        if (onerror) {
            if (e instanceof CryptoError) {
                onerror(UacCodes.libError, e);
            } else {
                onerror(UacCodes.generalError, e);
            }
        }
    };
    
    function _loadKeyStore(keyData, pass, success, error) {
        try {
            var data = keyData.data;
            if (keyData.name) {
                if (keyData.name.toLowerCase().endsWith(".jks")) {
                    data = Uac.jksToPfx(data, pass);
                }
            }
            var pfxKey = exec(new Uac.PfxKey(data, pass));
            var store = new UacKeyStore(data, pass);
            if (pfxKey.hasPendingKey()) {
                store.keys.push(new UacKey(data, pass));
                success(store);
            } else {
                var keys = pfxKey.getKeys();
                var len = keys.length;
                for(var i=0; i<len; i++) {
                    var key = keys[i];
                    var cert = key.getCert();
                    var info = cert ? getCertInfo(cert) : null;
                    var keyId = info ? info.keyId : key.getId();
                    store.keys.push(new UacKey(key.getKey(), key.getPwd(), keyId, cert, info));
                }
                keys = pfxKey.getCertsAlone();
                len = keys.length;
                if (len) {
                    var certs = [];
                    for(var i=0; i<len; i++) {
                        var key = keys[i];
                        var cert = key.getCert();
                        var info = getCertInfo(cert);
                        certs.push(new UacKey(null, null, info.keyId, cert, info));
                    }
                    _keyStoreAddCerts(store, certs, true, 
                        function(store) { success(store); }, 
                        error
                    );
                } else {
                    success(store);
                }
            }
        } catch(e) {
            if (e instanceof CryptoError) {
                error(UacCodes.keyStroreError, e);
            } else {
                procError(e, error);
            }
        }
    };
    
    function _keyStoreAddCerts(store, certKeys, saveAll, success, error) {
        try {
            var certKeysLen = certKeys.length;
            var changes = 0;
            for(var i=0; i<certKeysLen; i++) {
                var certKey = certKeys[i];
                var cert = certKey.cert;
                if (cert && store.indexOfCert(cert) === -1) {
                    var storeKeysLen = store.keys.length;
                    for(var j=0; j<storeKeysLen; j++) {
                        var key = store.keys[j];
                        if (key.isKeyOnly()) {
                            if (key.keyId && key.keyId != certKey.info.keyId) {
                                continue;
                            }
                            var keyPair = exec(new Uac.KeyPairInfo(certKey.cert, key.key, key.pass));
                            if (Uac.keyPairVerify(keyPair)) {
                                key.cert = certKey.cert;
                                key.info = certKey.info;
                                key.keyId = certKey.info.keyId;
                                changes++;
                                break;
                            } else {
                                var lastError = Uac.getLastError();
                                var code = lastError.getCode();
                                if (code != 0) {
                                    var msg = lastError.getMessage();
                                    lastError.reset();
                                    if (code != 2004) {
                                        throw new CryptoError(code, msg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (saveAll) {
                for(var i=0; i<certKeysLen; i++) {
                    var certKey = certKeys[i];
                    var cert = certKey.cert;
                    if (cert && store.indexOfCert(cert) === -1) {
                        store.keys.push(certKey);
                        changes++;
                    }
                }
            }
            success(store, changes);
        } catch(e) {
            procError(e, error);
        }
    }
    
    function _checkCertStatus(key, date, success, error) {
        try {
            if (url) {
                var params = { cert: key.cert};
                if (date && date instanceof Date) {
                    params.date = Uac.timeToString(date.getTime());
                }
                _request("status", params,
                        function(r) {
                            if (r.status === 0) {
                                success();
                            } else {
                                error(UacCodes.certStatusError);
                            }
                        }, 
                        error);
            } else {
                success();
            }
        } catch(e) {
            procError(e, error);
        }
    };
    
    function _sign_ts(data, key, options, success, error) {
        try {
            var hash = exec( Uac.hashCreate(data, key.cert) );
            _request("getts", { hash: hash }, 
                    function(r) { _sign(data, key, options, r.ts, success, error); },
                    error);
        } catch(e) {
            procError(e, error);
        }
    }
    
    function _sign(data, key, options, ts, success, error) {
        try {
            var si = new Uac.SignatureInfo();
            if (options & Uac.UAC_OPTION_INCLUDE_SIGNED_TIME) {
                si.setSigningTime(Uac.timeToString(new Date().getTime()));
            }
            if (ts) {
                si.setContentTS(ts);
            }
            var sd = exec(new Uac.SignedData(data, key.key, key.pass, key.cert, si, 
                    options ? options : Uac.UAC_OPTIONS_DEFAULT));
            var env = exec(sd.getBytes());
            success(env);
        } catch(e) {
            procError(e, error);
        }
    };
    
    function _signedDataGetContent(env, success, error) {
        try {
            var sdi = exec( Uac.signedDataLoad(env) );
            success(sdi.content);
        } catch(e) {
            procError(e, error);
        }
    };
    
    function _verify(env, data, success, error) {
        try {
            var sd = exec(new Uac.SignedData(env));
            var sdi = sd.getInfo();
            var signCount = sdi.getSignatures().length;
            var vrArr = exec(sd.verifyExternal(data));
            var signInfos = [];
            var checkArr = [];
            for(var i=0; i<signCount; i++) {
                if (!(vrArr[i] & Uac.SignedData.vrContentSignatureOk)) {
                    throw new CryptoError(UacCodes.verifyError, "Verify signature error!");
                }
                var si = sdi.getSignatures()[i];
                var certsCount = sdi.getCerts().length;
                var signerCert;
                for(var c=0; c<certsCount; c++) {
                    var cert = sdi.getCerts()[c];
                    if ( exec(Uac.certMatch(cert, si.getSignerRef())) ) {
                        signerCert = cert;
                        break;
                    }
                }
                var signInfo = new UacSignInfo(getCertInfo(signerCert), _strToDate(si.getSigningTime()));
                checkArr.push({cert:signerCert, date:si.getSigningTime()});
                var ts = si.getContentTS(); 
                if (ts) {
                    if (!(vrArr[i] & Uac.SignedData.vrContentTSOk)) {
                        throw new CryptoError(UacCodes.verifyError, "Verify TS error!");
                    }
                    var tsi = Uac.tsResponseLoad(ts);
                    signInfo.timestamp = _strToDate(tsi.getGenTime());
                    checkArr.push({cert:tsi.getIncludedCert(), date:si.getSigningTime()});
                }
                signInfos.push(signInfo);
            }
            success(signInfos, data);
        } catch(e) {
            procError(e, error);
        }
    };
    
    function _encrypt(data, key, recipients, options, success, error) {
        try {
            var env = exec( Uac.encrypt(data, key.key, key.pass, key.cert, recipients, null, null, 
                    options ? options : Uac.UAC_OPTIONS_DEFAULT) );
            success(env);
        } catch(e) {
            procError(e, error);
        }
    }

    function _findKeyByRef(store, certRef) {
        var len = store.keys.length;
        for(var i=0; i<len; i++) {
            var key = store.keys[i];
            if (key.cert && exec( Uac.certMatch(key.cert, certRef) )) {
                return key;
            }
        }
        return null;
    }
    
    function _decrypt(env, store, success, error) {
        try {
            var envInfo = exec( Uac.envelopeLoad(env) );
            var exchs = envInfo.getExchanges();
            for(var i=0; i<exchs.length; i++) {
                var exch = exchs[i];
                var recipient = _findKeyByRef(store, exch.getRecipient());
                if (recipient && recipient.isKeyPair()) {
                    var originator = null;
                    if (envInfo.getCertCount() === 0) {
                        var it = _findKeyByRef(store, exch.getOriginator());
                        if (it) {
                            originator = it.cert;
                        } 
                    }
                    var data = exec( Uac.decrypt(env, recipient.key, recipient.pass, recipient.cert, originator, null) ); 
                    success(data);
                    return;
                }
            }
            error(UacCodes.notFoundRecipient);
        } catch(e) {
            procError(e, error);
        }
    }
    
    return {
        version: '1.3',
        getUrl: function() {
            return url;
        },
        loadKeyStore: function(data, pass, success, error) {
            _loadKeyStore(data, pass, success, error);
        },
        loadCert: function(data) {
            try {
                var info = getCertInfo(data);
                return new UacKey(null, null, info.keyId, data, info);
            } catch(e) {
                return null;
            }
        },
        keyStoreAddCerts: function(store, certKeys, saveAll, success, error) {
            _keyStoreAddCerts(store, certKeys, saveAll, success, error);
        },
        sign: function(data, key, options, addTimestamp, success, error) {
            _checkCertStatus(key, 
                    null,
                    function() {
                        if (addTimestamp) {
                            _sign_ts(data, key, options, success, error);
                        } else {
                            _sign(data, key, options, null, success, error);
                        }
                    }, 
                    error);
        },
        signedDataGetContent: function(env, success, error) {
            _signedDataGetContent(env, success, error);
        },
        verify: function(env, data, success, error) {
            _verify(env, data, success, error);
        },
        encrypt: function(data, key, recipients, options, success, error) {
            _checkCertStatus(key, new Date(), 
                    function() { _encrypt(data, key, recipients, options, success, error); }, 
                    error);
        },
        decrypt: function(env, store, success, error) {
            _decrypt(env, store, success, error);
        }
    };
};