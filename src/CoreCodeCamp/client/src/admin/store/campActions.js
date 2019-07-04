import Vue from "vue";
import _ from "lodash";
import AdminDataService from "../adminDataService";

export default {
  loadCamps(ctx) {
    ctx.commit("setBusy");
    let dataService = new AdminDataService();
    return dataService.getAllEvents()
      .then(res => {
        ctx.commit("setCamps", res.data);
      })
      .catch(() => ctx.commit("setError", "Failed to load camps"))
      .finally(() => ctx.commit("clearBusy"));
  },
  updateSummary({ state, commit }) {
    var summary = {
      approved: state.talks.filter(t => t.approved).length,
      talks: state.talks.length,
      speakers: state.talks.map(t => t.speaker.name).filter((x, i, s) => s.indexOf(x) === i).length,
      sponsors: state.sponsors.length,
      users: state.users.length
    };
    commit("setSummary", summary);
  },
  setCampFromMoniker(ctx, moniker) {
    let camp = _.find(ctx.state.camps, c => c.moniker == moniker);
    if (camp) {
      let dataService = new AdminDataService(camp.moniker);

      this.commit("setBusy");
      Vue.Promise.all([
        dataService.getAllTalks(),
        dataService.getRooms(),
        dataService.getTimeSlots(),
        dataService.getTracks(),
        dataService.getSponsors(),
        dataService.getUsers()
      ])
        .then(result => {
          ctx.commit("setCurrentCamp", camp);
          ctx.commit("setTalks", result[0].data);
          ctx.commit("setRooms", result[1].data);
          ctx.commit("setTimeSlots", result[2].data);
          ctx.commit("setTracks", result[3].data);
          ctx.commit("setSponsors", result[4].data);
          ctx.commit("setUsers", result[5].data);
          ctx.dispatch("updateSummary");
        }, () => {
          ctx.commit("setError", "Failed to load camp data");
        })
        .finally(() => {
          this.commit("clearBusy");
        });
    }
  }
}