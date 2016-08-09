var path = require('path');
var webpack = require('webpack');
var isProduction = process.env.ASPNETCORE_ENVIRONMENT !== 'Development';
var outputDir = path.normalize(path.join(__dirname, 'wwwroot', 'lib', 'site'));

console.log("output: " + outputDir);
console.log("dirName:" + __dirname);

module.exports = {
  resolve: {
    extensions: ['', '.js']
  },
  module: {
    loaders: [
        { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' }
]
  },
  entry: {
    vendor: [
      'es6-shim',
      '@angular/common',
      '@angular/compiler',
      '@angular/core',
      '@angular/http',
      '@angular/platform-browser',
      '@angular/platform-browser-dynamic',
      '@angular/router'
    ]
  },
  output: {
    path: outputDir,
    filename: '[name].js',
    library: '[name]_[hash]',
  },
  plugins: [
      new webpack.optimize.OccurenceOrderPlugin(),
      new webpack.DllPlugin({
        path: path.join(outputDir, '[name]-manifest.json'),
        name: '[name]_[hash]'
      })
  ].concat(!isProduction ? [] : [
      new webpack.optimize.UglifyJsPlugin({
        compress: { warnings: false },
        minimize: true,
        mangle: false // Due to https://github.com/angular/angular/issues/6678
      })
  ])
};
