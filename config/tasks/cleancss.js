var del = require('del');

module.exports = function(gulp) {
  return gulp.task('clean:css', function(cb) {
    del(["generated/css/**/*", ".tmp/css/**/*"], cb);
  });
};
