import Vue from 'lib/vue';
import VeeValidate from 'lib/vee-validate';

export default class App {

  constructor(private view: Vue) {

  }

  private setup() {
    Vue.use(VeeValidate);
    let passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
    VeeValidate.Validator.extend('strongPassword', {
      getMessage: field => 'The ' + field + ' requires an uppercase, a lower case and a number.',
      validate: value => passwordValidation.test(value)
    });
  }

  bootstrap() {
    this.setup();
    new Vue({
      el: "body",
      render: h => h(this.view)
    })
  }

}