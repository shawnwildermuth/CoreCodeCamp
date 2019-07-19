import AdminDataService from "../adminDataService";
import _ from "lodash";

export default {
  async toggleApprove({ state, commit, dispatch }, talk) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    var res = await svc.toggleApproved(talk);
    commit("setTalkApproved", { talk, value: res.data });
    dispatch("updateSummary");
  },
  sortTalks({ state, commit }, colName) {
    let sort = {
      column: state.talkSort.column,
      ascending: state.talkSort.ascending
    };
    if (colName == state.talkSort.column) {
      // Reverse the direction of the sort
      sort.ascending = !sort.ascending;
    } else {
      sort.column = colName;
      sort.ascending = true;
    }
    commit("setTalkSort", sort);

    if (colName == "speaker") { // Special Case
      commit("setTalks", _.orderBy(state.talks, t => t.speaker.name, sort.ascending ? "asc" : "desc"));
    } else {
      commit("setTalks", _.orderBy(state.talks, sort.column, sort.ascending ? "asc" : "desc"));
    }
  }
}