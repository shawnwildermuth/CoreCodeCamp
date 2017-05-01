module CodeCamp.Common {

  export class DataService {

    constructor(private http) {

    }

    // Utilities
    private baseUrl(moniker: string = this.moniker) {
      return '/' + moniker + "/api/";
    }

    public get moniker() {
      return window.location.pathname.split('/')[1];
    }

    // Events
    public getAllEvents() {
      return this.http.get("/api/events/");
    }

    public getEventInfo() {
      return this.http.get("/api/events/" + this.moniker);
    }

    public saveEventInfo(eventInfo: any) {
      return this.http.put("/api/events/" + this.moniker, eventInfo);
    }

    public saveEventLocation(location: any) {
      return this.http.put("/api/events/" + this.moniker + "/location", location);
    }

    public addEventInfo(moniker: any) {
      return this.http.post("/api/events/" + moniker, { moniker: moniker});
    }

    // Sponsors
    public getSponsors() {
      return this.http.get(this.baseUrl() + "sponsors");
    }

    public saveSponsor(sponsor: any) {
      return this.http.post(this.baseUrl() + "sponsors", sponsor);
    }

    public deleteSponsor(sponsor: any) {
      return this.http.delete(this.baseUrl() + "sponsors/" + sponsor.id);
    }

    public togglePaid(sponsor: any) {
      return this.http.put(this.baseUrl() + "sponsors/" + sponsor.id + "/togglePaid/", null);
    }

    // Speakers
    public getMySpeaker() {
      return this.http.get(this.baseUrl() + "speakers/me");
    }

    public saveSpeaker(speaker: any) {
      return this.http.post(this.baseUrl() + "speakers/me", speaker);
    }

    // Talks
    public getTalks() {
      return this.http.get(this.baseUrl() + "talks/me");
    }

    public getAllTalks() {
      return this.http.get(this.baseUrl() + "talks");
    }

    public saveTalk(talk: any) {
      return this.http.post(this.baseUrl() + "speakers/me/talks", talk);
    }

    public deleteTalk(id: Number) {
      return this.http.delete(this.baseUrl() + "talks/" + id);
    }

    public toggleApproved(talk: any) {
      return this.http.put(this.baseUrl() + "talks/" + talk.id + "/toggleApproved", talk);
    }

    public updateTalkRoom(talk: any, value: any) {
      return this.http.put(this.baseUrl() + "talks/" + talk.id + "/room", { room: value });
    }

    public updateTalkTime(talk: any, value: any) {
      return this.http.put(this.baseUrl() + "talks/" + talk.id + "/time", { time: value });
    }

    public updateTalkTrack(talk: any, value: any) {
      return this.http.put(this.baseUrl() + "talks/" + talk.id + "/track", { track: value });
    }

    // Users
    public getUsers() {
      return this.http.get("/api/users");
    }

    public toggleAdmin(user: any) {
      return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleAdmin", user);
    }

    public toggleConfirmation(user: any) {
      return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleconfirmation", user);
    }

    // Time Slots
    public getTimeSlots() {
      return this.http.get(this.baseUrl() + "timeSlots");
    }

    public saveTimeSlot(timeSlot) {
      return this.http.post(this.baseUrl() + "timeSlots", { time: timeSlot });
    }

    public deleteTimeSlot(timeSlot) {
      return this.http.delete(this.baseUrl() + "timeSlots/" + timeSlot.id);
    }

    // Rooms
    public getRooms() {
      return this.http.get(this.baseUrl() + "rooms");
    }

    public saveRoom(room) {
      return this.http.post(this.baseUrl() + "rooms", { name: room });
    }

    public deleteRoom(room) {
      return this.http.delete(this.baseUrl() + "rooms/" + room.id);
    }

    // Tracks
    public getTracks() {
      return this.http.get(this.baseUrl() + "tracks");
    }

    public saveTrack(track) {
      return this.http.post(this.baseUrl() + "tracks", { name: track });
    }

    public deleteTrack(track) {
      return this.http.delete(this.baseUrl() + "tracks/" + track.id);
    }

  }
}