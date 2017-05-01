///<reference path="../common/dataService.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var _: any;

  export function users() {
    CodeCamp.App.bootstrap(CodeCamp.UsersView);
  }

  export let UsersView = {
    el: "#users-view",
    data: {
      users: [],
      errorMessage: "",
      busy: true;
    },
    methods: {
      onToggleAdmin(user) {
        this.busy = true;

        this.$dataService.toggleAdmin(user).then(function (result) {
          user.isAdmin = result.data;
        }, function () {
          this.errorMessage = "Failed to toggle admin";
        }).finally(() => this.busy = false);
      },

      onToggleConfirmation(user) {
        this.busy = true;

        this.$dataService.toggleConfirmation(user).then(function (result) {
          user.isEmailConfirmed = result.data;
        }, function () {
          this.errorMessage = "Failed to toggle confirmation";
        }).finally(() => this.busy = false);
      }
    },
    mounted() {
      this.$dataService = new CodeCamp.Common.DataService(this.$http);

      this.$dataService.getUsers().then(function (result) {
        this.users = result.data;
      }, function () {
        this.errorMessage = "Failed to get user data";
      }).finally(() => this.busy = false);
    }



  };
}
