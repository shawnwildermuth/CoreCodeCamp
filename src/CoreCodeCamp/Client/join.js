import Vue from 'vue';
import mount from "./common/mount";
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

mount(theView, "#view-join"); 