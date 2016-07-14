// routes.ts
import { provideRouter, RouterConfig } from '@angular/router';
import { TalksForm } from "./talksForm";
import { TalkEditor } from "./talkEditor";

export const talkRoutes: RouterConfig = [
  provideRouter([
    {
      path: '',
      redirectTo: 'speaker',
      pathMatch: 'full'
    },
    { path: 'edit/:id', component: TalkEditor },
    { path: 'speaker', component: TalksForm }
  ])
];