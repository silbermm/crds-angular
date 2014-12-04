var karma = require('karma').server,
    _ = require('lodash'),
    karmaConf = require('../karma.conf');

module.exports = function(gulp) {
  return gulp.task("karma", ["scripts"], function(done) {
    karma.start(_.assign({}, karmaConf, {
      singleRun: true
    }), done);
  });
};
