import { Component }          from '@angular/core';
import { ROUTER_DIRECTIVES }  from '@angular/router';
import { Location }           from "@angular/common";

@Component({
  selector: 'talks',
  template: `<h3>Your Talks</h3>
    <router-outlet></router-outlet>`,
  directives: [ROUTER_DIRECTIVES]
})
export class TalksComponent{
  constructor(location: Location) {
  }
}