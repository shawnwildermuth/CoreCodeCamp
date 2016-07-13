"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var forms_1 = require('@angular/forms');
var http_1 = require('@angular/http');
var router_1 = require('@angular/router');
var common_1 = require('@angular/common');
var talkService_1 = require("./talkService");
var TalksComponent_1 = require('./TalksComponent');
var routes_1 = require('./routes');
platform_browser_dynamic_1.bootstrap(TalksComponent_1.TalksComponent, [forms_1.disableDeprecatedForms(),
    forms_1.provideForms(),
    talkService_1.TalkService,
    http_1.HTTP_PROVIDERS,
    router_1.ROUTER_DIRECTIVES,
    routes_1.talkRoutes,
    { provide: common_1.LocationStrategy, useClass: common_1.HashLocationStrategy }
]);
//# sourceMappingURL=main.js.map