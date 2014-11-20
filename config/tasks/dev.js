module.exports = function(gulp) {
  return gulp.task("dev", ["scripts", "sass", "icons", "jekyll", "server", "watch"]);
};
