import Vue from 'lib/vue';
import VeeValidate from "lib/vee-validate";

export default class JoinView extends Vue {

  constructor() {
    super({
      el: "#join-view",
    });
  }

  private mail: {
    name: "",
    email: "",
    password: "",
    confirmPassword: ""
  };
  private errorMessage: "";


  onSubmit() {
    var me = this;
    this.$validator.validateAll().then(function (success) {
      if (!success) {
        me.errorMessage = "Please fix validation Issues";
        return false;
      }
    });
  }


  isPristine() {
    var val = (this.mail.name == "" || this.mail.email == "" || this.mail.password == "" || this.mail.confirmPassword == "");
    return val;
  }

  created() {
    this.$set(this, 'errors', this.$validator.errorBag)
  }



}