"use strict";
// main.ts
var platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
var http_1 = require("@angular/http");
var forms_1 = require("@angular/forms");
// Turn on Production Mode
var buildType_1 = require("../common/buildType");
buildType_1.buildType();
var eventInfoForm_1 = require("./eventInfoForm");
var dataService_1 = require("../common/dataService");
platform_browser_dynamic_1.bootstrap(eventInfoForm_1.EventInfoForm, [forms_1.disableDeprecatedForms(),
    forms_1.provideForms(),
    http_1.HTTP_PROVIDERS,
    dataService_1.DataService]);
//# sourceMappingURL=main.js.map