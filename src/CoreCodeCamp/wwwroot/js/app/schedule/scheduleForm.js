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
// scheduleForm.ts
var core_1 = require('@angular/core');
var dataService_1 = require("../common/dataService");
var ScheduleForm = (function () {
    function ScheduleForm(data) {
        this.data = data;
        this.talks = [];
        this.isBusy = false;
        this.error = null;
        this.loadSchedule();
    }
    ScheduleForm.prototype.showError = function (err) {
        this.error = err;
        this.isBusy = false;
    };
    ScheduleForm.prototype.loadSchedule = function () {
        var _this = this;
        this.isBusy = true;
        this.data.getAllTalks()
            .subscribe(function (res) { return _this.talks = res.json(); }, function (res) { return _this.showError("Failed to get talks"); }, function () { return _this.isBusy = false; });
    };
    ScheduleForm.prototype.onDelete = function (talk) {
        var _this = this;
        this.isBusy = true;
        this.data.deleteTalk(talk.id)
            .subscribe(function (res) {
            _this.talks.splice(_this.talks.indexOf(talk), 1);
        }, function (e) { return _this.showError("Failed to delete talk"); }, function () { return _this.isBusy = false; });
    };
    ScheduleForm.prototype.onToggleApproved = function (talk) {
        var _this = this;
        this.isBusy = true;
        this.data.toggleApproved(talk)
            .subscribe(function (res) {
            talk.approved = !talk.approved;
        }, function (e) { return _this.showError("Failed to toggle approved flag"); }, function () { return _this.isBusy = false; });
    };
    ScheduleForm = __decorate([
        core_1.Component({
            selector: "schedule-form",
            templateUrl: "/js/app/schedule/scheduleForm.html"
        }), 
        __metadata('design:paramtypes', [dataService_1.DataService])
    ], ScheduleForm);
    return ScheduleForm;
}());
exports.ScheduleForm = ScheduleForm;
//# sourceMappingURL=scheduleForm.js.map