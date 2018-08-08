/// <binding Clean='default' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rename = require("gulp-rename");
var gutil = require("gulp-util");
var rimraf = require("gulp-rimraf");
var merge = require("merge-stream");


gulp.task("min", function () {
  return gulp.src([ "wwwroot/js/site.js"])
    .pipe(uglify())
    .pipe(rename("site.min.js"))
    .pipe(gulp.dest("wwwroot/lib/site/"));
});

// Dependency Dirs
var deps = {
  "jquery": {
    "jquery*.js": ""
  },
  "jquery-ui-dist": {
    "jquery-ui*.*": ""
  },
  "jquery-backstretch": {
    "jquery.backstretch.*": ""
  },
  "jquery-validation": {
    "jquery-validation*.*": ""
  },
  "jquery-validation-unobtrusive": {
    "*.js": ""
  },
  "jquery-waypoints": {
    "waypoints*.js": ""
  },
  "bootstrap": {
    "dist/**/*": ""
  },
  "font-awesome": {
    "*": ""
  },
  "typicons.font": {
    "src/font/*": ""
  },
  "wowjs": {
    "dist/*": ""
  },
  "retina.js": {
    "src/*": ""
  },
  "lodash": {
    "lodash*.*": ""
  },
  "moment": {
    "**/*.js": ""
  },
  "vue": {
    "dist/*": ""
  },
  "vee-validate": {
    "dist/*": ""
  },
  "vue-resource": {
    "dist/*": ""
  },
  "vue-router": {
    "dist/*": ""
  }
};

gulp.task("clean", function (cb) {
  return gulp.src('./wwwroot/lib', { read: false }) // much faster
    .pipe(rimraf());});

gulp.task("scripts", function () {

  var streams = [];

  for (var prop in deps) {
    console.log("Prepping Scripts for: " + prop);
    for (var itemProp in deps[prop]) {
      streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
        .pipe(gulp.dest("wwwroot/lib/" + prop + "/" + deps[prop][itemProp])));
    }
  }

  return merge(streams);

});

gulp.task('default', ["min", "scripts"]);