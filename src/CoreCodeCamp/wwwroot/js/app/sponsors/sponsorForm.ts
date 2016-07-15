// usersForm.ts
import { Component } from '@angular/core';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import { SponsorService } from "./sponsorService";
import { FileUploadService } from "../common/fileUploadService";

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

  constructor(private sponsorService: SponsorService, private upload: FileUploadService) {
    this.loadEvents();
  }

  loadEvents() {
    this.isBusy = true;
    this.sponsorService
      .getEvents()
      .subscribe(
      res => this.events = res.json(),
      res => this.error = "Failed to get events",
      () => this.isBusy = false);
  }

  loadSponsors() {
    if (this.currentMoniker) {
      this.isBusy = true;
      this.sponsorService
        .getSponsors(this.currentMoniker)
        .subscribe(
        res => this.sponsors = res.json(),
        res => this.error = "Failed to get sponsors",
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

  onNew() {
    this.isEditing = true;
    this.model = {};
  }

  onImagePicked(filePicker: any) {
    this.isBusy = true;
    this.upload.uploadFile(filePicker.files[0], "api/sponsor/image")
      .then(imageUrl => {
        this.model.imageUrl = imageUrl;
      }, (e) => {
        this.imageError = e.json();
      })
      .then(() => this.isBusy = false);
  }

  validImage() {
    if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0) return true;
    return false;
  }



}