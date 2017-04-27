/// <binding Clean='default' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rename = require("gulp-rename");
var gutil = require("gulp-util");

gulp.task("min", function () {
  return gulp.src([ "wwwroot/js/site.js"])
    //.pipe(uglify())
    //.pipe(rename("site.min.js"))
    .pipe(gulp.dest("wwwroot/lib/site/"));
});

gulp.task("app", function () {
    return gulp.src(["wwwroot/app/*.js"])
        .pipe(concat("app.js"))
        //.pipe(uglify())
        //.pipe(rename("app.min.js"))
        .pipe(gulp.dest("wwwroot/lib/site/"));
});

gulp.task('default', ["min", "app"]);