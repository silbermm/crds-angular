module.exports = function(gulp) {
  return gulp.task('server_watch', function() {
    gulp.watch('server/**/*', ['mocha']);
  });
};
