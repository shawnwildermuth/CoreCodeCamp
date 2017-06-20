/// <reference path="./speakerData.ts" />
namespace CodeCamp {
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
    methods: {
      onDeleteTalk(talk) {
        this.busy = true;
        this.errorMessage = "";
        CodeCamp.speakerData.deleteTalk(talk)
          .then(() => { }, () => this.errorMessage = "Failed to delete talk")
          .finally(() => this.busy = false);
      }
    },
    mounted() {
      if (!this.speaker) {
        let _this = this;
        CodeCamp.speakerData.getSpeaker()
          .then(skr => {
            if (!skr || skr.id == 0) {
              CodeCamp.callForSpeakersRouter.router.push({ name: "editor" });
            } else {
              _this.speaker = skr;
            }
          },
          () => _this.errorMessage = "Failed to load speaker")
      }
    }
  };
}