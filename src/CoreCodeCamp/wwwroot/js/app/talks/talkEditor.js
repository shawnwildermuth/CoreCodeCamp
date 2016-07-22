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
var talk_1 = require("./talk");
var talkService_1 = require("./talkService");
var TalkEditor = (function () {
    function TalkEditor(route, talkService, router) {
        this.route = route;
        this.talkService = talkService;
        this.router = router;
        this.isBusy = false;
        this.error = "";
    }
    TalkEditor.prototype.ngOnInit = function () {
        var _this = this;
        this.sub = this.route.params.subscribe(function (params) {
            var id = params['id'];
            if (id == "new") {
                _this.model = new talk_1.Talk();
            }
            else {
                _this.model = _this.talkService.talks.find(function (t) { return t.id == id; });
            }
        });
    };
    TalkEditor.prototype.ngOnDestroy = function () {
        this.sub.unsubscribe();
    };
    TalkEditor.prototype.onSave = function () {
        var _this = this;
        this.isBusy = true;
        this.talkService.saveTalk(this.model)
            .then(function () {
            _this.router.navigate(["/speaker"]);
        }, function (err) { return _this.error = err; })
            .then(function () { return _this.isBusy = false; });
    };
    TalkEditor = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "/js/app/talks/talkEditor.html",
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [router_2.ActivatedRoute, talkService_1.TalkService, router_2.Router])
    ], TalkEditor);
    return TalkEditor;
}());
exports.TalkEditor = TalkEditor;
//# sourceMappingURL=talkEditor.js.map