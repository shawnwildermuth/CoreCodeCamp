// External JS Libraries
import Vue from "vue";
import VueRouter from "vue-router";
import Admin from "./admin";
import Talks from "./talks";
import CampEditor from "./campEditor";
import Schedule from "./schedule";
import Sponsors from "./sponsors";
import Users from "./users";
import SponsorEditor from "./sponsorEditor";

Vue.use(VueRouter);

let routes = [
  { path: "/", name: "admin", component: Admin },
  { path: "/talks", name: "talks", component: Talks},
  { path: "/camp/edit/:type", name: "campEditor", component: CampEditor },
  { path: "/schedule", name: "schedule", component: Schedule},
  { path: "/sponsors", name: "sponsors", component: Sponsors },
  { path: "/sponsors/edit/:type", name: "sponsorEditor", component: SponsorEditor, props: true },
  { path: "/users", name: "users", component: Users},
  { path: "*", redirect: { name: "admin" } }
];

export default new VueRouter({
  routes: routes
});

