// usersForm.ts
import { Component } from '@angular/core';
import { DataService } from "../common/dataService";
import { ImageUploadService } from "../common/imageUploadService";

@Component({
  selector: "sponsors-form",
  moduleId: module.id, // To make urls become relative!
  templateUrl: "/js/app/sponsors/sponsorForm.html"
})
export class SponsorForm {

  model: any = {};
  sponsors: Array<any> = [];
  currentMoniker: string = "";

  isBusy: boolean = false;
  isEditing: boolean = false;
  error: string = null;
  imageError: string = null;

  constructor(private data: DataService, private upload: ImageUploadService) {
    this.loadSponsors();
  }

  showError(err) {
    this.error = err;
    this.isBusy = false;
  }

  loadSponsors() {
    this.isBusy = true;
    this.data.getSponsors()
      .subscribe(
      res => this.sponsors = res.json(),
      res => this.showError("Failed to get sponsors"),
      () => this.isBusy = false);
  }

  onEdit(sponsor: any) {
    this.model = sponsor;
    this.isEditing = true;
  }

  onDelete(sponsor: any) {
    this.isBusy = true;
    this.data.deleteSponsor(sponsor)
      .subscribe(res => {
        this.sponsors.splice(this.sponsors.indexOf(sponsor), 1);
      }, e => this.showError("Failed to delete sponsor"), () => this.isBusy = false);
  }

  onTogglePaid(sponsor: any) {
    this.isBusy = true;
    this.data.togglePaid(sponsor)
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

    this.data.saveSponsor(this.model)
      .subscribe(res => {
        this.sponsors.push(res.json());
        this.isEditing = false;
      }, (e) => this.showError("Failed to save sponsor"), () => this.isBusy = false);
  }

  onImagePicked(filePicker: any) {
    this.isBusy = true;
    this.upload.uploadSponsor(filePicker.files[0])
      .then((imageUrl: any) => {
        this.model.imageUrl = imageUrl;
      }, (e) => this.showError("Failed to upload Image"))
      .then(() => this.isBusy = false);
  }

  validImage() {
    if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0) return true;
    return false;
  }



}