"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var http_1 = require('@angular/http');
var forms_1 = require('@angular/forms');
var sponsorForm_1 = require('./sponsorForm');
var sponsorService_1 = require("./sponsorService");
var imageUploadService_1 = require('../common/imageUploadService');
platform_browser_dynamic_1.bootstrap(sponsorForm_1.SponsorForm, [forms_1.disableDeprecatedForms(),
    forms_1.provideForms(),
    http_1.HTTP_PROVIDERS,
    sponsorService_1.SponsorService,
    imageUploadService_1.ImageUploadService]);
//# sourceMappingURL=main.js.map