module CodeCamp {

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
        var val = (this.user.name == "" || this.user.email == "" || this.user.password == "" || this.user.confirmPassword == "");
        return val;
      }
    },
    methods: {
      onSubmit() {
        window.alert("Submitting!");
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




