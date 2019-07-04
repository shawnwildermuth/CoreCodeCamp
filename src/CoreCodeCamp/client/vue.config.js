// vue.config.js
module.exports = {
  outputDir: "../wwwroot/js/app/",
  filenameHashing: false,
  runtimeCompiler: true,
  pages: {
    join: {
      entry: "src/join.js",
      filenameHashing: false
    },
    admin: { 
      entry: "src/admin/index.js",
      filenameHashing: false
    },
    callforspeakers: {
      entry: "src/callForSpeakers/index.js",
      filenameHashing: false
    }
  }
}