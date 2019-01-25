import Vue from "vue";
import _ from "lodash";
import dataService from "../common/dataService";

class SpeakerService {

  _speaker = null;

  getSpeaker() {
    let _this = this;
    return new Vue.Promise((resolve, reject) => {
      if (_this._speaker == null) {
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

  saveSpeaker(speaker) {
    this._speaker = speaker;
    return dataService.saveSpeaker(speaker);
  }

  deleteTalk(talk) {
    let _this = this;
    return new Vue.Promise(function (resolve, reject) {
      dataService.deleteTalk(talk.id)
        .then( () => {
          _this._speaker.talks.splice(_this._speaker.talks.indexOf(talk), 1)
          resolve();
        }, () => reject());
    });
  }

  saveTalk(talk) {
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
        }, function (err) {
          reject(err.data);
        });
    });
  }

}
var speakerData = new SpeakerService();
export default speakerData;
