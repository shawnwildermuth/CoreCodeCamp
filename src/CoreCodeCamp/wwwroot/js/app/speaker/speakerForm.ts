// speakerForm.ts
import { Component } from '@angular/core';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import { Http, Headers } from '@angular/http';
import { SpeakerViewModel } from './speakerViewModel';
import { ImageUploadService } from "../common/imageUploadService";
import { DataService } from "../common/dataService";

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

  constructor(private data: DataService, private upload: ImageUploadService) {
    this.onLoad();
  }

  private onLoad() {
    this.isBusy = true;
    this.data.getMySpeaker()
      .subscribe((res) => {
        this.model = res.json();
      }, (e) => {
        this.error = e.response;
      }, () => this.isBusy = false);
  }

  onSave() {
    this.isBusy = true;
    this.data.saveSpeaker(this.model)
      .subscribe((res) => {
        window.location.href = "./manage";
      }, (e) => {
        this.error = e;
        this.isBusy = false;
      });
  }

  onImagePicked(filePicker: any) {
    this.isBusy = true;
    this.upload.uploadSpeaker(filePicker.files[0])
      .then((imageUrl:any) => {
        this.model.imageUrl = imageUrl;
      }, (e) => {
        this.imageError = e;
      })
      .then(() => this.isBusy = false);
  }

  validImage() {
    if (this.model && this.model.imageUrl && this.model.imageUrl.length > 0) return true;
    return false;
  }

}

