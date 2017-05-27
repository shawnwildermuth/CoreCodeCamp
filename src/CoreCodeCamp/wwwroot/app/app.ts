/// <reference path="common/validators.ts" />
/// <reference path="common/filters.ts" />
/// <reference path="common/datepicker.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VeeValidate: any;
  declare var VueResource: any;
  declare var VueRouter: any;

  export let App = {

    setup: function() {
      
      Vue.use(VeeValidate);
      Vue.use(VueResource);
      CodeCamp.Common.createValidators();
      CodeCamp.Common.createFilters();
      CodeCamp.Common.createDatePicker();
    },

    bootstrap: function (theView: any) {
      this.setup();
      new Vue(theView);
    },
  }
}