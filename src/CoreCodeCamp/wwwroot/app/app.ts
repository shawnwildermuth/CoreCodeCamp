module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VeeValidate: any;
  declare var VueResource: any;
  declare var moment: any;

  export let App = {

    setup: function() {
      
      Vue.use(VeeValidate);
      Vue.use(VueResource);
      let passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
      VeeValidate.Validator.extend('strongPassword', {
        getMessage: field => 'The ' + field + ' requires an uppercase, a lower case and a number.',
        validate: value => passwordValidation.test(value)
      });
      Vue.filter('formatDate', function (value) {
        if (value) {
          return moment(String(value)).format('MM-DD-YYYY')
        }
      };
      Vue.filter('formatTime', function (value) {
        if (value) {
          return moment(String(value)).format('hh:mm a')
        }
      };
    },

    bootstrap: function (theView: any) {
      this.setup();
      new Vue(theView);
    }
  }
}