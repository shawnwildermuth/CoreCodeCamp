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
var router_2 = require('@angular/router');
var talkService_1 = require("./talkService");
var Talk = (function () {
    function Talk() {
    }
    return Talk;
}());
var TalkEditor = (function () {
    function TalkEditor(route, talkService) {
        this.route = route;
        this.talkService = talkService;
        this.isBusy = false;
        this.error = "";
    }
    TalkEditor.prototype.ngOnInit = function () {
        var _this = this;
        this.sub = this.route.params.subscribe(function (params) {
            var id = params['id'];
            if (id == "new") {
                _this.model = new Talk();
            }
            else {
                _this.model = _this.talkService.talks.find(function (t) { return t.Id == +id; });
            }
        });
    };
    TalkEditor.prototype.ngOnDestroy = function () {
        this.sub.unsubscribe();
    };
    TalkEditor = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "talkEditor.html",
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [router_2.ActivatedRoute, talkService_1.TalkService])
    ], TalkEditor);
    return TalkEditor;
}());
exports.TalkEditor = TalkEditor;
//# sourceMappingURL=talkEditor.js.map