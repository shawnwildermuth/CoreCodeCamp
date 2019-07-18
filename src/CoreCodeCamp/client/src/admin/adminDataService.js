import Vue from "vue";

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
    talk.room = value;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/room", talk);
  }

  updateTalkTime(talk, value) {
    talk.time = value;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/time", talk);
  }

  updateTalkTrack(talk, value) {
    talk.track = value;
    return this.http.put(this.baseUrl() + "talks/" + talk.id + "/track", talk);
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
    return this.http.get(this.baseUrl() + "timeSlots");
  }

  saveTimeSlot(timeSlot) {
    return this.http.post(this.baseUrl() + "timeSlots", { time: timeSlot });
  }

  updateTimeSlot(timeSlot, value) {
    return this.http.put(this.baseUrl() + `timeSlots/${timeSlot.id}`, { id: timeSlot.id, time: value });
  }

  deleteTimeSlot(timeSlot) {
    return this.http.delete(this.baseUrl() + "timeSlots/" + timeSlot.id);
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
