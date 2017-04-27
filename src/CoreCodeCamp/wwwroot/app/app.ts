
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VeeValidate: any;
  declare var VueResource: any;

  export let App = {

    setup: function() {
      
      Vue.use(VeeValidate);
      Vue.use(VueResource);
      let passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
      VeeValidate.Validator.extend('strongPassword', {
        getMessage: field => 'The ' + field + ' requires an uppercase, a lower case and a number.',
        validate: value => passwordValidation.test(value)
      });
    },

    bootstrap: function (theView: any) {
      this.setup();
      new Vue(theView);
    }
  }
}