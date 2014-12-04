var del = require('del');

module.exports = function(gulp) {
  return gulp.task('clean:js', function(cb) {
    del(["generated/js/**/*", ".tmp/js/**/*"], cb);
  });
};
