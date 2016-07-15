// fileUploadService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';

@Injectable()
export class ImageUploadService {

  constructor(private http: Http) { }

  uploadImage(file: File, imageType: string, imagePath: string): Promise<string> {
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

      xhr.open('POST', "/api/images/" + imageType + "?imagePath=" + encodeURIComponent(imagePath), true);

      let formData = new FormData();
      formData.append("file", file, file.name);
      xhr.send(formData);
    });
  }

}