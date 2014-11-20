module.exports = function(gulp, $) {
  return gulp.task('mocha', function() {
    gulp.src('server/test/**/*.js', { read: false })
      .pipe($.mocha()).on('error', $.util.log);
  });
};
