"use strict";
// main.ts
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var usersForm_1 = require('./usersForm');
var http_1 = require('@angular/http');
var dataService_1 = require("../common/dataService");
platform_browser_dynamic_1.bootstrap(usersForm_1.UsersForm, [http_1.HTTP_PROVIDERS, dataService_1.DataService]);
//# sourceMappingURL=main.js.map