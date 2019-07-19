import Vue from "vue";
import _ from "lodash";
import AdminDataService from "../adminDataService";

export default {
  loadCamps({ commit }) {
    commit("setBusy");
    commit("setError", "");
    let dataService = new AdminDataService();
    return dataService.getAllEvents()
      .then(res => {
        commit("setCamps", res.data);
      })
      .catch(() => commit("setError", "Failed to load camps"))
      .finally(() => commit("clearBusy"));
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
  setCampFromMoniker({ state, commit, dispatch }, moniker) {
 
    // Save to local storage to come back to same year every time
    console.log(`Moniker: ${moniker}`)
    if (localStorage) localStorage.setItem("moniker", moniker);

    let camp = _.find(state.camps, c => c.moniker == moniker);
    if (camp) {
      let dataService = new AdminDataService(camp.moniker);
      commit("setError", "");
      commit("setBusy");
      Vue.Promise.all([
        dataService.getAllTalks(),
        dataService.getRooms(),
        dataService.getTimeSlots(),
        dataService.getTracks(),
        dataService.getSponsors(),
        dataService.getUsers()
      ])
        .then(result => {
          commit("setCurrentCamp", camp);
          commit("setTalks", result[0].data);
          commit("setRooms", result[1].data);
          commit("setTimeSlots", result[2].data);
          commit("setTracks", result[3].data);
          commit("setSponsors", result[4].data);
          commit("setUsers", result[5].data);
          dispatch("updateSummary");
        }, () => {
          commit("setError", "Failed to load camp data");
        })
        .finally(() => {
          commit("clearBusy");
        });
    }
  },
  addCamp({ commit }, camp) {
    let svc = new AdminDataService();
    commit("setError", "");
    commit("setBusy");
    svc.addEventInfo(camp.moniker, camp)
      .then(response => {
        commit("addCamp", response.data);
        commit("setCurrentCamp", response.data);
      })
      .catch(() => commit("setError", "Failed to update camp"))
      .finally(() => commit("clearBusy"));
  },
  updateCamp({ commit }, camp) {
    let svc = new AdminDataService(camp.moniker);
    let promise = Vue.Promise((res, rej) => {
      commit("setError", "");
      commit("setBusy");
      svc.saveEventInfo(camp)
        .then(() => {
          svc.saveEventLocation(camp.location)
            .then(() => {
              commit("updateCamp", camp);
              commit("setCurrentCamp", camp);
              res();
            })
            .catch(e => rej(e));
        })
        .catch(err => {
          rej(err);
        })
        .finally(() => commit("clearBusy"));
    });

    return promise;
  },
}