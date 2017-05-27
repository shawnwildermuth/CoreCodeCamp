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
      events: [],
      errorMessage: "",
      currentEvent: null,
      newEventMoniker: "",
      selectedModelMoniker: ""
    },
    methods: {
      onEventChanged(moniker) {
        this.currentEvent = _.find(this.events, e => e.moniker === moniker);
      },

      onAddEvent() {
        CodeCamp.Common.dataService.addEventInfo(this.newEventMoniker).then(function (result) {
          this.events.splice(0, 0, result.data);
          this.currentEvent = result.data;
          this.selectedModelMoniker = result.data.moniker;
          this.newEventMoniker = "";
        }, function () {
          this.errorMessage = "Failed to save new event";
        })
      }
    },
    mounted() {
      CodeCamp.Common.dataService.getAllEvents().then(function (result) {
        this.events = result.data;
        this.currentEvent = _.first(this.events);
        this.selectedModelMoniker = this.currentEvent.moniker
      }, function () {
        this.errorMessage = "Failed to get event data";
      });
    }



  };
}

