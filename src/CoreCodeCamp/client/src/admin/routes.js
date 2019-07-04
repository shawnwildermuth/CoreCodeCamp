// External JS Libraries
import Vue from "vue";
import VueRouter from "vue-router";
import Admin from "./admin";
import Talks from "./talks";
//import Schedule from "./schedule";
//import Sponsors from "./sponsors";
import Users from "./users";

Vue.use(VueRouter);

let routes = [
  { path: "/", name: "admin", component: Admin },
  { path: "/talks", name: "talks", component: Talks},
  //{ path: "/schedule", name: "schedule", component: Schedule},
  //{ path: "/sponsors", name: "sponsors", component: Sponsors },
  { path: "/users", name: "users", component: Users},
  { path: "*", redirect: { name: "admin" } }
];

export default new VueRouter({
  routes: routes
});

