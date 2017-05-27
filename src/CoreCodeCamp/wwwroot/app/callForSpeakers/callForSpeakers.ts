///<reference path="../common/imageUploadService.ts" />
///<reference path="../common/dataService.ts" />
///<reference path="./Routes.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VueRouter: any;

  export function callForSpeakers() {
    Vue.use(VueRouter);
    CodeCamp.App.bootstrap({ router: CodeCamp.callForSpeakersRouter.router, el: "#call-for-speaker-view" });
  }

}