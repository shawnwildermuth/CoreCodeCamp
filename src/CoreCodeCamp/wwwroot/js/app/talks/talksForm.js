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
// talksForm.ts
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var http_1 = require('@angular/http');
var talkService_1 = require("./talkService");
var TalksForm = (function () {
    function TalksForm(http, talkService, router) {
        this.http = http;
        this.talkService = talkService;
        this.router = router;
    }
    TalksForm.prototype.onEdit = function (talk) {
        this.router.navigate(["/edit/" + talk.id]);
    };
    TalksForm.prototype.onDelete = function (talk) {
        this.talkService.delete(talk);
    };
    TalksForm = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "talksForm.html",
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [http_1.Http, talkService_1.TalkService, router_1.Router])
    ], TalksForm);
    return TalksForm;
}());
exports.TalksForm = TalksForm;
//# sourceMappingURL=talksForm.js.map