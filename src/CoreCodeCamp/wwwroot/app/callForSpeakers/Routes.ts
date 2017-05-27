///<reference path="./callForSpeakers.ts" />
///<reference path="./speakerEditorView.ts" />
///<reference path="./speakerInfoView.ts" />
///<reference path="./speakerTalksView.ts" />
namespace CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VueRouter: any;

  let routes = [
    { path: "/", redirect: { name: "info" } },
    { path: "/info", name: "info", component: CodeCamp.SpeakerInfoView },
    { path: "/edit", name: "editor", component: CodeCamp.SpeakerEditorView },
    { path: "/talks", name: "talks", component: CodeCamp.SpeakerTalksView },
  ];

  export let callForSpeakersRouter = {
    router: new VueRouter({
      routes: routes
    })
  }; 

}