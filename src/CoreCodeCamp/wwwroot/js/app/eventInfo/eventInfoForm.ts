// scheduleForm.ts
import { Component } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Response } from '@angular/http';
import { DataService } from "../common/dataService";
import { BaseForm } from "../common/baseForm";
import { Observable } from 'rxjs/Rx';
import * as moment from 'moment/moment';
import { UtcDatePipe } from "../common/utcDatePipe";


declare var jQuery: any;

@Component({
  selector: "event-info-form",
  templateUrl: "/js/app/eventInfo/eventInfoForm.html",
  pipes: [ UtcDatePipe ]
})
export class EventInfoForm extends BaseForm {

  model: any = {};
  location: any = {};
  timeSlots: Array<any> = [];
  tracks: Array<any> = [];
  rooms: Array<any> = [];
  newRoom: string = "";
  newTimeSlot: string = "";
  newTrack: string = "";
  datePipe: DatePipe = new DatePipe();
  msg: string = "";

  constructor(private data: DataService) {
    super();
    this.loadEventInfo();
  }

  loadEventInfo() {
    this.isBusy = true;

    Observable.forkJoin(
      this.data.getEventInfo(),
      this.data.getRooms(),
      this.data.getTimeSlots(),
      this.data.getTracks()
    ).subscribe(
      res => {
        this.model = this.mapEvent(res[0].json());
        this.location = this.model.location;
        this.rooms = res[1].json();
        this.timeSlots = res[2].json();
        this.tracks = res[3].json();
      },
      res => this.showError("Failed to get data"),
      () => this.isBusy = false);
  }

  mapEvent(src) {
    src.eventDate = this.datePipe.transform(src.eventDate, 'MM/dd/yyyy');
    src.callForSpeakersOpened = this.datePipe.transform(src.callForSpeakersOpened, 'MM/dd/yyyy');
    src.callForSpeakersClosed = this.datePipe.transform(src.callForSpeakersClosed, 'MM/dd/yyyy');
    return src;
  }

  saveEvent() {
    this.msg = "";
    this.isBusy = true;
    this.data.saveEventInfo(this.model)
      .subscribe((res) => {
        this.isBusy = false;
        this.msg = "Saved...";
        window.setTimeout(() => this.msg = "", 5000);
      }, (e) => {
        this.error = e;
        this.isBusy = false;
      });
  }


  saveTrack() {
    this.isBusy = true;
    this.data.saveTrack(this.newTrack)
      .subscribe((res) => {
        this.tracks.push(res.json());
        this.newTrack = "";
        this.isBusy = false;
      }, (e) => {
        this.error = e;
        this.isBusy = false;
      });
  }

  saveRoom() {
    this.isBusy = true;
    this.data.saveRoom(this.newRoom)
      .subscribe((res) => {
        this.rooms.push(res.json());
        this.isBusy = false;
        this.newRoom = "";
      }, (e) => {
        this.error = e;
        this.isBusy = false;
      });
  }

  saveTimeSlot() {
    this.isBusy = true;
    this.data.saveTimeSlot(this.newTimeSlot)
      .subscribe((res) => {
        this.timeSlots.push(res.json());
        this.newTimeSlot = "";
        this.isBusy = false;
      }, (e) => {
        this.error = e;
        this.isBusy = false;
      });
  }

  deleteTrack(track: any) {
    this.isBusy = true;
    this.data.deleteTrack(track)
      .subscribe(res => {
        this.tracks.splice(this.tracks.indexOf(track), 1);
      }, e => this.showError("Failed to delete track"), () => this.isBusy = false);
  }

  deleteRoom(room: any) {
    this.isBusy = true;
    this.data.deleteRoom(room)
      .subscribe(res => {
        this.rooms.splice(this.rooms.indexOf(room), 1);
      }, e => this.showError("Failed to delete room"), () => this.isBusy = false);
  }

  deleteTimeSlot(timeSlot: any) {
    this.isBusy = true;
    this.data.deleteTimeSlot(timeSlot)
      .subscribe(res => {
        this.timeSlots.splice(this.timeSlots.indexOf(timeSlot), 1);
      }, e => this.showError("Failed to delete Time Slot"), () => this.isBusy = false);
  }

  ngAfterViewChecked(): void {
    jQuery(".datepicker").datepicker();
  }

}

