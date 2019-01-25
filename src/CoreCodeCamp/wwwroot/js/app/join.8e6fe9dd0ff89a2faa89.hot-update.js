webpackHotUpdate("join",{

/***/ "./node_modules/cache-loader/dist/cjs.js?!./node_modules/babel-loader/lib/index.js!./node_modules/cache-loader/dist/cjs.js?!./node_modules/vue-loader/lib/index.js?!./wwwroot/app/components/join.vue?vue&type=script&lang=js&":
false,

/***/ "./node_modules/cache-loader/dist/cjs.js?{\"cacheDirectory\":\"node_modules/.cache/vue-loader\",\"cacheIdentifier\":\"30ca27a8-vue-loader-template\"}!./node_modules/vue-loader/lib/loaders/templateLoader.js?!./node_modules/cache-loader/dist/cjs.js?!./node_modules/vue-loader/lib/index.js?!./wwwroot/app/components/join.vue?vue&type=template&id=03b8c63e&":
false,

/***/ "./node_modules/vue-hot-reload-api/dist/index.js":
false,

/***/ "./node_modules/vue-loader/lib/runtime/componentNormalizer.js":
false,

/***/ "./wwwroot/app/common/helpers.js":
false,

/***/ "./wwwroot/app/components/join.vue":
false,

/***/ "./wwwroot/app/components/join.vue?vue&type=script&lang=js&":
false,

/***/ "./wwwroot/app/components/join.vue?vue&type=template&id=03b8c63e&":
false,

/***/ "./wwwroot/app/join.js":
/*!*****************************!*\
  !*** ./wwwroot/app/join.js ***!
  \*****************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.esm.js\");\n/* harmony import */ var _common_bootstrap__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./common/bootstrap */ \"./wwwroot/app/common/bootstrap.js\");\n!(function webpackMissingModule() { var e = new Error(\"Cannot find module '../common/helpers'\"); e.code = 'MODULE_NOT_FOUND'; throw e; }());\n\n\n\nlet theView = {\n  data: {\n    user: {\n      name: \"\",\n      email: \"\",\n      password: \"\",\n      confirmPassword: \"\"\n    },\n    errorMessage: \"\"\n  },\n  computed: {\n    isPristine: function () {\n      return !(function webpackMissingModule() { var e = new Error(\"Cannot find module '../common/helpers'\"); e.code = 'MODULE_NOT_FOUND'; throw e; }()).isPristine(this.fields);\n    }\n  },\n  methods: {\n    onSubmit() {\n      let me = this;\n      this.$validator.validateAll().then(function (success) {\n        if (!success) {\n          me.errorMessage = \"Please fix validation Issues\";\n          return false;\n        }\n      });\n    },\n\n    created() {\n      this.$set(this, 'errors', this.$validator.errorBag);\n    }\n\n  }\n};\nObject(_common_bootstrap__WEBPACK_IMPORTED_MODULE_1__[\"default\"])(theView, \"#view-join\");//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi93d3dyb290L2FwcC9qb2luLmpzLmpzIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vLy4vd3d3cm9vdC9hcHAvam9pbi5qcz81NmIxIl0sInNvdXJjZXNDb250ZW50IjpbImltcG9ydCBWdWUgZnJvbSAndnVlJztcbmltcG9ydCBib290c3RyYXAgZnJvbSBcIi4vY29tbW9uL2Jvb3RzdHJhcFwiO1xuaW1wb3J0IGhlbHBlcnMgZnJvbSBcIi4uL2NvbW1vbi9oZWxwZXJzXCI7XHJcblxubGV0IHRoZVZpZXcgPSB7XHJcbiAgZGF0YToge1xyXG4gICAgdXNlcjoge1xyXG4gICAgICBuYW1lOiBcIlwiLFxyXG4gICAgICBlbWFpbDogXCJcIixcclxuICAgICAgcGFzc3dvcmQ6IFwiXCIsXHJcbiAgICAgIGNvbmZpcm1QYXNzd29yZDogXCJcIlxyXG4gICAgfSxcclxuICAgIGVycm9yTWVzc2FnZTogXCJcIlxyXG4gIH0sXHJcbiAgY29tcHV0ZWQ6IHtcclxuICAgIGlzUHJpc3RpbmU6IGZ1bmN0aW9uICgpIHtcclxuICAgICAgcmV0dXJuIGhlbHBlcnMuaXNQcmlzdGluZSh0aGlzLmZpZWxkcyk7XHJcbiAgICB9XHJcbiAgfSxcclxuICBtZXRob2RzOiB7XHJcbiAgICBvblN1Ym1pdCgpIHtcclxuICAgICAgbGV0IG1lID0gdGhpcztcclxuICAgICAgdGhpcy4kdmFsaWRhdG9yLnZhbGlkYXRlQWxsKCkudGhlbihmdW5jdGlvbiAoc3VjY2Vzcykge1xyXG4gICAgICAgIGlmICghc3VjY2Vzcykge1xyXG4gICAgICAgICAgbWUuZXJyb3JNZXNzYWdlID0gXCJQbGVhc2UgZml4IHZhbGlkYXRpb24gSXNzdWVzXCI7XHJcbiAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgICB9KTtcclxuICAgIH0sXHJcbiAgICBjcmVhdGVkKCkge1xyXG4gICAgICB0aGlzLiRzZXQodGhpcywgJ2Vycm9ycycsIHRoaXMuJHZhbGlkYXRvci5lcnJvckJhZylcclxuICAgIH1cclxuXHJcbiAgfVxyXG5cclxufTtcclxuXG5cblxuYm9vdHN0cmFwKHRoZVZpZXcsIFwiI3ZpZXctam9pblwiKTsgIl0sIm1hcHBpbmdzIjoiQUFBQTtBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQ0E7QUFDQTtBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBSkE7QUFNQTtBQVBBO0FBU0E7QUFDQTtBQUNBO0FBQ0E7QUFIQTtBQUtBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFBQTtBQUNBO0FBQ0E7QUFDQTtBQWJBO0FBZkE7QUFtQ0EiLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./wwwroot/app/join.js\n");

/***/ })

})