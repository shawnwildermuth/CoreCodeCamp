/// <binding ProjectOpened='Watch - Development' />
var path = require('path');
var webpack = require('webpack');
var merge = require('extendify')({ isDeep: true, arrays: 'concat' });
var devConfig = require('./webpack.config.dev');
var prodConfig = require('./webpack.config.prod');
var isProduction = process.env.ASPNETCORE_ENVIRONMENT !== 'Development';
var outputDir = path.normalize(path.join(__dirname, 'wwwroot', 'lib', 'site'));

console.log("output: " + outputDir);
console.log("dirName:" + __dirname);

module.exports = merge({
  resolve: {
    extensions: ['', '.ts', '.js']
  },
  module: {
    loaders: [
        { test: /\.ts$/, include: /ClientApps/, loader: 'ts-loader?silent=true' },
        { test: /\.html$/, include: /ClientApps/, loader: 'raw-loader' }
    ]
  },
  context: path.join(__dirname, "ClientApps"),
  entry: {
    eventInfo: "./eventInfo/main.ts",
    speaker: "./speaker/main.ts",
    sponsors: "./sponsors/main.ts",
    talks: "./talks/main.ts",
    schedule: "./schedule/main.ts",
    users: "./users/main.ts"
  },
  output: {
    path: outputDir,
    filename: '[name].js',
    publicPath: '/lib/site/'
  },
  plugins: [
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require('./wwwroot/lib/site/vendor-manifest.json')
      })
  ]
}, isProduction ? prodConfig : devConfig);
