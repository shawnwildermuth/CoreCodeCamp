module CodeCamp {

  declare var Vue: any;

  export let SpeakerEditorView = {
    template: "#speaker-editor",
    data() {
      return {
        busy: true,
        errorMessage: "",
        speaker: {},
        imageError: "",
        filePicker: {}
      };
    },
    methods: {
      onSaveSpeaker() {
        this.$validator.validateAll().then(result => {
          if (result) {
            this.busy = true;
            this.errorMessage = "";
            CodeCamp.speakerData.saveSpeaker(this.speaker).then(function () {
              CodeCamp.callForSpeakersRouter.router.push({ name: "info" });
            }, function () {
              this.errorMessage = "Failed to save speaker."
            }).finally(() => this.busy = false);
          }
        });
      },
      onFilePicker() {
        this.$refs.filePicker.click();
      },
      onImagePicked() {
        this.isBusy = true;
        this.$imgService.uploadSpeaker(this.$refs.filePicker.files[0])
          .then((imageUrl: any) => {
            this.speaker.imageUrl = imageUrl;
            this.$forceUpdate();
          }, (e) => {
            this.imageError = e;
          })
          .finally(() => this.isBusy = false);
      },
      validImage() {
        return this.speaker && this.speaker.imageUrl && this.speaker.imageUrl.length > 0;
      }
    },
    mounted() {
      this.$imgService = new CodeCamp.Common.ImageUploadService();
      CodeCamp.speakerData.getSpeaker()
        .then(function (skr) {
          if (skr) this.speaker = skr;
          this.busy = false;
        }.bind(this),
        function () {
          this.errorMessage = "Failed to load speaker";
          this.busy = false;
        }.bind(this));
    }
  }
}