/// <reference path="../common/dataService.ts" />
namespace CodeCamp {

  declare var VueResource: any;
  declare var Promise: any; 


  export class Speaker {

    _speaker = null;
    _dataService = null;

    getSpeaker(dataService) {
      let _this = this;
      return new Promise(function (resolve, reject) {
        if (this._speaker == null) {
          dataService.getMySpeaker()
            .then(result => {
              _this._speaker = result.body;
              resolve(_this._speaker);
            }, () => {
              reject();
            });

        } else {
          resolve(_this._speaker);
        }
      });
    }

    saveSpeaker(dataService, speaker) {
      this._speaker = speaker;
      return dataService.saveSpeaker(speaker);
    }

  }

  export var speakerData: Speaker = new Speaker();
}