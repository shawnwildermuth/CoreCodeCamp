// main.ts
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';
import { ngModule } from "@angular/core";

// Turn on Production Mode
import { buildType } from "../common/buildType";
buildType();


import { EventInfoForm } from './eventInfoForm';
import { DataService } from "../common";

@ngModule({
  bootstrap: EventInfoForm,
  
})
class EventModule { }

platformBrowserDynamic().bootstrapModule(EventModule);


//EventInfoForm,
//  [disableDeprecatedForms(),
//    provideForms(),
//    HTTP_PROVIDERS,
//    DataService]);