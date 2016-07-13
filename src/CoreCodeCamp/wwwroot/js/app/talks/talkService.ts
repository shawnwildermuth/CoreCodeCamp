// talkService.ts
import {Injectable } from '@angular/core';
import {Http, Headers} from '@angular/http';
import {Observable} from 'rxjs/Rx';

@Injectable()
export class TalkService {
  _http: Http;
  _talks: Array<any>;

  constructor(http: Http) {
    this._http = http;
  }
  
  get talks() { return this._talks; }

  private get baseUrl() {
    return '/' + this.moniker + "/api/cfs/speaker";
  }

  private get moniker() {
    return window.location.pathname.split('/')[1];
  }

  getTasks() {
    return new Promise<Boolean>((resolve, reject) => {

      this._http.get(this.baseUrl)
        .subscribe(res => {
          this._talks = res.json().talks;
          resolve(true);
        }, err => reject(err));

    });
  }

}