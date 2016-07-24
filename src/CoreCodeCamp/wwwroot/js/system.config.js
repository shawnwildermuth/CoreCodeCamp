(function (global) {

  // map tells the System loader where to look for things
  var map = {
    '@angular': '/lib/angular2',
    'rxjs': '/lib/rxjs'
  };

  // packages tells the System loader how to load when no filename and/or no extension
  var packages = {
    'rxjs': { defaultExtension: 'js' },
  };

  // Our Components
  ["users", "speaker", "talks", "sponsors", "schedule", "eventInfo"]
    .forEach(function (c) {
      map[c] = '/js/app/' + c;
      packages[c] = { main: 'main.js', defaultExtension: 'js' };
    }); 

  ["fileUploadService"].forEach(function (c) {
    map[c] = '/js/app/common/';
    packages[c] = { defaultExtension: 'js' };
  });


  var ngPackageNames = [
    'common',
    'compiler',
    'core',
    'forms',
    'http',
    'platform-browser',
    'platform-browser-dynamic',
    'router',
    'router-deprecated',
    'upgrade',
  ];


  // Bundled (~40 requests):
  function packUmd(pkgName) {
    packages['@angular/' + pkgName] = { main: 'bundles/' + pkgName + '.umd.js', defaultExtension: 'js' };
  }

  // Add package entries for angular packages
  ngPackageNames.forEach(packUmd);

  var config = {
    map: map,
    packages: packages
  };

  System.config(config);

})(this);