import mount from "./common/mount";
import helpers from "./common/helpers";

let theView = {
  data: () => {
    return {
      user: {
        name: "",
        email: "",
        password: "",
        confirmPassword: ""
      },
      errorMessage: ""
    };
  },
  computed: {
    isPristine: function () {
      return helpers.isPristine(this.fields);
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
    }

  }

};

mount(theView, "#join-view"); 