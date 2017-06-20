/// <reference path="../common/helpers.ts" />
namespace CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var VeeValidate: any;

  export let JoinView = {
    el: "#join-view",
    data: {
      user: {
        name: "",
        email: "",
        password: "",
        confirmPassword: ""
      },
      errorMessage: ""
    },
    computed: {
      isPristine: function () {
        return CodeCamp.Common.helpers.isPristine(this.fields);
      }
    },
    methods: {
      onSubmit() {
        let me = this;
        this.$validator.validateAll().then(function (success) {
          if (!success) {
            me.errorMessage = "Please fix validation Issues";
            return false;
          }
        });
      },
      created() {
        this.$set(this, 'errors', this.$validator.errorBag)
      }

    }

  };
}




