// talkService.ts
import {Injectable, OnInit, OnDestroy } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable, BehaviorSubject} from 'rxjs/Rx';
import {Subject} from "rxjs/Subject";

import {Talk} from "./talk";

@Injectable()
export class TalkService {

  public talks: Array<Talk> = [];

  constructor(private http: Http) {
    this.loadInitialData();
  }

  private get baseUrl() {
    return '/' + this.moniker + "/api/cfs/speaker";
  }

  private get moniker() {
    return window.location.pathname.split('/')[1];
  }

  loadInitialData() {

    this.http.get(this.baseUrl)
      .subscribe(res => {
        var resTalks = res.json().talks;
        resTalks.forEach((t:any) => this.talks.push(t));
      }, err => console.log(err));

  }

  saveTalk(talk: Talk) {

    return new Promise((resolve, reject) => {

      var oldTalk = this.talks.splice(this.talks.indexOf(talk), 1);

      var obj = this.http.post(this.baseUrl + "/talk", talk)
        .subscribe(res => {
          var updatedTalk = res.json();
          this.talks.push(updatedTalk);
          resolve(updatedTalk);
        }, error => reject(error));
    });
  }

  delete(talk: Talk) {
    var obj = this.http.delete(this.baseUrl + "/talk/" + talk.id)
      .subscribe(res => {
        this.talks.splice(this.talks.indexOf(talk), 1);
      });
  }
}