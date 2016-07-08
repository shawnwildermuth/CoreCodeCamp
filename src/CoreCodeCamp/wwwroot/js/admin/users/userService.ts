// userService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';

@Injectable()
export class UserService {
  _http: Http;

  constructor(http: Http) {
    this._http = http;
  }

  public getUsers() {

    return this._http.get("/api/users");
  }

  public toggleUser(userName: string) {

    let headers = new Headers();
    headers.append("Content-Type", "application/json");

    return this._http.put("/api/users/toggleAdmin", userName, { headers: headers });

  }
}