// talksForm.ts
import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { Http, Headers } from '@angular/http';
import { ActivatedRoute } from '@angular/router';
import { OnInit, OnDestroy } from "@angular/core";

import { TalkService } from "./talkService";

class Talk {
  id: number;
  title: string;
  abstract: string;
  prerequisites: string;
  audience: string;
  level: number;
}

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

  constructor(private route: ActivatedRoute, private talkService: TalkService) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      let id = params['id'];

      if (id == "new") {
        this.model = new Talk();
      } else {
        this.model = this.talkService.talks.find(t => t.Id == +id);
      }

    });

  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }


}