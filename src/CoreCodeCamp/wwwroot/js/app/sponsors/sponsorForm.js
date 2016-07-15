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
// usersForm.ts
var core_1 = require('@angular/core');
var sponsorService_1 = require("./sponsorService");
var fileUploadService_1 = require("../common/fileUploadService");
var SponsorForm = (function () {
    function SponsorForm(sponsorService, upload) {
        this.sponsorService = sponsorService;
        this.upload = upload;
        this.model = {};
        this.sponsors = [];
        this.events = [];
        this.currentMoniker = "";
        this.isBusy = false;
        this.isEditing = false;
        this.error = null;
        this.imageError = null;
        this.loadEvents();
    }
    SponsorForm.prototype.loadEvents = function () {
        var _this = this;
        this.isBusy = true;
        this.sponsorService
            .getEvents()
            .subscribe(function (res) { return _this.events = res.json(); }, function (res) { return _this.error = "Failed to get events"; }, function () { return _this.isBusy = false; });
    };
    SponsorForm.prototype.loadSponsors = function () {
        var _this = this;
        if (this.currentMoniker) {
            this.isBusy = true;
            this.sponsorService
                .getSponsors(this.currentMoniker)
                .subscribe(function (res) { return _this.sponsors = res.json(); }, function (res) { return _this.error = "Failed to get sponsors"; }, function () { return _this.isBusy = false; });
        }
    };
    SponsorForm.prototype.onMonikerChange = function ($event) {
        this.loadSponsors();
    };
    SponsorForm.prototype.onEdit = function (sponsor) {
        this.model = sponsor;
        this.isEditing = true;
    };
    SponsorForm.prototype.onNew = function () {
        this.isEditing = true;
        this.model = {};
    };
    SponsorForm.prototype.onImagePicked = function (filePicker) {
        var _this = this;
        this.isBusy = true;
        this.upload.uploadFile(filePicker.files[0], "api/sponsor/image")
            .then(function (imageUrl) {
            _this.model.imageUrl = imageUrl;
        }, function (e) {
            _this.imageError = e.json();
        })
            .then(function () { return _this.isBusy = false; });
    };
    SponsorForm.prototype.validImage = function () {
        if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0)
            return true;
        return false;
    };
    SponsorForm = __decorate([
        core_1.Component({
            selector: "sponsors-form",
            moduleId: module.id,
            templateUrl: "sponsorForm.html"
        }), 
        __metadata('design:paramtypes', [sponsorService_1.SponsorService, fileUploadService_1.FileUploadService])
    ], SponsorForm);
    return SponsorForm;
}());
exports.SponsorForm = SponsorForm;
//# sourceMappingURL=sponsorForm.js.map