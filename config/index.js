var args = require('yargs').argv,
    gulp = require('gulp'),
    $ = require('gulp-load-plugins')(),
    opts = {
      dev: process.env.NODE_ENV !== 'production',
      n: args.n,
      sync: args.sync,
      burp: args.burp
    };

require('./tasks/scripts')(gulp, opts, $);
