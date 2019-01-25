// External JS Libraries
import _ from "lodash";
import mount from "../common/mount";
import dataService from "../common/dataService";

let theView = {
  data: {
    users: [],
    errorMessage: "",
    busy: true
  },
  methods: {
    onToggleAdmin(user) {
      this.busy = true;

      dataService.toggleAdmin(user).then((result) => {
        user.isAdmin = result.data;
      }, () => {
        this.errorMessage = "Failed to toggle admin";
      }).finally(() => this.busy = false);
    },

    onToggleConfirmation(user) {
      this.busy = true;

      dataService.toggleConfirmation(user).then((result) => {
        user.isEmailConfirmed = result.data;
      }, () => {
        this.errorMessage = "Failed to toggle confirmation";
      }).finally(() => this.busy = false);
    }
  },
  mounted() {
    dataService.getUsers().then((result) => {
      this.users = result.data;
    }, () => {
      this.errorMessage = "Failed to get user data";
    }).finally(() => this.busy = false);
  }
};

mount(theView, "#users-view");
