"use strict";
var BaseForm = (function () {
    function BaseForm() {
        this.isBusy = false;
        this.error = null;
    }
    BaseForm.prototype.showError = function (err) {
        this.error = err;
        this.isBusy = false;
    };
    return BaseForm;
}());
exports.BaseForm = BaseForm;
//# sourceMappingURL=baseForm.js.map