///<reference path="./callForSpeakers.ts" />
///<reference path="./speakerEditorView.ts" />
///<reference path="./speakerInfoView.ts" />
///<reference path="./speakerTalkEditorView.ts" />
namespace CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VueRouter: any;

  let routes = [
    { path: "/", redirect: { name: "info" } },
    { path: "/info", name: "info", component: CodeCamp.SpeakerInfoView },
    { path: "/edit", name: "editor", component: CodeCamp.SpeakerEditorView },
    { path: "/talks/:id", name: "talkEditor", component: CodeCamp.SpeakerTalkEditorView, props: true },
    { path: "*", redirect: { name: "info" } }
  ];

  export let callForSpeakersRouter = {
    router: new VueRouter({
      routes: routes
    })
  }; 

}