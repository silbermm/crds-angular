module.exports = function(gulp) {
  return gulp.task('copy', ['html'], function() {
    gulp.src('app/_includes/css/*.css')
      .pipe(gulp.dest('generated/css/'));
    gulp.src('app/_includes/js/*.js')
      .pipe(gulp.dest('generated/js/'));
  });
};
