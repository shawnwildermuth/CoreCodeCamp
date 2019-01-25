import Vue from 'vue';
import bootstrap from "./common/bootstrap";
import helpers from "./common/helpers";

let theView = {
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
    },
    created() {
      this.$set(this, 'errors', this.$validator.errorBag)
    }

  }

};



bootstrap(theView, "#view-join"); 