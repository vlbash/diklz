
String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};
String.prototype.isEmpty = function () {
    return (this.length === 0 || !this.trim());
};

var Utils = {
    binaryStringToBase64: function (v) {
        return window.btoa(v);
    },
    arrayBufferToBase64: function (buffer) {
        return window.btoa(String.fromCharCode.apply(String, new Uint8Array(buffer)));
    },
    setVisible: function (el, val) {
        el.style.display = val ? 'block' : 'none'; //'inline'
    },
    find: function (id, parent) {
        return parent ? parent.querySelector('#' + id) : document.getElementById(id);
    },
    addOption: function (selector, text, value) {
        var option = document.createElement("option");
        option.text = text;  //innerHTML? 
        option.value = value;
        selector.appendChild(option);
    },
    addCertInfoOption: function (selector, info, index) {
        Utils.addOption(selector, info.subject + ", №" + info.serial, index);
    },
    clearOptions: function (selector) {
        var length = selector.options.length;
        for (var i = 0; i < length; i++) {
            selector.options[i] = null;
        }
    },
    getSelectedValue: function (selector) {
        var value = selector.options[selector.selectedIndex].value;
        return value;
    },
    // next functions b64EncodeUnicode() and b64DecodeUnicode() deal with unicode problem
    // described here https://developer.mozilla.org/ru/docs/Web/API/WindowBase64/Base64_encoding_and_decoding#The_Unicode_Problem
    b64EncodeUnicode: function (str) {
        // first we use encodeURIComponent to get percent-encoded UTF-8,
        // then we convert the percent encodings into raw bytes which
        // can be fed into btoa.
        return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g,
            function toSolidBytes(match, p1) {
                return String.fromCharCode('0x' + p1);
            })
        );
    },
    b64DecodeUnicode: function (str) {
        // Going backwards: from bytestream, to percent-encoding, to original string.
        return decodeURIComponent(atob(str).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }
};