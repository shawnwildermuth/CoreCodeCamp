/// <reference path="../common/dataService.ts" />
namespace CodeCamp {

  declare var Vue: any;
  declare var VueResource: any;
  declare var Promise: any; 
  declare var _: any;


  export class SpeakerService {

    _speaker = null;
    _dataService = null;

    getSpeaker(dataService) {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        if (this._speaker == null) {
          Vue.Promise.all([
            dataService.getMySpeaker(),
            dataService.getTalks()
          ])
            .then(result => {
              _this._speaker = result[0].data;
              _this._speaker.talks = result[1].data;
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

    deleteTalk(dataService, talk) {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        dataService.deleteTalk(talk.id)
          .then(function () {
            _this._speaker.talks.splice(_this._speaker.talks.indexOf(talk), 1)
            resolve();
          }, () => reject());
      });
    }

    saveTalk(dataService, talk) {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        dataService.saveTalk(talk)
          .then(function (result) {
            let resultTalk = result.body;
            let talk = _.find(_this._speaker.talks, t => t.id == resultTalk.id);
            if (!talk) {
              _this._speaker.talks.push(resultTalk);
            }
            resolve();
          }, function () {
            reject();
          });
      });
    }

  }

  export var speakerData: SpeakerService = new SpeakerService();
}