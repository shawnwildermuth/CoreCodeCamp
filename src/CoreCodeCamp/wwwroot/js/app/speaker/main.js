"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var speakerForm_1 = require('./speakerForm');
var forms_1 = require('@angular/forms');
var http_1 = require('@angular/http');
var imageUploadService_1 = require("../common/imageUploadService");
platform_browser_dynamic_1.bootstrap(speakerForm_1.SpeakerForm, [forms_1.disableDeprecatedForms(),
    forms_1.provideForms(),
    http_1.HTTP_PROVIDERS,
    imageUploadService_1.ImageUploadService]);
//# sourceMappingURL=main.js.map