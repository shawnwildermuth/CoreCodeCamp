// usersForm.ts
import { Component } from '@angular/core';
import { DataService } from "../common/dataService";
import { ImageUploadService } from "../common/imageUploadService";

@Component({
  selector: "sponsors-form",
  moduleId: module.id, // To make urls become relative!
  templateUrl: "sponsorForm.html"
})
export class SponsorForm {

  model: any = {};
  sponsors: Array<any> = [];
  events: Array<any> = [];
  currentMoniker: string = "";

  isBusy: boolean = false;
  isEditing: boolean = false;
  error: string = null;
  imageError: string = null;

  constructor(private data: DataService, private upload: ImageUploadService) {
    this.loadEvents();
  }

  loadEvents() {
    this.isBusy = true;
    this.data
      .getEvents()
      .subscribe(
      res => this.events = res.json(),
      res => this.showError("Failed to get events"),
      () => this.isBusy = false);
  }

  showError(err) {
    this.error = "Failed to get events";
    this.isBusy = false;
  }

  loadSponsors() {
    if (this.currentMoniker) {
      this.isBusy = true;
      this.data.getSponsors(this.currentMoniker)
        .subscribe(
        res => this.sponsors = res.json(),
        res => this.showError("Failed to get sponsors"),
        () => this.isBusy = false);
    }
  }

  onMonikerChange($event: any) {
    this.loadSponsors();
  }

  onEdit(sponsor: any) {
    this.model = sponsor;
    this.isEditing = true;
  }

  onDelete(sponsor: any) {
    this.isBusy = true;
    this.data.deleteSponsor(this.currentMoniker, sponsor)
      .subscribe(res => {
        this.sponsors.splice(this.sponsors.indexOf(sponsor), 1);
      }, e => this.showError("Failed to delete sponsor"), () => this.isBusy = false);
  }

  onTogglePaid(sponsor: any) {
    this.isBusy = true;
    this.data.togglePaid(this.currentMoniker, sponsor) 
      .subscribe(res => {
        sponsor.paid = !sponsor.paid;
      }, e => this.showError("Failed to toggle paid flag"), () => this.isBusy = false);
  }

  onNew() {
    this.isEditing = true;
    this.model = {};
  }

  onCancel() {
    this.isEditing = false;
    this.model = {};
  }

  onSave() {
    // Remove old one
    var old = this.sponsors.indexOf(this.model);
    if (old > -1) this.sponsors.splice(this.sponsors.indexOf(this.model), 1); 
    this.isBusy = true;

    this.data.saveSponsor(this.currentMoniker, this.model)
      .subscribe(res => {
        this.sponsors.push(res.json());
        this.isEditing = false;
      }, (e) => this.showError("Failed to save sponsor"), () => this.isBusy = false);
  }

  onImagePicked(filePicker: any) {
    this.isBusy = true;
    this.upload.uploadSponsor(filePicker.files[0], this.currentMoniker)
      .then((imageUrl:any) => {
        this.model.imageUrl = imageUrl;
      }, (e) => this.showError("Failed to upload Image"))
      .then(() => this.isBusy = false);
  }

  validImage() {
    if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0) return true;
    return false;
  }



}