'use strict'
const os = require('os')
const browser = os.platform() === 'linux'
    ? 'google chrome'
    : os.platform() === 'darwin' ? 'google chrome' : os.platform() === 'win32' ? 'chrome' : 'firefox'

module.exports = {
    port: 8060,
    browser: browser,
    paths: {
        src: './wwwroot',
        build: './wwwroot/fe-ext',
        less: './wwwroot/less/**/**.less',
        mainLess: './wwwroot/less/**.less',
        js: './wwwroot/custom_js/**/**.js',
        mainJs: './wwwroot/custom_js/main.js',
        img: './wwwroot/images/**/*.*',
        fonts: './wwwroot/fonts/**/*.*'
        //jsLibs: './ui-template/libs/**.js',
        //jsLibsMain: './ui-template/libs/vendor.js',
        //templateCss: "D:/Projects/css-template/src"
    }
    //browserSync: {
    //  proxy: 'http://localhost:8000/index.html',
    //  files: ['build/**/*.*'],
    //  browser: browser,
    //  port: 8061
    //}
};
