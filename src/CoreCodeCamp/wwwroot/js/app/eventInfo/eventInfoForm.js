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
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var dataService_1 = require("../common/dataService");
var baseForm_1 = require("../common/baseForm");
var Rx_1 = require("rxjs/Rx");
var utcDatePipe_1 = require("../common/utcDatePipe");
var EventInfoForm = (function (_super) {
    __extends(EventInfoForm, _super);
    function EventInfoForm(data) {
        var _this = _super.call(this) || this;
        _this.data = data;
        _this.model = {};
        _this.location = {};
        _this.timeSlots = [];
        _this.tracks = [];
        _this.rooms = [];
        _this.newRoom = "";
        _this.newTimeSlot = "";
        _this.newTrack = "";
        _this.datePipe = new common_1.DatePipe();
        _this.msg = "";
        _this.loadEventInfo();
        return _this;
    }
    EventInfoForm.prototype.loadEventInfo = function () {
        var _this = this;
        this.isBusy = true;
        Rx_1.Observable.forkJoin(this.data.getEventInfo(), this.data.getRooms(), this.data.getTimeSlots(), this.data.getTracks()).subscribe(function (res) {
            _this.model = _this.mapEvent(res[0].json());
            _this.location = _this.model.location;
            _this.rooms = res[1].json();
            _this.timeSlots = res[2].json();
            _this.tracks = res[3].json();
        }, function (res) { return _this.showError("Failed to get data"); }, function () { return _this.isBusy = false; });
    };
    EventInfoForm.prototype.mapEvent = function (src) {
        src.eventDate = this.datePipe.transform(src.eventDate, 'MM/dd/yyyy');
        src.callForSpeakersOpened = this.datePipe.transform(src.callForSpeakersOpened, 'MM/dd/yyyy');
        src.callForSpeakersClosed = this.datePipe.transform(src.callForSpeakersClosed, 'MM/dd/yyyy');
        return src;
    };
    EventInfoForm.prototype.saveEvent = function () {
        var _this = this;
        this.msg = "";
        this.isBusy = true;
        this.data.saveEventInfo(this.model)
            .subscribe(function (res) {
            _this.isBusy = false;
            _this.msg = "Saved...";
            window.setTimeout(function () { return _this.msg = ""; }, 5000);
        }, function (e) {
            _this.error = e;
            _this.isBusy = false;
        });
    };
    EventInfoForm.prototype.saveTrack = function () {
        var _this = this;
        this.isBusy = true;
        this.data.saveTrack(this.newTrack)
            .subscribe(function (res) {
            _this.tracks.push(res.json());
            _this.newTrack = "";
            _this.isBusy = false;
        }, function (e) {
            _this.error = e;
            _this.isBusy = false;
        });
    };
    EventInfoForm.prototype.saveRoom = function () {
        var _this = this;
        this.isBusy = true;
        this.data.saveRoom(this.newRoom)
            .subscribe(function (res) {
            _this.rooms.push(res.json());
            _this.isBusy = false;
            _this.newRoom = "";
        }, function (e) {
            _this.error = e;
            _this.isBusy = false;
        });
    };
    EventInfoForm.prototype.saveTimeSlot = function () {
        var _this = this;
        this.isBusy = true;
        this.data.saveTimeSlot(this.newTimeSlot)
            .subscribe(function (res) {
            _this.timeSlots.push(res.json());
            _this.newTimeSlot = "";
            _this.isBusy = false;
        }, function (e) {
            _this.error = e;
            _this.isBusy = false;
        });
    };
    EventInfoForm.prototype.deleteTrack = function (track) {
        var _this = this;
        this.isBusy = true;
        this.data.deleteTrack(track)
            .subscribe(function (res) {
            _this.tracks.splice(_this.tracks.indexOf(track), 1);
        }, function (e) { return _this.showError("Failed to delete track"); }, function () { return _this.isBusy = false; });
    };
    EventInfoForm.prototype.deleteRoom = function (room) {
        var _this = this;
        this.isBusy = true;
        this.data.deleteRoom(room)
            .subscribe(function (res) {
            _this.rooms.splice(_this.rooms.indexOf(room), 1);
        }, function (e) { return _this.showError("Failed to delete room"); }, function () { return _this.isBusy = false; });
    };
    EventInfoForm.prototype.deleteTimeSlot = function (timeSlot) {
        var _this = this;
        this.isBusy = true;
        this.data.deleteTimeSlot(timeSlot)
            .subscribe(function (res) {
            _this.timeSlots.splice(_this.timeSlots.indexOf(timeSlot), 1);
        }, function (e) { return _this.showError("Failed to delete Time Slot"); }, function () { return _this.isBusy = false; });
    };
    EventInfoForm.prototype.ngAfterViewChecked = function () {
        jQuery(".datepicker").datepicker();
    };
    return EventInfoForm;
}(baseForm_1.BaseForm));
EventInfoForm = __decorate([
    core_1.Component({
        selector: "event-info-form",
        templateUrl: "/js/app/eventInfo/eventInfoForm.html",
        pipes: [utcDatePipe_1.UtcDatePipe]
    }),
    __metadata("design:paramtypes", [dataService_1.DataService])
], EventInfoForm);
exports.EventInfoForm = EventInfoForm;
//# sourceMappingURL=eventInfoForm.js.map