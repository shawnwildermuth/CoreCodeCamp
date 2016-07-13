// talksForm.ts
import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import {Http, Headers} from '@angular/http';
import { TalkService }  from "./talkService";

@Component({
  moduleId: module.id, // To make urls become relative!
  templateUrl: "talksForm.html",
  directives: [ROUTER_DIRECTIVES]
})
export class TalksForm {

  isBusy: boolean = false;
  error: string = "";
  talks: Array<any> = [];
  private _http: Http;
  private _talkService: TalkService;

  constructor(http: Http, talkService: TalkService) {
    this._http = http;
    this._talkService = talkService;
    this.talks = this._talkService.talks;
    this.getTalks();
  }

  getTalks() {
    this.isBusy = true;
    this._talkService.getTasks();
  }

}