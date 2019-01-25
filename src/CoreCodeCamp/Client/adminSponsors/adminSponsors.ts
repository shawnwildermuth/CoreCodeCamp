///<reference path="../common/dataService.ts" />
namespace CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var jQuery: any;

  export function adminSponsors() {
    CodeCamp.App.bootstrap(adminSponsorsView);
  }

  export let adminSponsorsView = {
    el: "#sponsors-view",
    data: {
      busy: true,
      sponsors: [],
      currentSponsor: null,
      errorMessage: "",
      userMessage: "",
      imageError: ""
    },
    computed: {
      validImage: function() {
        return (this.currentSponsor && this.currentSponsor.imageUrl && this.currentSponsor.imageUrl.length > 0) ? true : false;
      },
      isPristine: function () {
        return 
      }
    },
    methods: {
      onTogglePaid(sponsor) {
        this.busy = true;
        CodeCamp.Common.dataService.togglePaid(sponsor)
          .then(() => sponsor.paid = !sponsor.paid, () => this.errorMessage = "Could not toggle paid")
          .finally(() => this.busy = false);
      },
      onEdit(sponsor) {
        this.currentSponsor = sponsor;
        this.$validator.validateAll();
      },
      onDelete(sponsor) {
        this.busy = true;
        CodeCamp.Common.dataService.deleteSponsor(sponsor)
          .then(() => this.sponsors.splice(this.sponsors.indexOf(sponsor), 1), () => this.errorMessage = "Could not delete sponsor")
          .finally(() => this.busy = false);
      },
      onNew() {
        this.currentSponsor = {};
        this.$validator.validateAll();
      },
      onSave() {

        var old = this.sponsors.indexOf(this.currentSponsor);
        if (old > -1) this.sponsors.splice(this.sponsors.indexOf(this.currentSponsor), 1);
        this.busy = true;

        CodeCamp.Common.dataService.saveSponsor(this.currentSponsor)
          .then(res => {
            this.sponsors.push(res.data);
            this.currentSponsor = null;
          }, () => this.errorMessage = "Could not save sponsor")
          .finally(() => this.busy = false);
      },
      onCancel() {
        this.currentSponsor = null;
      },
      onImagePicked(filePicker) {
        this.busy = true;
        let file = jQuery("#thePicker")[0].files[0];
        CodeCamp.Common.imageUploadService.uploadSponsor(file)
          .then((imageUrl: any) => {
            Vue.set(this.currentSponsor, "imageUrl", imageUrl);
          }, (e) => this.errorMessage = "Failed to upload Image")
          .finally(() => this.busy = false);
      },
      onShowPicker() {
        jQuery("#thePicker").click();
      }
    },
    mounted() {
      CodeCamp.Common.dataService.getSponsors()
        .then(function (result) {
          this.sponsors = result.data;
          this.currentSponsor = null;
        }.bind(this), () => this.errorMessage = "Failed to load data")
        .finally(() => this.busy = false);

    }
  };
}

