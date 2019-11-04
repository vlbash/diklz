'use strict';

const config = require('./gulpconfig.js'),
  gulp = require('gulp'),
  rigger = require('gulp-rigger'),
  less = require('gulp-less'),
  autoprefixer = require('gulp-autoprefixer'),
  cleancss = require('gulp-clean-css'),
  sourcemaps = require('gulp-sourcemaps'),
  uglify = require('gulp-uglify'),
  imagemin = require('gulp-imagemin'),
  gutil = require('gulp-util'),
  rename = require("gulp-rename");


//css tasks

function css() {
  return gulp.src(config.paths.mainLess)
    .pipe(sourcemaps.init())
    .pipe(less())
    .on('error', function (err) {
      const type = err.type || '';
      const message = err.message || '';
      const extract = err.extract || [];
      const line = err.line || '';
      const column = err.column || '';
      gutil.log(gutil.colors.red.bold('[Less error]') + ' ' + gutil.colors.bgRed(type) + ' (' + line + ':' + column + ')');
      gutil.log(gutil.colors.bold('message:') + ' ' + message);
      gutil.log(gutil.colors.bold('codeframe:') + '\n' + extract.join('\n'));
      this.emit('end');
    })
    .pipe(autoprefixer())
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(config.paths.build + '/stylesheets'));
};

function lessCustomize(customPath) {
  return gulp.src(customPath + '/**.less')
    .pipe(gulp.dest('./src/less/abstracts'));
};

function cssClean() {
  return gulp.src(config.paths.mainLess)
    .pipe(less())
    .on('error', function (err) {
      const type = err.type || '';
      const message = err.message || '';
      const extract = err.extract || [];
      const line = err.line || '';
      const column = err.column || '';
      gutil.log(gutil.colors.red.bold('[Less error]') + ' ' + gutil.colors.bgRed(type) + ' (' + line + ':' + column + ')');
      gutil.log(gutil.colors.bold('message:') + ' ' + message);
      gutil.log(gutil.colors.bold('codeframe:') + '\n' + extract.join('\n'));
      this.emit('end');
    })
    .pipe(autoprefixer())
    .pipe(cleancss({
      compatibility: 'ie8'
    }))
    .pipe(rename({
      suffix: '.min'
    }))
    .pipe(gulp.dest(config.paths.build + '/stylesheets'));
};

//js tasks

function js() {
  return gulp.src(config.paths.mainJs)
    .pipe(rigger())
    .pipe(gulp.dest(config.paths.build + '/javascripts'))
};



function jsClean() {
  return gulp.src(config.paths.mainJs)
    .pipe(rigger())
    .pipe(uglify({
      mangle: true,
      output: {
        beautify: false
      }
    }))
    .pipe(rename({
      suffix: '.min'
    }))
    .pipe(gulp.dest(config.paths.build + '/javascripts'))
};

function jsLibs() {
  return gulp.src(config.paths.jsLibsMain)
    .pipe(rigger())
    .pipe(uglify())
    .pipe(rename({
      suffix: '.min'
    }))
    .pipe(gulp.dest(config.paths.build + '/javascripts'))

};



//img transfer && minimize *.png
function img() {
  return gulp.src(config.paths.img)
    .pipe(imagemin())
    .pipe(gulp.dest(config.paths.build + '/img'))

};

//fonts transfer
function fonts() {
  return gulp.src(config.paths.fonts)
    .pipe(gulp.dest(config.paths.build + '/fonts'))
};

function icons() {
  return gulp.src(config.paths.icons)
    .pipe(gulp.dest(config.paths.build + '/ico'))
};

function customizeIcons(customPath) {
  return gulp.src(customPath + '/custom_icons/**')
    .pipe(gulp.dest('./src/fonts/custom_icons'))
};
//watchers
function watcher() {
  gulp.watch(config.paths.less, gulp.series(css, cssClean));
  gulp.watch(config.paths.mainLess, gulp.series(css, cssClean));
  gulp.watch(config.paths.js, gulp.series(js, jsClean));
  gulp.watch(config.paths.mainJs, gulp.series(js, jsClean));
  gulp.watch(config.paths.jsLibs, gulp.series(jsLibs));
  gulp.watch(config.paths.jsLibsMain, gulp.series(jsLibs));
};

gulp.task('css', css);
gulp.task('cssClean', cssClean);
gulp.task('js', js);
gulp.task('jsClean', jsClean);
gulp.task('jsLibs', jsLibs);
gulp.task('img', img);
gulp.task('fonts', fonts);
gulp.task('icons', icons);
gulp.task('customize', lessCustomize);
gulp.task('customizeIcons', customizeIcons);

gulp.task("watch", gulp.parallel(watcher));


//step 1 init task with custom palette&variables
gulp.task('build-default', gulp.series(function () {
  return lessCustomize(config.paths.themeDefaultCost),
    customizeIcons(config.paths.themeDefaultCost)
}));
gulp.task('build-dark-blue', gulp.series(function () {
  return lessCustomize(config.paths.themeDarkBlue),
    customizeIcons(config.paths.themeDarkBlue)
}));
gulp.task('build-dark-grey', gulp.series(function () {
  return lessCustomize(config.paths.themeDarkGrey),
    customizeIcons(config.paths.themeDarkGrey)
}));
gulp.task('build-mis', gulp.series(function () {
  return lessCustomize(config.paths.themePrjMis),
    customizeIcons(config.paths.themePrjMis)
}));
gulp.task('build-diklz', gulp.series(function () {
  return lessCustomize(config.paths.themeDIKLZ),
    customizeIcons(config.paths.themeDIKLZ)
}));

//step 2 init build task
gulp.task('build', gulp.series('img', 'fonts', 'icons', 'cssClean', 'jsLibs', 'jsClean'));

//step 3 init gulp default task
gulp.task('default', gulp.series('css', 'jsLibs', 'js', 'cssClean', 'jsClean', 'watch'));

gulp.task('transfer', gulp.series('img', 'fonts','icons', 'js', 'css', 'jsLibs'));
gulp.task('test', gulp.series('img', 'fonts','icons', 'css', 'jsLibs', 'js'));
gulp.task('build', gulp.series('img', 'fonts', 'icons', 'cssClean', 'jsLibs', 'jsClean'));