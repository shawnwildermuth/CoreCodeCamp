import Vue from "vue";
import _ from "lodash";

class AdminDataService {

  constructor(moniker = window.location.pathname.split('/')[1]) {
    this._moniker = moniker;
  }

  // Utilities
  baseUrl(moniker = this.moniker) {
    return '/' + moniker + "/api/";
  }

  get moniker() {
    return this._moniker;
  }

  get http () {
    return Vue.http;
  }

  // Events
  getAllEvents() {
    return this.http.get("/api/events/");
  }

  getEventInfo() {
    return this.http.get("/api/events/" + this.moniker);
  }

  saveEventInfo(eventInfo) {
    return this.http.put("/api/events/" + this.moniker, eventInfo);
  }

  saveEventLocation(location) {
    return this.http.put("/api/events/" + this.moniker + "/location", location);
  }

  addEventInfo(moniker, eventInfo) {
    return this.http.post("/api/events/" + eventInfo.moniker, eventInfo);
  }

  // Sponsors
  getSponsors() {
    return this.http.get(this.baseUrl() + "sponsors");
  }

  saveSponsor(sponsor) {
    return this.http.post(this.baseUrl() + "sponsors", sponsor);
  }

  deleteSponsor(sponsor) {
    return this.http.delete(this.baseUrl() + "sponsors/" + sponsor.id);
  }

  togglePaid(sponsor) {
    return this.http.put(this.baseUrl() + "sponsors/" + sponsor.id + "/togglePaid/", null);
  }

  // Speakers
  getMySpeaker() {
    return this.http.get(this.baseUrl() + "speakers/me");
  }

  saveSpeaker(speaker) {
    return this.http.post(this.baseUrl() + "speakers/me", speaker);
  }

  // Talks
  getTalks() {
    return this.http.get(this.baseUrl() + "talks/me");
  }

  getAllTalks() {
    return this.http.get(this.baseUrl() + "talks");
  }

  saveTalk(talk) {
    return this.http.post(this.baseUrl() + "speakers/me/talks", talk);
  }

  deleteTalk(id) {
    return this.http.delete(this.baseUrl() + "talks/" + id);
  }

  toggleApproved(talk) {
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/toggleApproved", talk);
  }

  updateRoomName(room, name) {
    return this.http.put(this.baseUrl() + `rooms/${room.id}`, { id: room.id, name: name});
  }

  updateTalkRoom(talk, value) {
    let update = _.cloneDeep(talk);
    update.room = value.name;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/room", update);
  }

  updateTalkTime(talk, value) {
    let update = _.cloneDeep(talk);
    update.time = value.time;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/time", update);
  }

  updateTalkTrack(talk, value) {
    talk.track = value;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/track", talk);
  }

  unassignTalk(talk) {
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/unassign");
  }

  // Users
  getUsers() {
    return this.http.get("/api/users");
  }

  toggleAdmin(user) {
    return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleAdmin", user);
  }

  toggleConfirmation(user) {
    return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleconfirmation", user);
  }

  // Time Slots
  getTimeSlots() {
    return this.http.get(this.baseUrl() + "timeslots");
  }

  saveTimeSlot(timeslot) {
    return this.http.post(this.baseUrl() + "timeslots", { time: timeslot });
  }

  updateTimeSlot(timeslot, value) {
    return this.http.put(this.baseUrl() + `timeslots/${timeslot.id}`, { id: timeslot.id, time: value });
  }

  deleteTimeSlot(timeslot) {
    return this.http.delete(this.baseUrl() + "timeslots/" + timeslot.id);
  }

  // Rooms
  getRooms() {
    return this.http.get(this.baseUrl() + "rooms");
  }

  saveRoom(name) {
    return this.http.post(this.baseUrl() + "rooms", { name });
  }

  deleteRoom(room) {
    return this.http.delete(this.baseUrl() + "rooms/" + room.id);
  }

  // Tracks
  getTracks() {
    return this.http.get(this.baseUrl() + "tracks");
  }

  saveTrack(track) {
    return this.http.post(this.baseUrl() + "tracks", { name: track });
  }

  deleteTrack(track) {
    return this.http.delete(this.baseUrl() + "tracks/" + track.id);
  }

  formatError(err) {
    let msg = "";
    if (!err.body) msg = "Unknown Error";
    else {
      for (var key in err.body) {
        let item = err.body[key];
        msg += "<br/>" + key + ":" + item[0];
      }
    }

    return msg;
  }
}

export default AdminDataService;
