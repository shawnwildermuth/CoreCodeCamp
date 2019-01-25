
// vue.config.js
module.exports = {
  pluginOptions: {
    sourceDir: "wwwroot/app/"
  },
  outputDir: "wwwroot/js/app/",
  filenameHashing: false,
  runtimeCompiler: true,
  pages: {
    join: "wwwroot/app/join.js"
  }
}