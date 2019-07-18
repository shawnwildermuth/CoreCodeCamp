import _ from "lodash";

export default {
  setCamps(state, camps) {
    state.camps = camps
  },
  setCurrentCamp(state, camp) { state.currentCamp = camp; },
  setBusy(state) { state.isBusy = true; },
  clearBusy(state) { state.isBusy = false; },
  setError(state, errorState) { state.errorState = errorState; },
  setTalks(state, talks) { state.talks = talks },
  setRooms(state, rooms) { state.rooms = rooms },
  setTracks(state, tracks) { state.tracks = tracks },
  setTimeSlots(state, timeSlots) { state.timeSlots = timeSlots },
  setSummary(state, summary) { state.summary = summary },
  setSponsors(state, sponsors) { state.sponsors = sponsors },
  setUsers(state, users) { state.users = users },
  setUserAdmin(state, { user, value }) { user.isAdmin = value; },
  setUserConfirmation(state, { user, value }) { user.isEmailConfirmed = value; },
  setTalkApproved(state, { talk, value }) { talk.approved = value; },
  setTalkSort(state, value) { state.talkSort = value; },
  setSponsorIsPaid(state, { sponsor, value }) {
    sponsor.paid = value;
  },
  addSponsor(state, sponsor) { state.sponsors.push(sponsor); },
  deleteSponsor(state, sponsor) {
    var index = _.findIndex(state.sponsors, s => s.id == sponsor.id);
    if (index > -1) state.sponsors.splice(index, 1);
  },
  updateSponsor(state, sponsor) {
    var index = _.findIndex(state.sponsors, s => s.id == sponsor.id);
    if (index > -1) state.sponsors.splice(index, 1, sponsor);
  },
  addCamp(state, value) { state.camps.push(value); },
  updateCamp(state, camp) {
    var index = _.findIndex(state.camps, c => c.id == camp.id);
    if (index > -1) state.camps.splice(index, 1, camp);
  },
  addRoom(state, value) { state.rooms.push(value); },
  removeRoom(state, room) { 
    var index = _.findIndex(state.rooms, c => c.id === room.id);
    if (index > -1) state.rooms.splice(index, 1);
  },
  setRoomName(state, value) { value.room.name = value.value; },
  addTimeslot(state, value) { state.timeSlots.push(value); },
  updateTimeslot(state, value) {
    value.timeslot.time = value.value; 
  }
}