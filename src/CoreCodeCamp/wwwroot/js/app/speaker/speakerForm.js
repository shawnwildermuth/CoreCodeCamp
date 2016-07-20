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
// speakerForm.ts
var core_1 = require('@angular/core');
var imageUploadService_1 = require("../common/imageUploadService");
var dataService_1 = require("../common/dataService");
var SpeakerForm = (function () {
    function SpeakerForm(data, upload) {
        this.data = data;
        this.upload = upload;
        this.model = {};
        this.isBusy = false;
        this.error = null;
        this.imageError = null;
        this.onLoad();
    }
    SpeakerForm.prototype.onLoad = function () {
        var _this = this;
        this.isBusy = true;
        this.data.getMySpeaker()
            .subscribe(function (res) {
            _this.model = res.json();
        }, function (e) {
            _this.error = e.response;
        }, function () { return _this.isBusy = false; });
    };
    SpeakerForm.prototype.onSave = function () {
        var _this = this;
        this.isBusy = true;
        this.data.saveSpeaker(this.model)
            .subscribe(function (res) {
            window.location.href = "./manage";
        }, function (e) {
            _this.error = e;
            _this.isBusy = false;
        });
    };
    SpeakerForm.prototype.onImagePicked = function (filePicker) {
        var _this = this;
        this.isBusy = true;
        this.upload.uploadSpeaker(filePicker.files[0])
            .then(function (imageUrl) {
            _this.model.imageUrl = imageUrl;
        }, function (e) {
            _this.imageError = e;
        })
            .then(function () { return _this.isBusy = false; });
    };
    SpeakerForm.prototype.validImage = function () {
        if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0)
            return true;
        return false;
    };
    SpeakerForm = __decorate([
        core_1.Component({
            selector: "speaker-form",
            moduleId: module.id,
            templateUrl: "speakerForm.html"
        }), 
        __metadata('design:paramtypes', [dataService_1.DataService, imageUploadService_1.ImageUploadService])
    ], SpeakerForm);
    return SpeakerForm;
}());
exports.SpeakerForm = SpeakerForm;
//# sourceMappingURL=speakerForm.js.map