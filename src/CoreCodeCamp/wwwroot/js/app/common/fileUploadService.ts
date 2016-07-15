// fileUploadService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';

@Injectable()
export class FileUploadService {

  constructor(private http: Http) { }

  uploadFile(file: File, url: string): Promise<string> {
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

      xhr.open('POST', url, true);

      let formData = new FormData();
      formData.append("file", file, file.name);
      xhr.send(formData);
    });
  }

}