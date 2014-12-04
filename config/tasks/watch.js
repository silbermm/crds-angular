var paths = require('../paths');

module.exports = function(gulp) {
  return gulp.task("watch", function() {
    gulp.watch([paths.scripts, paths.templates], ["scripts"]);
    gulp.watch(paths.sass, ["sass"]);
  });
};
