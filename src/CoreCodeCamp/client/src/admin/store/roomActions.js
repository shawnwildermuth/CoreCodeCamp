import AdminDataService from "../adminDataService";
import moment from "moment";

export default {
  updateRoomName({state, commit}, value) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.updateRoomName(value.room, value.name)
      .then(res => {
        if (res.data) commit("setRoomName", { room: value.room, value: value.name });
      })
      .catch(() => commit("setError", "Failed to update Room Name"))
      .finally(() => commit("clearBusy"));

  },
  addRoom({state, commit}) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.saveRoom("Room Name")
      .then(res => {
        if (res.data) commit("addRoom", res.data);
      })
      .catch(() => commit("setError", "Failed to create Room"))
      .finally(() => commit("clearBusy"));

  },
  deleteRoom({state, commit}, room) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.deleteRoom(room)
      .then(() => {
        commit("removeRoom", room);
      })
      .catch(() => commit("setError", "Failed to delete Room"))
      .finally(() => commit("clearBusy"));
  },
  addTimeslot({state, commit}) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.saveTimeSlot(moment().set({hour: 12, minute: 0}))
      .then(res => {
        if (res.data) commit("addTimeslot", res.data);
      })
      .catch(() => commit("setError", "Failed to create Room"))
      .finally(() => commit("clearBusy"));

  },
  updateTimeslot({state, commit}, value) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.updateTimeSlot(value.timeslot, value.value)
      .then(res => {
        if (res.data) commit("updateTimeslot", value);
      })
      .catch(() => commit("setError", "Failed to update Timeslot"))
      .finally(() => commit("clearBusy"));
  },
  deleteTimeslot({state, commit}, slot) {
    let svc = new AdminDataService(state.currentCamp.moniker);
    commit("setError", "");
    commit("setBusy");
    return svc.deleteTimeslot(slot)
      .then(() => {
        commit("removeTimeslot", slot);
      })
      .catch(() => commit("setError", "Failed to delete Room"))
      .finally(() => commit("clearBusy"));
  },


}