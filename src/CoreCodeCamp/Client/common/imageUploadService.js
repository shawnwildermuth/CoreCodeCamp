
import Vue from "vue";
import dataService from "../common/dataService";

class ImageUploadService {

  uploadSpeaker(img) {
    return this.uploadImage(img, "speakers");
  }

  uploadSponsor(img, moniker = dataService.moniker) {
    return this.uploadImage(img, "sponsors", moniker);
  }

  uploadImage(file, imageType, moniker = dataService.moniker) {
    return new Vue.Promise((resolve, reject) => {

      let xhr = new XMLHttpRequest();
      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4) {
          if (xhr.status >= 200 && xhr.status <= 299) {
            let location = xhr.getResponseHeader("location");
            if (!location) location = JSON.parse(xhr.responseText).location;
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

let imageUploadService = new ImageUploadService();
export default imageUploadService;
