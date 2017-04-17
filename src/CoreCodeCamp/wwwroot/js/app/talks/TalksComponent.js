"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var talkEditor_1 = require("./talkEditor");
var talksForm_1 = require("./talksForm");
var TalksComponent = (function () {
    function TalksComponent() {
    }
    return TalksComponent;
}());
TalksComponent = __decorate([
    core_1.Component({
        selector: 'talks',
        template: "<router-outlet></router-outlet>",
        directives: [router_1.ROUTER_DIRECTIVES],
        precompile: [talkEditor_1.TalkEditor, talksForm_1.TalksForm]
    })
], TalksComponent);
exports.TalksComponent = TalksComponent;
//# sourceMappingURL=TalksComponent.js.map