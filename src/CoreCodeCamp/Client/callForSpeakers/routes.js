// External JS Libraries
import Vue from "vue";
import VueRouter from "vue-router";
import SpeakerInfoView from "./speakerInfoView";
import SpeakerEditorView from "./speakerEditorView";
import SpeakerTalkEditorView from "./speakerTalkEditorView";

let routes = [
  { path: "/", redirect: { name: "info" } },
  { path: "/info", name: "info", component: SpeakerInfoView },
  { path: "/edit", name: "editor", component: SpeakerEditorView },
  { path: "/talks/:id", name: "talkEditor", component: SpeakerTalkEditorView, props: true },
  { path: "*", redirect: { name: "info" } }
];

let routerView = {
  router: new VueRouter({
    routes: routes
  })
};

export default routerView; 

