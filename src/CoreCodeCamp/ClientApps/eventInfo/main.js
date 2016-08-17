"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
// Turn on Production Mode
var buildType_1 = require("../common/buildType");
buildType_1.buildType();
var eventInfoForm_1 = require('./eventInfoForm');
var EventModule = (function () {
    function EventModule() {
    }
    EventModule = __decorate([
        core_1.ngModule({
            bootstrap: eventInfoForm_1.EventInfoForm,
        }), 
        __metadata('design:paramtypes', [])
    ], EventModule);
    return EventModule;
}());
platform_browser_dynamic_1.platformBrowserDynamic().bootstrapModule(EventModule);
//EventInfoForm,
//  [disableDeprecatedForms(),
//    provideForms(),
//    HTTP_PROVIDERS,
//    DataService]); 
//# sourceMappingURL=main.js.map