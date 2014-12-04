module.exports = function(gulp) {
  return gulp.task("test", ["sass", "karma", "protractor", "spec-watch"]);
};
