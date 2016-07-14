"use strict";
// routes.ts
var router_1 = require('@angular/router');
var talksForm_1 = require("./talksForm");
var talkEditor_1 = require("./talkEditor");
exports.talkRoutes = [
    router_1.provideRouter([
        {
            path: '',
            redirectTo: 'speaker',
            pathMatch: 'full'
        },
        { path: 'edit/:id', component: talkEditor_1.TalkEditor },
        { path: 'speaker', component: talksForm_1.TalksForm }
    ])
];
//# sourceMappingURL=routes.js.map