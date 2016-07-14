// talksForm.ts
import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { Http, Headers } from '@angular/http';
import { ActivatedRoute, Router } from '@angular/router';
import { OnInit, OnDestroy } from "@angular/core";
import { Talk } from "./talk";
import {Observable, BehaviorSubject} from 'rxjs/Rx';

import { TalkService } from "./talkService";


@Component({
  moduleId: module.id, // To make urls become relative!
  templateUrl: "talkEditor.html",
  directives: [ROUTER_DIRECTIVES]
})
export class TalkEditor implements OnInit {

  model: Talk;
  isBusy: boolean = false;
  error: string = "";
  sub: any;

  constructor(private route: ActivatedRoute, private talkService: TalkService, private router: Router) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      let id = params['id'];

        if (id == "new") {
          this.model = new Talk();
        } else {
          this.model = this.talkService.talks.find(t => t.id == id);
        }
    });

  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  onSave() {
    this.isBusy = true;
    this.talkService.saveTalk(this.model)
      .then(() => {
        this.router.navigate(["/speaker"]);
      },
      err => this.error = err)
      .then(() => this.isBusy = false);
  }

}