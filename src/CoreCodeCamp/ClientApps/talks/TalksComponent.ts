import { Component }          from '@angular/core';
import { ROUTER_DIRECTIVES }  from '@angular/router';
import { TalkEditor } from "./talkEditor";
import { TalksForm } from "./talksForm";


@Component({
  selector: 'talks',
  template: `<router-outlet></router-outlet>`,
  directives: [ROUTER_DIRECTIVES],
  precompile: [TalkEditor, TalksForm]
})
export class TalksComponent{
}