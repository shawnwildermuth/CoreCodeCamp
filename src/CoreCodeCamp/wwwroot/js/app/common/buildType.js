"use strict";
var core_1 = require("@angular/core");
function buildType() {
    if (this.process && this.process.env.ASPNETCORE_ENVIRONMENT !== "Development") {
        core_1.enableProdMode();
        console.log("Enabling Production Mode");
    }
}
exports.buildType = buildType;
;
//# sourceMappingURL=buildType.js.map