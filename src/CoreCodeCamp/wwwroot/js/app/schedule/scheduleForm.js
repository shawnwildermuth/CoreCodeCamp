"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var baseForm_1 = require("../common/baseForm");
var Rx_1 = require('rxjs/Rx');
var ScheduleForm = (function (_super) {
    __extends(ScheduleForm, _super);
    function ScheduleForm(data) {
        _super.call(this);
        this.data = data;
        this.talks = [];
        this.timeSlots = [];
        this.rooms = [];
        this.tracks = [];
        this.msg = "";
        this.sort = "";
        this.sortAsc = true;
        this.loadSchedule();
    }
    ScheduleForm.prototype.loadSchedule = function () {
        var _this = this;
        this.isBusy = true;
        Rx_1.Observable.forkJoin(this.data.getAllTalks(), this.data.getRooms(), this.data.getTimeSlots(), this.data.getTracks()).subscribe(function (res) {
            _this.talks = res[0].json();
            _this.rooms = res[1].json();
            _this.timeSlots = res[2].json();
            _this.tracks = res[3].json();
        }, function (res) { return _this.showError("Failed to get data"); }, function () { return _this.isBusy = false; });
    };
    ScheduleForm.prototype.setMsg = function (text) {
        var _this = this;
        this.msg = text;
        window.setTimeout(function () { return _this.msg = ""; }, 5000);
    };
    ScheduleForm.prototype.onTrackChanged = function (talk, value) {
        var _this = this;
        this.isBusy = true;
        this.data.updateTalkTrack(talk, value)
            .subscribe(function (res) {
            _this.setMsg("Saved...");
            _this.isBusy = false;
        }, function (e) {
            _this.showError("Failed to update talk");
            _this.isBusy = false;
        });
    };
    ScheduleForm.prototype.onTimeChanged = function (talk, value) {
        var _this = this;
        this.isBusy = true;
        this.data.updateTalkTime(talk, value)
            .subscribe(function (res) {
            _this.setMsg("Saved...");
            _this.isBusy = false;
        }, function (e) {
            _this.showError("Failed to update talk");
            _this.isBusy = false;
        });
    };
    ScheduleForm.prototype.onRoomChanged = function (talk, value) {
        var _this = this;
        this.isBusy = true;
        this.data.updateTalkRoom(talk, value)
            .subscribe(function (res) {
            _this.setMsg("Saved...");
            _this.isBusy = false;
        }, function (e) {
            _this.showError("Failed to update talk");
            _this.isBusy = false;
        });
    };
    ScheduleForm.prototype.onSort = function (sort) {
        var _this = this;
        if (sort == this.sort) {
            if (this.sortAsc)
                this.sortAsc = false;
            else {
                this.sort = "title"; // Reset to default sort
                this.sortAsc = true;
            }
        }
        else {
            this.sort = sort;
            this.sortAsc = true;
        }
        // do the sort
        this.talks = this.talks.sort(function (a, b) {
            if (a[_this.sort] == b[_this.sort])
                return 0;
            if (a[_this.sort] < b[_this.sort])
                return _this.sortAsc ? -1 : 1;
            else
                return _this.sortAsc ? 1 : -1;
        });
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
}(baseForm_1.BaseForm));
exports.ScheduleForm = ScheduleForm;
//# sourceMappingURL=scheduleForm.js.map