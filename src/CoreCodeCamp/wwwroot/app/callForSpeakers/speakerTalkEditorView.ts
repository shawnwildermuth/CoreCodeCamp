/// <reference path="../common/helpers.ts" />
/// <reference path="../common/dataservice.ts" />
/// <reference path="speakerdata.ts" />
module CodeCamp {
  declare var Vue: any;
  declare var _: any;

  export let SpeakerTalkEditorView = {
    template: "#speaker-talk-editor",
    data() {
      return {
        busy: true,
        errorMessage: "",
        speaker: {},
        talk: {}
      };
    },
    methods: {
      onSave() {
        this.$validator.validateAll().then(result => {
          if (result) {
            this.busy = true;
            this.errorMessage = "";
            CodeCamp.speakerData.saveTalk(this.talk).then(function (result) {
              CodeCamp.callForSpeakersRouter.router.push({ name: "info" });
            }, function () {
              this.errorMessage = "Failed to save speaker."
            }).then(() => this.busy = false);
          }
        });
      }
    },
    computed: {
      isPristine: function () {
        return CodeCamp.Common.helpers.isPristine(this.fields);
      }
    },
    mounted() {
      CodeCamp.speakerData.getSpeaker()
        .then(function (skr) {
          if (skr) this.speaker = skr;
          let theId = this.$route.params.id;
          if (theId != 'new') {
            this.talk = _.find(this.speaker.talks, t => t.id == theId);
          } 
          this.busy = false;
        }.bind(this),
        function () {
          this.errorMessage = "Failed to load speaker";
          this.busy = false;
        }.bind(this));
    }

  };
}