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
var http_1 = require('@angular/http');
var TalkService = (function () {
    function TalkService(http) {
        this._http = http;
    }
    Object.defineProperty(TalkService.prototype, "talks", {
        get: function () { return this._talks; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TalkService.prototype, "baseUrl", {
        get: function () {
            return '/' + this.moniker + "/api/cfs/speaker";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TalkService.prototype, "moniker", {
        get: function () {
            return window.location.pathname.split('/')[1];
        },
        enumerable: true,
        configurable: true
    });
    TalkService.prototype.getTasks = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            _this._http.get(_this.baseUrl)
                .subscribe(function (res) {
                _this._talks = res.json().talks;
                resolve(true);
            }, function (err) { return reject(err); });
        });
    };
    TalkService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], TalkService);
    return TalkService;
}());
exports.TalkService = TalkService;
//# sourceMappingURL=talkService.js.map