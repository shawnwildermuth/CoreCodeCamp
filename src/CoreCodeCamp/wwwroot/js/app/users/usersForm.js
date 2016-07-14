"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
// usersForm.ts
var core_1 = require('@angular/core');
var userService_1 = require("./userService");
var UsersForm = (function () {
    function UsersForm(userService) {
        this.userService = userService;
        this.isBusy = false;
        this.loadUsers();
    }
    UsersForm.prototype.loadUsers = function () {
        var _this = this;
        this.isBusy = true;
        this.userService
            .getUsers()
            .subscribe(function (res) { return _this.users = res.json(); }, function (res) { return console.log("Failed to get users"); }, function () { return _this.isBusy = false; });
    };
    UsersForm.prototype.onToggleAdmin = function (user) {
        var _this = this;
        this.isBusy = true;
        this.userService
            .toggleUser(user)
            .subscribe(function (res) {
            var result = res.json();
            user.isAdmin = result;
        }, function (res) { return console.log("Failed to toggle users."); }, function () { return _this.isBusy = false; });
    };
    UsersForm = __decorate([
        core_1.Component({
            selector: "users-form",
            moduleId: module.id,
            templateUrl: "usersForm.html"
        }), 
        __metadata('design:paramtypes', [userService_1.UserService])
    ], UsersForm);
    return UsersForm;
}());
exports.UsersForm = UsersForm;
//# sourceMappingURL=usersForm.js.map