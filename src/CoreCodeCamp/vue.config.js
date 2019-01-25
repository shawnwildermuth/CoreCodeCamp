
// vue.config.js
module.exports = {
  pluginOptions: {
    sourceDir: "Client"
  },
  outputDir: "wwwroot/js/app/",
  filenameHashing: false,
  runtimeCompiler: true,
  pages: {
    join: {
      entry: "Client/join.js",
      filenameHashing: false,
    },
    admin: { 
      entry: "Client/admin.js",
      filenameHashing: false,
    },
    eventInfo: {
      entry: "Client/eventInfo.js",
      filenameHashing: false,
    }
  }
}