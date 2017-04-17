(function () {

  Vue.use(VeeValidate);
  let passwordValidation = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,25}$/;
  VeeValidate.Validator.extend('strongPassword', {
    getMessage: field => 'The ' + field + ' requires an uppercase, a lower case and a number.',
    validate: value => passwordValidation.test(value)
  });

  var vm = new Vue({
    el: "#join-view",
    data: {
      mail: {
        name: "",
        email: "",
        password: "",
        confirmPassword: ""
      },
      errorMessage: ""
    },

    methods: {
      onSubmit() {
        var me = this;
        this.$validator.validateAll().then(function (success) {
          if (!success) {
            me.errorMessage = "Please fix validation Issues";
            return false;
          }
        });
      }
    },
    computed: {
      isPristine: function () {
        var val = (this.mail.name == "" || this.mail.email == "" || this.mail.password == "" || this.mail.confirmPassword == "");
        return val;
      }
    },
    created: function () {
      this.$set(this, 'errors', this.$validator.errorBag)
    }
  });

})();



