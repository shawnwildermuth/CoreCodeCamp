// talksForm.ts
import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
import { TalkService }  from "./talkService";
import { Talk } from "./talk";

@Component({
  moduleId: module.id, // To make urls become relative!
  templateUrl: "/js/app/talks/talksForm.html",
  directives: [ROUTER_DIRECTIVES]
})
export class TalksForm {

  constructor(private talkService: TalkService, private router: Router) {
  }

  onEdit(talk: Talk) {
    this.router.navigate(["/edit/" + talk.id]);
  }

  onDelete(talk: Talk) {
    this.talkService.delete(talk);
  }
}