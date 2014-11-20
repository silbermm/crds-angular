module.exports = function(gulp) {
  return gulp.task('server_test', ['mocha', 'server_watch']);
};
