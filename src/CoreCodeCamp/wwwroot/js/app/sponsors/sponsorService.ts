// SponsorService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';

@Injectable()
export class SponsorService {

  constructor(private http: Http) {}

  public getEvents() {
    return this.http.get("/api/events");
  }

  public getSponsors(moniker: string) {
    return this.http.get("/api/sponsors/" + moniker);
  }


}