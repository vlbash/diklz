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

function lessCustomize(customPath) {
    return gulp.src(customPath)
        .pipe(less())
        .pipe(gulp.dest('./wwwroot/fe-ext/abstracts'));
}


//js tasks

function js() {
    return gulp.src(config.paths.mainJs)
        .pipe(rigger())
        .pipe(gulp.dest(config.paths.build + '/javascripts'));
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
        .pipe(gulp.dest(config.paths.build + '/javascripts'));
}

//img transfer && minimize *.png
function img() {
    return gulp.src(config.paths.img)
        .pipe(imagemin())
        .pipe(gulp.dest(config.paths.build + '/images'));

}

//fonts transfer
function fonts() {
    return gulp.src(config.paths.fonts)
        .pipe(gulp.dest(config.paths.build + '/fonts'));
}

//watchers
function watcher() {
    gulp.watch(config.paths.less, gulp.series(css, cssClean));
    gulp.watch(config.paths.mainLess, gulp.series(css, cssClean));
    gulp.watch(config.paths.js, gulp.series(js, jsClean));
    gulp.watch(config.paths.mainJs, gulp.series(js, jsClean));

};

gulp.task('css', css);
gulp.task('cssClean', cssClean);
gulp.task('js', js);
gulp.task('jsClean', jsClean);
gulp.task('img', img);
gulp.task('fonts', fonts);
gulp.task('customize', lessCustomize);

gulp.task("watch", gulp.parallel(watcher));


gulp.task('default', gulp.series('css', 'js', 'cssClean', 'jsClean', 'watch'));


gulp.task('transfer', gulp.series('img', 'fonts', 'js', 'css'));
gulp.task('test', gulp.series('img', 'fonts', 'css', 'js'));
gulp.task('build', gulp.series('img', 'fonts', 'cssClean', 'jsClean'));