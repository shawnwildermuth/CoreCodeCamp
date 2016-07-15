// speakerForm.ts
import { Component } from '@angular/core';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import { Http, Headers } from '@angular/http';
import { SpeakerViewModel } from './speakerViewModel';
import { FileUploadService } from "../common/fileUploadService";

@Component({
  selector: "speaker-form",
  moduleId: module.id, // To make urls become relative!
  templateUrl: "speakerForm.html"
})
export class SpeakerForm {

  model:any = {};
  isBusy: boolean = false;
  error: string = null;
  imageError: string = null;

  constructor(private http: Http, private upload: FileUploadService) {
    this.onLoad();
  }

  private onLoad() {
    this.isBusy = true;
    this.http.get(this.baseUrl)
      .subscribe((res) => {
        this.model = res.json();
      }, (e) => {
        this.error = e.json();
      }, () => this.isBusy = false);
  }

  private get baseUrl() {
    return '/' + this.moniker + "/api/cfs/speaker";
  }

  private get moniker() {
    return window.location.pathname.split('/')[1];
  }

  onSave() {
    this.isBusy = true;
    var url = this.baseUrl;
    this.http.post(url, this.model)
      .subscribe((res) => {
        window.location.href = "./manage";
      }, (e) => {
        this.error = e.response.json();
      }, () => this.isBusy = false);
  }

  onImagePicked(filePicker: any) {
    this.isBusy = true;
    this.upload.uploadFile(filePicker.files[0], this.baseUrl + "/headshot")
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

