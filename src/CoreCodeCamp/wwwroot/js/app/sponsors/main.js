"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var http_1 = require('@angular/http');
var forms_1 = require('@angular/forms');
var sponsorForm_1 = require('./sponsorForm');
var dataService_1 = require("../common/dataService");
var imageUploadService_1 = require('../common/imageUploadService');
platform_browser_dynamic_1.bootstrap(sponsorForm_1.SponsorForm, [forms_1.disableDeprecatedForms(),
    forms_1.provideForms(),
    http_1.HTTP_PROVIDERS,
    imageUploadService_1.ImageUploadService,
    dataService_1.DataService]);
//# sourceMappingURL=main.js.map