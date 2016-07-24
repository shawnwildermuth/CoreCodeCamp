// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';

// Turn on Production Mode
import { buildType } from "../common/buildType";
buildType();


import { ScheduleForm } from './scheduleForm';
import { DataService } from "../common/dataService";

bootstrap(ScheduleForm,
  [disableDeprecatedForms(),
    provideForms(),
    HTTP_PROVIDERS,
    DataService]);