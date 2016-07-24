// scheduleForm.ts
import { Component } from '@angular/core';
import { DataService } from "../common/dataService";

@Component({
  selector: "schedule-form",
  templateUrl: "/js/app/schedule/scheduleForm.html"
})
export class ScheduleForm {

  talks: Array<any> = [];

  isBusy: boolean = false;
  error: string = null;

  constructor(private data: DataService) {
    this.loadSchedule();
  }

  showError(err) {
    this.error = err;
    this.isBusy = false;
  }

  loadSchedule() {
    this.isBusy = true;
    this.data.getAllTalks()
      .subscribe(
      res => this.talks = res.json(),
      res => this.showError("Failed to get talks"),
      () => this.isBusy = false);
  }

  onDelete(talk: any) {
    this.isBusy = true;
    this.data.deleteTalk(talk.id)
      .subscribe(res => {
        this.talks.splice(this.talks.indexOf(talk), 1);
      }, e => this.showError("Failed to delete talk"), () => this.isBusy = false);
  }

  onToggleApproved(talk: any) {
    this.isBusy = true;
    this.data.toggleApproved(talk)
      .subscribe(res => {
        talk.approved = !talk.approved;
      }, e => this.showError("Failed to toggle approved flag"), () => this.isBusy = false);
  }

}