// usersForm.ts
import { Component } from '@angular/core';
import { FormBuilder, Validators, Control, ControlGroup } from '@angular/common';
import { UserService } from "./userService";

@Component({
  selector: "users-form",
  moduleId: module.id, // To make urls become relative!
  templateUrl: "usersForm.html"
})
export class UsersForm {

  users: Array<any>;
  isBusy: boolean = false;

  constructor(private userService: UserService) {
    this.loadUsers();
  }

  loadUsers() {
    this.isBusy = true;
    this.userService
      .getUsers()
      .subscribe(
      res => this.users = res.json(),
      res => console.log("Failed to get users"),
      () => this.isBusy = false);
  }

  public onToggleAdmin(user: any) {
    this.isBusy = true;
    this.userService
      .toggleUser(user)
      .subscribe(
      res => {
        let result = res.json();
        user.isAdmin = result;
      },
      res => console.log("Failed to toggle users."),
      () => this.isBusy = false);
  }

}