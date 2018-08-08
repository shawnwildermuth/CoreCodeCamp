/// <binding Clean='default' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rename = require("gulp-rename");
var gutil = require("gulp-util");

gulp.task("min", function () {
  return gulp.src([ "wwwroot/js/site.js"])
    .pipe(uglify())
    .pipe(rename("site.min.js"))
    .pipe(gulp.dest("wwwroot/lib/site/"));
});

// Dependency Dirs
var deps = {
  "jquery": {
    "dist/*": ""
  },
  "bootstrap": {
    "dist/**/*": ""
  },
  "lodash": {
    "lodash*.*": ""
  },
  "respond.js": {
    "dest/*": ""
  },
  "tether": {
    "dist/**/*": ""
  },
  "vue": {
    "dist/*": ""
  },
  "vee-validate": {
    "dist/*": ""
  },
  "vue-resource": {
    "dist/*": ""
  }
};

gulp.task("clean", function (cb) {
  return rimraf("wwwroot/lib/", cb);
});

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