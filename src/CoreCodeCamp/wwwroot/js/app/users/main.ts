// main.ts
import { bootstrap } from '@angular/platform-browser-dynamic';
import { UsersForm } from './usersForm';
import { HTTP_PROVIDERS } from '@angular/http';
import { UserService } from "./userService";

bootstrap(UsersForm, [HTTP_PROVIDERS, UserService]);