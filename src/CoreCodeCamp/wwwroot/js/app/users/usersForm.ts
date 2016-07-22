// usersForm.ts
import { Component } from '@angular/core';
import { DataService } from "../common/dataService";

@Component({
  selector: "users-form",
  templateUrl: "/js/app/users/usersForm.html"
})
export class UsersForm {

  users: Array<any>;
  isBusy: boolean = false;
  error: string = "";

  constructor(private data: DataService) {
    this.loadUsers();
  }

  loadUsers() {
    this.isBusy = true;
    this.data.getUsers()
      .subscribe(
      res => {
        this.users = res.json();
        this.isBusy = false;
      },
      res => {
        this.error = "Failed to get users";
        this.isBusy = false;
      });
  }

  onToggleAdmin(user: any) {
    this.isBusy = true;
    this.data.toggleAdmin(user)
      .subscribe(
      res => {
        let result = res.json();
        user.isAdmin = result;
        this.isBusy = false;
      },
      res => {
        this.error = "Failed to toggle users.";
        this.isBusy = false;
      });
  }

}