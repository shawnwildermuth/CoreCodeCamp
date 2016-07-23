/// <binding Clean='default' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rename = require("gulp-rename");
var webpack = require("webpack");
var gutil = require("gulp-util");
var webpackConfig = require("./webpack.config.js");

gulp.task("npmTasks", function () {
  var libs = {
    "angular2": '@angular/**/*.*',
    "systemjs": 'systemjs/dist/*.*',
    "rxjs": 'rxjs/**/*.*',
    "core-js": "core-js/client/*.js",
    "zone.js": "zone.js/dist/*.js",
    "reflect-metadata": "reflect-metadata/*.js"
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

gulp.task("webpack", function (callback) {
  var myConfig = Object.create(webpackConfig);
  webpack(myConfig).run(function (err, stats) {
    if (err) throw new gutil.PluginError("webpack", err);
    gutil.log("[webpack]", stats.toString({
      colors: true
    }));
    callback();
  });
});

gulp.task("build", ["npmTasks", "min", "webpack"]);

gulp.task('default', ["npmTasks", "min"]);