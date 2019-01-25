
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
      filenameHashing: false
    },
    admin: { 
      entry: "Client/admin/admin.js",
      filenameHashing: false
    },
    eventInfo: {
      entry: "Client/admin/eventInfo.js",
      filenameHashing: false
    },
    users: {
      entry: "Client/admin/users.js",
      filenameHashing: false
    },
    schedule: {
      entry: "Client/admin/schedule.js",
      filenameHashing: false
    },
    sponsors: {
      entry: "Client/admin/sponsors.js",
      filenameHashing: false
    }
  }
}