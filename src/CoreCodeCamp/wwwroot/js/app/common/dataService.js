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
// dataService.ts
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var DataService = (function () {
    function DataService(http) {
        this.http = http;
    }
    // Utilities
    DataService.prototype.baseUrl = function (moniker) {
        if (moniker === void 0) { moniker = this.moniker; }
        return '/' + moniker + "/api/";
    };
    Object.defineProperty(DataService.prototype, "moniker", {
        get: function () {
            return window.location.pathname.split('/')[1];
        },
        enumerable: true,
        configurable: true
    });
    // Events
    DataService.prototype.getEvents = function () {
        return this.http.get("/api/events");
    };
    // Sponsors
    DataService.prototype.getSponsors = function (moniker) {
        return this.http.get(this.baseUrl(moniker) + "sponsors");
    };
    DataService.prototype.saveSponsor = function (moniker, sponsor) {
        return this.http.post(this.baseUrl(moniker) + "sponsors", sponsor);
    };
    DataService.prototype.deleteSponsor = function (moniker, sponsor) {
        return this.http.delete(this.baseUrl(moniker) + "sponsors/" + sponsor.id);
    };
    DataService.prototype.togglePaid = function (moniker, sponsor) {
        return this.http.put(this.baseUrl(moniker) + "sponsors/" + sponsor.id + "/togglePaid/", null);
    };
    // Speakers
    DataService.prototype.getMySpeaker = function () {
        return this.http.get(this.baseUrl() + "speakers/me");
    };
    DataService.prototype.saveSpeaker = function (speaker) {
        return this.http.post(this.baseUrl() + "speakers", speaker);
    };
    // Talks
    DataService.prototype.getTalks = function () {
        return this.http.get(this.baseUrl() + "talks/me");
    };
    DataService.prototype.saveTalk = function (talk) {
        return this.http.post(this.baseUrl() + "speakers/me/talks", talk);
    };
    DataService.prototype.deleteTalk = function (id) {
        return this.http.delete(this.baseUrl() + "talks/" + id);
    };
    // Users
    DataService.prototype.getUsers = function () {
        return this.http.get("/api/users");
    };
    DataService.prototype.toggleAdmin = function (user) {
        return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleAdmin", user);
    };
    DataService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], DataService);
    return DataService;
}());
exports.DataService = DataService;
//# sourceMappingURL=dataService.js.map