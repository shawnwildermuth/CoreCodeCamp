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
// SponsorService.ts
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
var SponsorService = (function () {
    function SponsorService(http) {
        this.http = http;
    }
    SponsorService.prototype.getEvents = function () {
        return this.http.get("/api/events");
    };
    SponsorService.prototype.getSponsors = function (moniker) {
        return this.http.get("/api/sponsors/" + moniker);
    };
    SponsorService.prototype.saveSponsor = function (moniker, sponsor) {
        return this.http.post("/api/sponsors/" + moniker, sponsor);
    };
    SponsorService.prototype.deleteSponsor = function (moniker, sponsor) {
        return this.http.delete("/api/sponsors/" + moniker + "/" + sponsor.id);
    };
    SponsorService.prototype.togglePaid = function (moniker, sponsor) {
        return this.http.put("/api/sponsors/" + moniker + "/togglePaid/" + sponsor.id, null);
    };
    SponsorService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], SponsorService);
    return SponsorService;
}());
exports.SponsorService = SponsorService;
//# sourceMappingURL=sponsorService.js.map