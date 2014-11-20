var paths = require('../paths');

module.exports = function(gulp, opts, $) {
  return gulp.task('lint', function() {
    return gulp.src(paths.scripts)
      .pipe($.jshint())
      .pipe($.jshint.reporter('default'))
      .pipe($.jshint.reporter('fail'))
  });
};
