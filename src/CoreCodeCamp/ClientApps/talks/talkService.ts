// talkService.ts
import {Injectable, OnInit, OnDestroy } from '@angular/core';
import {Observable, BehaviorSubject} from 'rxjs/Rx';
import {Subject} from "rxjs/Subject";

import { DataService } from "../common/dataService";
import {Talk} from "./talk";

@Injectable()
export class TalkService {

  public talks: Array<Talk> = [];

  constructor(private data: DataService) {
    this.loadInitialData();
  }

  loadInitialData() {

    this.data.getTalks()
      .subscribe(res => {
        var resTalks = res.json();
        resTalks.forEach((t:any) => this.talks.push(t));
      }, err => console.log(err));

  }

  saveTalk(talk: Talk) {

    return new Promise((resolve, reject) => {

      var index = this.talks.indexOf(talk);
      if (index > -1) this.talks.splice(index, 1);

      var obj = this.data.saveTalk(talk)
        .subscribe(res => {
          var updatedTalk = res.json();
          this.talks.push(updatedTalk);
          resolve(updatedTalk);
        }, error => reject(error));
    });
  }

  delete(talk: Talk) {
    var obj = this.data.deleteTalk(talk.id)
      .subscribe(res => {
        this.talks.splice(this.talks.indexOf(talk), 1);
      });
  }
}