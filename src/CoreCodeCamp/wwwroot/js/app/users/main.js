"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var usersForm_1 = require('./usersForm');
var http_1 = require('@angular/http');
var userService_1 = require("./userService");
platform_browser_dynamic_1.bootstrap(usersForm_1.UsersForm, [http_1.HTTP_PROVIDERS, userService_1.UserService]);
//# sourceMappingURL=main.js.map