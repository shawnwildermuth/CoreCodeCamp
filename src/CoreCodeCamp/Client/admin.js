import Vue from "vue";
import _ from "lodash";
import bootstrap from "./common/bootstrap";
import dataService from "./common/dataService";

let theView = {
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
      dataService.addEventInfo(this.newEventMoniker).then(function (result) {
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
    dataService.getAllEvents()
      .then((result) => {
        this.campEvents = result.data;
        this.currentEvent = _.first(this.campEvents);
        this.selectedModelMoniker = this.currentEvent.moniker
      }, () => {
        this.errorMessage = "Failed to get event data";
      });
  }



};


bootstrap(theView, "#events-view"); 