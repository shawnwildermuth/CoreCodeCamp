///<reference path="../common/dataService.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var _: any;

  export function adminEvents() {
    CodeCamp.App.bootstrap(CodeCamp.AdminEventsView);
  }

  export let AdminEventsView = {
    el: "#events-view",
    data: {
      campEvents: [],
      errorMessage: "",
      currentEvent: null,
      newEventMoniker: "",
      selectedModelMoniker: ""
    },
    methods: {
      onEventChanged(moniker) {
        this.currentEvent = _.find(this.campEvents, e => e.moniker === moniker);
      },

      onAddEvent() {
        CodeCamp.Common.dataService.addEventInfo(this.newEventMoniker).then(function (result) {
          this.campEvents.splice(0, 0, result.body);
          this.currentEvent = result.body;
          this.selectedModelMoniker = result.data.moniker;
          this.newEventMoniker = "";
        }.bind(this), function () {
          this.errorMessage = "Failed to save new event";
        }.bind(this))
      }
    },
    mounted() {
      CodeCamp.Common.dataService.getAllEvents()
        .then(function (result) {
          this.campEvents = result.data;
          this.currentEvent = _.first(this.campEvents);
          this.selectedModelMoniker = this.currentEvent.moniker
        }.bind(this), function () {
          this.errorMessage = "Failed to get event data";
        }.bind(this));
    }



  };
}

