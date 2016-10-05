// scheduleForm.ts
import { Component } from '@angular/core';
import { DataService } from "../common/dataService";
import { BaseForm } from "../common/baseForm";
import { Observable } from 'rxjs/Rx';
import { UtcDatePipe } from "../common/utcDatePipe";

@Component({
  selector: "schedule-form",
  templateUrl: "/js/app/schedule/scheduleForm.html",
  pipes: [ UtcDatePipe ]
})
export class ScheduleForm extends BaseForm {

  talks: Array<any> = [];
  timeSlots: Array<any> = [];
  rooms: Array<any> = [];
  tracks: Array<any> = [];
  msg: string = "";
  sort: string = "";
  sortAsc: boolean = true;
  summary = {
    speakers: 0,
    approved: 0,
    talks: 0
  };

  constructor(private data: DataService) {
    super();
    this.loadSchedule();
  }

  updateSummary() {
    this.summary.approved = this.talks.filter(t => t.approved).length; 
    this.summary.talks = this.talks.length; 
    this.summary.speakers = this.talks.map(t => t.speaker.name).filter((x, i, s) => s.indexOf(x) === i).length;
  }

  loadSchedule() {
    this.isBusy = true;

    Observable.forkJoin([
      this.data.getAllTalks(),
      this.data.getRooms(),
      this.data.getTimeSlots(),
      this.data.getTracks()]
    ).subscribe(
      res => {
        this.talks = res[0].json();
        this.rooms = res[1].json();
        this.timeSlots = res[2].json();
        this.tracks = res[3].json();
        this.updateSummary();
      },
      res => this.showError("Failed to get data"),
      () => this.isBusy = false);

  }

  setMsg(text: string) {
    this.msg = text;
    window.setTimeout(() => this.msg = "", 5000);
  }

  onTrackChanged(talk: any, value) {
    this.isBusy = true;
    this.data.updateTalkTrack(talk, value)
      .subscribe(res => {
        this.setMsg("Saved...");
        this.isBusy = false;
      }, e => {
        this.showError("Failed to update talk");
        this.isBusy = false;
      });
  }

  onTimeChanged(talk: any, value) {
    this.isBusy = true;
    this.data.updateTalkTime(talk, value)
      .subscribe(res => {
        this.setMsg("Saved...");
        this.isBusy = false;
      }, e => {
        this.showError("Failed to update talk");
        this.isBusy = false;
      });
  }

  onRoomChanged(talk: any, value) {
    this.isBusy = true;
    this.data.updateTalkRoom(talk, value)
      .subscribe(res => {
        this.setMsg("Saved...");
        this.isBusy = false;
      }, e => {
        this.showError("Failed to update talk");
        this.isBusy = false;
      });
  }

  onSort(sort: string) {
    if (sort == this.sort) {
      if (this.sortAsc) this.sortAsc = false;
      else {
        this.sort = "title"; // Reset to default sort
        this.sortAsc = true;
      }
    } else {
      this.sort = sort;
      this.sortAsc = true;
    }

    // do the sort
    this.talks = this.talks.sort((a, b) => {
      if (a[this.sort] == b[this.sort]) return 0;
      if (a[this.sort] < b[this.sort]) return this.sortAsc ? -1 : 1;
      else return this.sortAsc ? 1 : -1;
    });  
  }

  onDelete(talk: any) {
    this.isBusy = true;
    this.data.deleteTalk(talk.id)
      .subscribe(res => {
        this.talks.splice(this.talks.indexOf(talk), 1);
        this.updateSummary();
      }, e => this.showError("Failed to delete talk"), () => this.isBusy = false);
  }

  onToggleApproved(talk: any) {
    this.isBusy = true;
    this.data.toggleApproved(talk)
      .subscribe(res => {
        talk.approved = !talk.approved;
        this.updateSummary();
      }, e => this.showError("Failed to toggle approved flag"), () => this.isBusy = false);
  }

}