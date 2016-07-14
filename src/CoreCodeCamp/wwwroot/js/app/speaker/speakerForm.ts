// speakerForm.ts
import { Component } from '@angular/core';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import {Http, Headers} from '@angular/http';
import { SpeakerViewModel } from './speakerViewModel';

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

  constructor(private http: Http) {
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
    this.uploadFile(filePicker.files[0])
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

  uploadFile(file: File): Promise<string> {
    return new Promise((resolve, reject) => {

      let xhr: XMLHttpRequest = new XMLHttpRequest();
      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4) {
          if (xhr.status === 200) {
            resolve(xhr.response);
          } else {
            reject(xhr.response);
          }
        }
      };

      xhr.open('POST', this.baseUrl + "/headshot", true);

      let formData = new FormData();
      formData.append("file", file, file.name);
      xhr.send(formData);
    });
  }
}

