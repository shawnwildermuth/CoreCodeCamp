// mount.js
import Vue from "vue";
import VeeValidate from "vee-validate";
import VueResource from "vue-resource";
import validators from "./validators";
import filters from "./filters";
import datePicker from "./datepicker";

export default function mount(theView, el) {

  Vue.config.productionTip = false;
  Vue.use(VeeValidate);
  Vue.use(VueResource);
  validators();
  filters();
  datePicker();
  Vue.config.errorHandler = function (err, vm, info) {
    console.log(err);
  };
  new Vue(theView).$mount(el);
}