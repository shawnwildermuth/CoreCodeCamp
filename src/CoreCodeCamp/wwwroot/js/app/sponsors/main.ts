// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';

import { SponsorForm } from './sponsorForm';
import { SponsorService } from "./sponsorService";
import { FileUploadService } from '../common/fileUploadService';

bootstrap(SponsorForm,
  [disableDeprecatedForms(),
    provideForms(),
    HTTP_PROVIDERS,
    SponsorService,
    FileUploadService]);