/// <reference path="../common/dataService.ts" />
namespace CodeCamp {

  declare var Vue: any;
  declare var VueResource: any;
  declare var Promise: any; 
  declare var _: any;


  export class SpeakerService {

    _speaker = null;

    getSpeaker() {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        if (this._speaker == null) {
          Vue.Promise.all([
            CodeCamp.Common.dataService.getMySpeaker(),
            CodeCamp.Common.dataService.getTalks()
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

    saveSpeaker(speaker) {
      this._speaker = speaker;
      return CodeCamp.Common.dataService.saveSpeaker(speaker);
    }

    deleteTalk(talk) {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        CodeCamp.Common.dataService.deleteTalk(talk.id)
          .then(function () {
            _this._speaker.talks.splice(_this._speaker.talks.indexOf(talk), 1)
            resolve();
          }, () => reject());
      });
    }

    saveTalk(talk) {
      let _this = this;
      return new Vue.Promise(function (resolve, reject) {
        CodeCamp.Common.dataService.saveTalk(talk)
          .then(function (result) {
            let resultTalk = result.body;
            let talk = _.find(_this._speaker.talks, t => t.id == resultTalk.id);
            if (!talk) {
              _this._speaker.talks.push(resultTalk);
            }
            resolve();
          }, function (err) {
            reject(err.data);
          });
      });
    }

  }

  export var speakerData: SpeakerService = new SpeakerService();
}