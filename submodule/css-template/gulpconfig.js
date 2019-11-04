'use strict'
const os = require('os')
const browser = os.platform() === 'linux' ?
  'google chrome' :
  os.platform() === 'darwin' ? 'google chrome' : os.platform() === 'win32' ? 'chrome' : 'firefox'

module.exports = {
  port: 8000,
  browser: browser,
  paths: {
    src: './src',
    build: '../../src/App.WebHost/wwwroot',
    less: './src/less/**/**.less',
    mainLess: './src/less/main.less',
    js: './src/js/**/**.js',
    mainJs: './src/js/main.js',
    img: './src/img/**/*.*',
    fonts: './src/fonts/**/*.*',
    icons: './src/ico/**/*.*',
    jsLibs: './src/libs/**.js',
    jsLibsMain: './src/libs/vendor.js',
    //custom themes
    themeDefaultCost: './src/less/themes/default_cost',
    themeDarkGrey: './src/less/themes/dark_grey',
    themePrjMis: './src/less/themes/default_mis',
    themeDarkBlue: './src/less/themes/dark_blue',
    themeDIKLZ: './src/less/themes/default_diklz/'
  },
  browserSync: {
    proxy: 'http://localhost:8000/index.html',
    files: ['build/**/*.*'],
    browser: browser,
    port: 8001
  }
}