///<reference path="../common/dataService.ts" />
namespace CodeCamp.Common {

  declare var Vue: any;

  export class ImageUploadService {

    uploadSpeaker(img: File) {
      return this.uploadImage(img, "speakers");
    }

    uploadSponsor(img: File, moniker: string = CodeCamp.Common.dataService.moniker) {
      return this.uploadImage(img, "sponsors", moniker);
    }

    private uploadImage(file: File, imageType: string, moniker: string = CodeCamp.Common.dataService.moniker) {
      return new Vue.Promise((resolve, reject) => {

        let xhr: XMLHttpRequest = new XMLHttpRequest();
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

  export let imageUploadService = new ImageUploadService();
}