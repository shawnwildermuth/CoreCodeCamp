///<reference path="../common/dataService.ts" />
module CodeCamp {

  // External JS Libraries
  declare var Vue: any;
  declare var _: any;

  export let AdminEventsView = {
    el: "#events-view",
    data: {
      events: [],
      errorMessage: "",
      currentEvent: null
    },
    methods: {
      onEventChanged(moniker) {
        this.currentEvent = _.find(this.events, e => e.moniker === moniker);
      }
    },
    mounted() {
      this.$dataService = new CodeCamp.Common.DataService(this.$http);

      this.$dataService.getEventInfo().then(function (result) {
        this.events = result.data;
        this.currentEvent = _.first(this.events);
      }, function () {
        this.errorMessage = "Failed to get event data";
      });
    }



  };
}

