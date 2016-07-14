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
var http_1 = require('@angular/http');
var SpeakerForm = (function () {
    function SpeakerForm(http) {
        this.http = http;
        this.model = {};
        this.isBusy = false;
        this.error = null;
        this.imageError = null;
        this.onLoad();
    }
    SpeakerForm.prototype.onLoad = function () {
        var _this = this;
        this.isBusy = true;
        this.http.get(this.baseUrl)
            .subscribe(function (res) {
            _this.model = res.json();
        }, function (e) {
            _this.error = e.json();
        }, function () { return _this.isBusy = false; });
    };
    Object.defineProperty(SpeakerForm.prototype, "baseUrl", {
        get: function () {
            return '/' + this.moniker + "/api/cfs/speaker";
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SpeakerForm.prototype, "moniker", {
        get: function () {
            return window.location.pathname.split('/')[1];
        },
        enumerable: true,
        configurable: true
    });
    SpeakerForm.prototype.onSave = function () {
        var _this = this;
        this.isBusy = true;
        var url = this.baseUrl;
        this.http.post(url, this.model)
            .subscribe(function (res) {
            window.location.href = "./manage";
        }, function (e) {
            _this.error = e.response.json();
        }, function () { return _this.isBusy = false; });
    };
    SpeakerForm.prototype.onImagePicked = function (filePicker) {
        var _this = this;
        this.isBusy = true;
        this.uploadFile(filePicker.files[0])
            .then(function (imageUrl) {
            _this.model.imageUrl = imageUrl;
        }, function (e) {
            _this.imageError = e.json();
        })
            .then(function () { return _this.isBusy = false; });
    };
    Object.defineProperty(SpeakerForm.prototype, "validImage", {
        get: function () {
            return this.model && this.model.imageUrl && this.model.imageUrl.length > 0;
        },
        enumerable: true,
        configurable: true
    });
    SpeakerForm.prototype.uploadFile = function (file) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        resolve(xhr.response);
                    }
                    else {
                        reject(xhr.response);
                    }
                }
            };
            xhr.open('POST', _this.baseUrl + "/headshot", true);
            var formData = new FormData();
            formData.append("file", file, file.name);
            xhr.send(formData);
        });
    };
    SpeakerForm = __decorate([
        core_1.Component({
            selector: "speaker-form",
            moduleId: module.id,
            templateUrl: "speakerForm.html"
        }), 
        __metadata('design:paramtypes', [http_1.Http])
    ], SpeakerForm);
    return SpeakerForm;
}());
exports.SpeakerForm = SpeakerForm;
//# sourceMappingURL=speakerForm.js.map