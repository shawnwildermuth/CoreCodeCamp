/// <reference path="./speakerData.ts" />
module CodeCamp {
  declare var Vue: any;

  export let SpeakerInfoView = {
    template: "#speaker-info",
    data() {
      return {
        busy: true,
        errorMessage: "",
        speaker: null
      };
    },
    mounted() {
      this.$dataService = new CodeCamp.Common.DataService(this.$http);
      let _this = this;
      CodeCamp.speakerData.getSpeaker(this.$dataService)
        .then(skr => {
          if (!skr) {
            CodeCamp.callForSpeakersRouter.router.push({ name: "editor" });
          } else {
            _this.speaker = skr;
          }
        },
        () => _this.errorMessage = "Failed to load speaker")
    }
  };
}