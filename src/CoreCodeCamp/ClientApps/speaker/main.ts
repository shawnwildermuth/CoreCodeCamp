// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { disableDeprecatedForms, provideForms } from '@angular/forms';
import { HTTP_PROVIDERS } from '@angular/http';

// Turn on Production Mode
import { buildType } from "../common/buildType";
buildType();

import { SpeakerForm } from './speakerForm';
import { ImageUploadService } from "../common/imageUploadService";
import { DataService } from "../common/dataService";

bootstrap(SpeakerForm,
  [disableDeprecatedForms(),
    provideForms(),
    HTTP_PROVIDERS,
    ImageUploadService,
    DataService]);