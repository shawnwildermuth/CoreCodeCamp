/// <binding Clean='default' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var uglifyCss = require('gulp-uglifycss');
var concat = require('gulp-concat');
var rename = require("gulp-rename");
var webpack = require("webpack");
var gutil = require("gulp-util");
var webpackConfig = require("./webpack.config.js");

gulp.task("npmTasks", function () {
  var libs = {
    "core-js": "core-js/client/*.js",
    "zone.js": "zone.js/dist/*.js",
    "reflect-metadata": "reflect-metadata/*.js",
    "jquery": "jquery/dist/*.js",
    "jquery-validation": "jquery-validation/dist/*.js",
    "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/*.js",
    "jquery.backstretch": "jquery.backstretch/jquery.backstretch*.js",
    "wowjs": "wowjs/dist/*.js",
    "retina.js": "retina.js/src/*.js",
    "waypoints": "waypoints/lib/jquery*.js",
    "bootstrap": "bootstrap/dist/**/*.*",
    "font-awesome": "font-awesome/**/*.*",
    "typicons.font": "typicons.font/*.*"
  };

  for (var name in libs) {
    gulp.src("node_modules/" + libs[name])
      .pipe(gulp.dest("wwwroot/lib/" + name));
  }

  return;
});

gulp.task("min", function () {
  return gulp.src([ "wwwroot/js/site.js"])
    .pipe(uglify())
    .pipe(rename("site.min.js"))
    .pipe(gulp.dest("wwwroot/lib/site/"));
});

gulp.task("css", function () {
  var srcs = [
    "wwwroot/template/css/animate.css",
    "wwwroot/template/css/style.css",
    "wwwroot/template/css/media-queries.css",
    "wwwroot/css/site.css",

  ];

  gulp.src(srcs)
    .pipe(concat("corecodecamp.css"))
    .pipe(gulp.dest("wwwroot/lib"));

  gulp.src(srcs)
    .pipe(concat("corecodecamp.min.css"))
    .pipe(uglifyCss())
    .pipe(gulp.dest("wwwroot/lib"));
});

gulp.task('default', ["npmTasks", "min", "css"]);