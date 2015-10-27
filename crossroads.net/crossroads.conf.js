var webpack = require('webpack');
var path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

var definePlugin = new webpack.DefinePlugin({
    __API_ENDPOINT__: JSON.stringify(process.env.CRDS_API_ENDPOINT || 'https://gatewayint.crossroads.net/gateway/'),
    __CMS_ENDPOINT__: JSON.stringify(process.env.CRDS_CMS_ENDPOINT || 'https://contentint.crossroads.net/'),
    __STRIPE_PUBKEY__ : JSON.stringify(process.env.CRDS_STRIPE_PUBKEY || 'pk_test_TR1GulD113hGh2RgoLhFqO0M')
});


module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: './',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
      'https://js.stripe.com/v2/',
      /*'assets/dependencies.js',*/
      //'assets/core.js',
      //'assets/main.js',
      'node_modules/angular/angular.js',
      'node_modules/angular-mocks/angular-mocks.js',
      'node_modules/moment/moment.js',
      './assets/trips.js',
      './assets/search*.js',
      './assets/profile*.js',
      './assets/media*.js',
      'spec/spec_index.js'
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
      'spec/spec_index.js': ['webpack','env']
    },

    webpack: {
      externals: {
        stripe: 'Stripe',
        angular: 'angular',
        moment: 'moment'

      },
      module: {
        loaders: [
          { test: /\.css$/, loader: 'style-loader!css-loader' },
          { test: /\.js$/, include: [
              path.resolve(__dirname, 'app'),
              path.resolve(__dirname, 'node_modules/angular-stripe')
            ], loader: 'babel-loader' },
          { test: /\.scss$/, 
            loader: ExtractTextPlugin.extract('style-loader', 
                         'css-loader!autoprefixer-loader!sass-loader') 
          },
          { test: /\.(jpe?g|png|gif|svg)$/i, loaders: [
            'image?bypassOnDebug&optimizationLevel=7&interlaced=false'] 
          },
          { test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, 
            loader: 'url-loader?limit=10000&minetype=application/font-woff' 
          },
          { test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: 'file-loader' },
          { test: /\.html$/, loader: 'ng-cache?prefix=[dir]' }
        ]
      },
      plugins: [ new ExtractTextPlugin('[name].css'), definePlugin ]  
    },

    webpackMiddleware: {
      stats: {
        colors: true  
      }
    },
    
    envPreprocessor: [
      'CRDS_API_ENDPOINT',
      'CRDS_CMS_ENDPOINT',
    ],


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['html'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false,

    // Plugins
    plugins: [
      require('karma-webpack'),
      require('karma-jasmine'),
      require('karma-html-reporter'),
      require('karma-chrome-launcher'),
      require('karma-env-preprocessor')
    ]
  });
};
