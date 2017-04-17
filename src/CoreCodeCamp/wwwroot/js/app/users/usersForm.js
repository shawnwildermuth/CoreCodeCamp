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
var core_1 = require("@angular/core");
var dataService_1 = require("../common/dataService");
var UsersForm = (function () {
    function UsersForm(data) {
        this.data = data;
        this.isBusy = false;
        this.error = "";
        this.loadUsers();
    }
    UsersForm.prototype.loadUsers = function () {
        var _this = this;
        this.isBusy = true;
        this.data.getUsers()
            .subscribe(function (res) {
            _this.users = res.json();
            _this.isBusy = false;
        }, function (res) {
            _this.error = "Failed to get users";
            _this.isBusy = false;
        });
    };
    UsersForm.prototype.onToggleAdmin = function (user) {
        var _this = this;
        this.isBusy = true;
        this.data.toggleAdmin(user)
            .subscribe(function (res) {
            var result = res.json();
            user.isAdmin = result;
            _this.isBusy = false;
        }, function (res) {
            _this.error = "Failed to toggle users.";
            _this.isBusy = false;
        });
    };
    return UsersForm;
}());
UsersForm = __decorate([
    core_1.Component({
        selector: "users-form",
        templateUrl: "/js/app/users/usersForm.html"
    }),
    __metadata("design:paramtypes", [dataService_1.DataService])
], UsersForm);
exports.UsersForm = UsersForm;
//# sourceMappingURL=usersForm.js.map