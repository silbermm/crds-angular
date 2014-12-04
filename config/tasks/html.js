module.exports = function(gulp, $) {
  return gulp.task('html', ['scripts', 'sass'], function() {
    return gulp.src(['app/_includes/head.html', 'app/_includes/footer-scripts.html'])
      .pipe($.usemin({
        css: [$.csso(), $.rev()],
        js: [$.ngmin(), $.uglify(), $.rev()],
        html: [$.util.noop()]
      }))
      .pipe(gulp.dest('app/_includes'));
  });
};
