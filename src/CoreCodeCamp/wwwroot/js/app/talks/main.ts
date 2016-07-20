// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { disableDeprecatedForms, provideForms } from '@angular/forms';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';

import { TalkService } from "./talkService";
import { DataService } from "../common/dataService";
import { TalksComponent } from './TalksComponent';
import { talkRoutes } from './routes';

bootstrap(TalksComponent, [disableDeprecatedForms(),
  provideForms(),
  HTTP_PROVIDERS,
  ROUTER_DIRECTIVES,
  talkRoutes,
  DataService,
  TalkService,
  { provide: LocationStrategy, useClass: HashLocationStrategy }
]);
