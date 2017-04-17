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
// fileUploadService.ts
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var dataService_1 = require("./dataService");
var ImageUploadService = (function () {
    function ImageUploadService(http, data) {
        this.http = http;
        this.data = data;
    }
    ImageUploadService.prototype.uploadSpeaker = function (img) {
        return this.uploadImage(img, "speakers");
    };
    ImageUploadService.prototype.uploadSponsor = function (img, moniker) {
        if (moniker === void 0) { moniker = this.data.moniker; }
        return this.uploadImage(img, "sponsors", moniker);
    };
    ImageUploadService.prototype.uploadImage = function (file, imageType, moniker) {
        if (moniker === void 0) { moniker = this.data.moniker; }
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4) {
                    if (xhr.status >= 200 && xhr.status <= 299) {
                        var location_1 = xhr.getResponseHeader("location");
                        if (!location_1)
                            location_1 = xhr.responseBody.location;
                        resolve(location_1);
                    }
                    else {
                        reject(xhr.response);
                    }
                }
            };
            xhr.open('POST', "/" + moniker + "/api/images/" + imageType, true);
            var formData = new FormData();
            formData.append("file", file, file.name);
            xhr.send(formData);
        });
    };
    return ImageUploadService;
}());
ImageUploadService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http, dataService_1.DataService])
], ImageUploadService);
exports.ImageUploadService = ImageUploadService;
//# sourceMappingURL=imageUploadService.js.map