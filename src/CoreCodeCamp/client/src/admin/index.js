import Vue from "vue";
import VueRouter from "vue-router";
import mount from "../common/mount";
import router from "./routes";
import App from "./app";
import store from "./store/index";
import VModal from 'vue-js-modal'
import VueDragDrop from 'vue-drag-drop';

Vue.use(VueDragDrop);
Vue.use(VModal)
Vue.use(VueRouter);

mount({
  router,
  store,
  components: { App },
  template: "<app></app>",
}, "#admin-view"); 
