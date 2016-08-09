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
// talkService.ts
var core_1 = require('@angular/core');
var dataService_1 = require("../common/dataService");
var TalkService = (function () {
    function TalkService(data) {
        this.data = data;
        this.talks = [];
        this.loadInitialData();
    }
    TalkService.prototype.loadInitialData = function () {
        var _this = this;
        this.data.getTalks()
            .subscribe(function (res) {
            var resTalks = res.json();
            resTalks.forEach(function (t) { return _this.talks.push(t); });
        }, function (err) { return console.log(err); });
    };
    TalkService.prototype.saveTalk = function (talk) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var index = _this.talks.indexOf(talk);
            if (index > -1)
                _this.talks.splice(index, 1);
            var obj = _this.data.saveTalk(talk)
                .subscribe(function (res) {
                var updatedTalk = res.json();
                _this.talks.push(updatedTalk);
                resolve(updatedTalk);
            }, function (error) { return reject(error); });
        });
    };
    TalkService.prototype.delete = function (talk) {
        var _this = this;
        var obj = this.data.deleteTalk(talk.id)
            .subscribe(function (res) {
            _this.talks.splice(_this.talks.indexOf(talk), 1);
        });
    };
    TalkService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [dataService_1.DataService])
    ], TalkService);
    return TalkService;
}());
exports.TalkService = TalkService;
//# sourceMappingURL=talkService.js.map