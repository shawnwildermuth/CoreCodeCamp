// dataService.ts
import { Injectable } from "@angular/core";
import { Http } from "@angular/http";

@Injectable()
export class DataService {

  constructor(private http: Http) {
  }

  // Utilities
  private baseUrl(moniker: string = this.moniker) {
    return '/' + moniker + "/api/";
  }

  public get moniker() {
    return window.location.pathname.split('/')[1];
  }

  // Events
  public getEvents() {
    return this.http.get("/api/events");
  }

  // Sponsors
  public getSponsors(moniker: string) {
    return this.http.get(this.baseUrl(moniker) + "sponsors");
  }

  public saveSponsor(moniker: string, sponsor: any) {
    return this.http.post(this.baseUrl(moniker) + "sponsors", sponsor);
  }

  public deleteSponsor(moniker: string, sponsor: any) {
    return this.http.delete(this.baseUrl(moniker) + "sponsors/" + sponsor.id);
  }

  public togglePaid(moniker: string, sponsor: any) {
    return this.http.put(this.baseUrl(moniker) + "sponsors/" + sponsor.id + "/togglePaid/", null);
  }

  // Speakers
  public getMySpeaker() {
    return this.http.get(this.baseUrl() + "speakers/me");
  }

  public saveSpeaker(speaker: any) {
    return this.http.post(this.baseUrl() + "speakers", speaker);
  }

  // Talks
  public getTalks() {
    return this.http.get(this.baseUrl() + "talks/me"); 
  }

  public saveTalk(talk: any) {
    return this.http.post(this.baseUrl() + "speakers/me/talks", talk);
  }

  public deleteTalk(id: Number) {
    return this.http.delete(this.baseUrl() + "talks/" + id);
  }

  // Users
  public getUsers() {
    return this.http.get("/api/users");
  }

  public toggleAdmin(user: any) {
    return this.http.put("/api/users/" + encodeURIComponent(user.userName) + "/toggleAdmin", user);
  }
}