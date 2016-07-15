// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { SpeakerForm } from './speakerForm';
import { disableDeprecatedForms, provideForms } from '@angular/forms';
import { HTTP_PROVIDERS } from '@angular/http';
import { FileUploadService } from "../common/fileUploadService";

bootstrap(SpeakerForm,
  [ disableDeprecatedForms(),
    provideForms(),
    HTTP_PROVIDERS,
    FileUploadService ]);