// userService.ts
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';

@Injectable()
export class UserService {

  constructor(private http: Http) {}

  public getUsers() {
    return this.http.get("/api/users");
  }

  public toggleUser(userName: string) {

    let headers = new Headers();
    headers.append("Content-Type", "application/json");

    return this.http.put("/api/users/toggleAdmin", userName, { headers: headers });

  }
}