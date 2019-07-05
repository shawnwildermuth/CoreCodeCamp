import _ from "lodash";

export default {
  setCamps(state, camps) { state.camps = camps },
  setCurrentCamp(state, camp) { state.currentCamp = camp; },
  setBusy(state) { state.isBusy = true; },
  clearBusy(state) { state.isBusy = false; },
  setTalks(state, talks) { state.talks = talks },
  setRooms(state, rooms) { state.rooms = rooms },
  setTracks(state, tracks) { state.tracks = tracks },
  setTimeSlots(state, timeSlots) { state.timeSlots = timeSlots },
  setSummary(state, summary) { state.summary = summary },
  setSponsors(state, sponsors) { state.sponsors = sponsors },
  setUsers(state, users) { state.users = users },
  setUserAdmin(state, { user, value }) { user.isAdmin = value; },
  setUserConfirmation(state, { user, value }) { user.isEmailConfirmed = value; },
  setTalkApproved(state, {talk, value}) { talk.approved = value; },
  setTalkSort(state, value) { state.talkSort = value; },
  addCamp(state, value) { state.camps.push(value); },
  updateCamp(state, camp) {
    var index = _.findIndex(state.camps, c => c.id);
    if (index > -1) state.camps.splice(index, 1, camp); 
  }
}