import mount from "../common/mount";
import Vue from "vue";
import dataService from "../common/dataService";

let theView = {
  data: {
    busy: true,
    talks: [],
    rooms: [],
    timeSlots: [],
    tracks: [],
    errorMessage: "",
    userMessage: "",
    sort: "",
    sortAsc: true,
    summary: {
      speakers: 0,
      approved: 0,
      talks: 0
    }
  },
  methods: {
    showError(err) {
      this.errorMessage = err;
    },
    setMsg(text) {
      this.userMessage = text;
      window.setTimeout(() => this.userMessage = "", 5000);
    },
    onTrackChanged(talk, $event) {
      this.busy = true;
      this.userMessage = "";
      this.errorMessage = "";
      let value = this.tracks[$event.target.selectedIndex];
      dataService.updateTalkTrack(talk, value.name)
        .then(() => this.setMsg("Saved..."),
          () => this.showError("Failed to update talk"))
        .finally(() => this.busy = false);
    },
    onRoomChanged(talk, $event) {
      this.busy = true;
      this.userMessage = "";
      this.errorMessage = "";
      let value = this.rooms[$event.target.selectedIndex];
      dataService.updateTalkRoom(talk, value.name)
        .then(result => {
          this.setMsg("Saved...");
        },
          e => {
            this.showError("Failed to update talk");
          })
        .finally(() => this.busy = false);
    },
    onTimeChanged(talk, $event) {
      this.busy = true;
      this.userMessage = "";
      this.errorMessage = "";
      let value = this.timeSlots[$event.target.selectedIndex];
      dataService.updateTalkTime(talk, value.time)
        .then(() => this.setMsg("Saved..."),
          () => this.showError("Failed to update talk"))
        .finally(() => this.busy = false);
    },
    onSort(sort) {
      if (sort == this.sort) {
        if (this.sortAsc) this.sortAsc = false;
        else {
          this.sort = "title"; // Reset to default sort
          this.sortAsc = true;
        }
      } else {
        this.sort = sort;
        this.sortAsc = true;
      }

      // do the sort
      this.talks = this.talks.sort((a, b) => {
        if (a[this.sort] == b[this.sort]) return 0;
        if (a[this.sort] < b[this.sort]) return this.sortAsc ? -1 : 1;
        else return this.sortAsc ? 1 : -1;
      });
    },
    updateSummary() {
      this.summary.approved = this.talks.filter(t => t.approved).length;
      this.summary.talks = this.talks.length;
      this.summary.speakers = this.talks.map(t => t.speaker.name).filter((x, i, s) => s.indexOf(x) === i).length;
    },
    onDelete(talk) {
      this.busy = true;
      dataService.deleteTalk(talk.id)
        .then(() => {
          this.talks.splice(this.talks.indexOf(talk), 1);
          this.updateSummary();
        }, e => this.showError("Failed to delete talk"))
        .finally(() => this.busy = false);
    },

    onToggleApproved(talk) {
      this.busy = true;
      dataService.toggleApproved(talk)
        .then(result => {
          talk.approved = !talk.approved;
          this.updateSummary();
        }, e => this.showError("Failed to toggle approved flag"))
        .finally(() => this.busy = false);
    }

  },
  mounted() {
    Vue.Promise.all([
      dataService.getAllTalks(),
      dataService.getRooms(),
      dataService.getTimeSlots(),
      dataService.getTracks()
    ])
      .then(result => {
        this.talks = result[0].data;
        this.rooms = result[1].data;
        this.timeSlots = result[2].data;
        this.tracks = result[3].data;
        this.updateSummary();
      },  () => {
        this.errorMessage = "Failed to load data";
      })
      .finally(() => {
        this.busy = false;
      });

  }
};

mount(theView, "#schedule-view");