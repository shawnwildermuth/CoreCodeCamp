///<reference path="../common/imageUploadService.ts" />
///<reference path="../common/dataService.ts" />
///<reference path="./Routes.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;

  export function callForSpeakers() {
    Vue.use(VueRouter);
    CodeCamp.App.bootstrap({ router: CodeCamp.createRouter(), el: "#call-for-speaker-view" });
  }

}