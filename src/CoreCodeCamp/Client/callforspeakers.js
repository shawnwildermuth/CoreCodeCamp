import Vue from "vue";
import VueRouter from "vue-router";
import mount from "./common/mount";
import router from "./callForSpeakers/routes";

Vue.use(VueRouter);

mount(router, "#call-for-speaker-view"); 
