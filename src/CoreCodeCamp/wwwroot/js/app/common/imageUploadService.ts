// fileUploadService.ts
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { DataService } from "./dataService";

@Injectable()
export class ImageUploadService {

  constructor(private http: Http, private data: DataService) { }

  uploadSpeaker(img: File) {
    return this.uploadImage(img, "speakers");
  }

  uploadSponsor(img: File, moniker: string = this.data.moniker) {
    return this.uploadImage(img, "sponsors", moniker);
  }

  private uploadImage(file: File, imageType: string, moniker: string = this.data.moniker): Promise<string> {
    return new Promise((resolve, reject) => {

      let xhr: XMLHttpRequest = new XMLHttpRequest();
      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4) {
          if (xhr.status >= 200 && xhr.status <= 299) {
            let location = xhr.getResponseHeader("location");
            if (!location) location = xhr.responseBody.location;
            resolve(location);
          } else {
            reject(xhr.response);
          }
        }
      };

      xhr.open('POST', "/" + moniker + "/api/images/" + imageType, true);

      let formData = new FormData();
      formData.append("file", file, file.name);
      xhr.send(formData);
    });
  }

}