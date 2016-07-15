// SponsorService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import { Observable } from 'rxjs/observable';

@Injectable()
export class SponsorService {

  constructor(private http: Http) {}

  public getEvents() {
    return this.http.get("/api/events");
  }

  public getSponsors(moniker: string) {
    return this.http.get("/api/sponsors/" + moniker);
  }

  public saveSponsor(moniker: string, sponsor: any) {
    return this.http.post("/api/sponsors/" + moniker, sponsor);
  }

  public deleteSponsor(moniker: string, sponsor: any) {
    return this.http.delete("/api/sponsors/" + moniker + "/" + sponsor.id);
  }

  public togglePaid(moniker: string, sponsor: any) {
    return this.http.put("/api/sponsors/" + moniker + "/togglePaid/" + sponsor.id, null);
  }
}