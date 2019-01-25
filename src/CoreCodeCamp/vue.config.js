
// vue.config.js
module.exports = {
  pluginOptions: {
    sourceDir: "Client"
  },
  outputDir: "wwwroot/js/app/",
  filenameHashing: false,
  runtimeCompiler: true,
  pages: {
    join: "Client/join.js",
    admin: "Client/admin.js"
  }
}