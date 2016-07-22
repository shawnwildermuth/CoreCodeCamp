// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';

// Turn on Production Mode
import { buildType } from "../common/buildType";
buildType();


import { SponsorForm } from './sponsorForm';
import { DataService } from "../common/dataService";
import { ImageUploadService } from '../common/imageUploadService';

bootstrap(SponsorForm,
  [disableDeprecatedForms(),
    provideForms(),
    HTTP_PROVIDERS,
    ImageUploadService,
    DataService]);