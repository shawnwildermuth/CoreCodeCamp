///<reference path="../common/dataService.ts" />
namespace CodeCamp {

  // External JS Libraries
  declare var Vue: any;

  export function adminSchedule() {
    CodeCamp.App.bootstrap(CodeCamp.adminScheduleView);
  }

  export let adminScheduleView = {
    el: "#schedule-view",
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
      setMsg(text: string) {
        this.userMessage = text;
        window.setTimeout(() => this.userMessage = "", 5000);
      },
      onTrackChanged(talk, $event) {
        this.busy = true;
        this.userMessage = "";
        this.errorMessage = "";
        let value = this.tracks[$event.target.selectedIndex];
        CodeCamp.Common.dataService.updateTalkTrack(talk, value.name)
          .then(() => this.setMsg("Saved..."),
            () => this.showError("Failed to update talk"))
          .finally(() => this.busy = false);
      },
      onRoomChanged(talk, $event) {
        this.busy = true;
        this.userMessage = "";
        this.errorMessage = "";
        let value = this.rooms[$event.target.selectedIndex];
        CodeCamp.Common.dataService.updateTalkRoom(talk, value.name)
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
        CodeCamp.Common.dataService.updateTalkTime(talk, value.time)
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
      onDelete(talk: any) {
        this.busy = true;
        CodeCamp.Common.dataService.deleteTalk(talk.id)
          .then(() => {
            this.talks.splice(this.talks.indexOf(talk), 1);
            this.updateSummary();
          }, e => this.showError("Failed to delete talk"))
            .finally(() => this.busy = false);
      },

      onToggleApproved(talk: any) {
        this.busy = true;
        CodeCamp.Common.dataService.toggleApproved(talk)
          .then(result => {
            talk.approved = !talk.approved;
            this.updateSummary();
          }, e => this.showError("Failed to toggle approved flag"))
            .finally(() => this.busy = false);
      }

    },
    mounted() {
      Vue.Promise.all([
        CodeCamp.Common.dataService.getAllTalks(),
        CodeCamp.Common.dataService.getRooms(),
        CodeCamp.Common.dataService.getTimeSlots(),
        CodeCamp.Common.dataService.getTracks()
      ])
        .then(function (result) {
          this.talks = result[0].data;
          this.rooms = result[1].data;
          this.timeSlots = result[2].data;
          this.tracks = result[3].data;
          this.updateSummary();
        }.bind(this), function () {
          this.errorMessage = "Failed to load data";
        }.bind(this))
        .finally(function () {
          this.busy = false;
        }.bind(this));

    }
  };
}

