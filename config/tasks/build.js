module.exports = function(gulp) {
  return gulp.task("build", ["icons", "imagemin", "jb"]);
};
