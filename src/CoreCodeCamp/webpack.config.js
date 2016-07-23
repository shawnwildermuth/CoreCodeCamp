var webpack = require('webpack');
var path = require('path');

module.exports = {
  context: path.resolve("./wwwroot/js/app/"),
  entry: {
    speaker: "./speaker/main.ts",
    sponsors: "./sponsors/main.ts",
    talks: "./talks/main.ts",
    users: "./users/main.ts"
  },
  output: {
    path: './wwwroot/lib/site/',
    filename: "[name].main.min.js"
  },
  module: {
    loaders: [
      {
        test: /\.ts$/,
        loaders: ['ts', 'angular2-template-loader']
      },
       {
         test: /\.html$/,
         loader: 'html'

       },
    ]
  },
  resolve: {
    extensions: ["", ".js", ".ts"],
  },
  plugins: [
    new webpack.optimize.UglifyJsPlugin({
      compress: {
        warnings: false
      },
      mangle: false
    })
  ]
};