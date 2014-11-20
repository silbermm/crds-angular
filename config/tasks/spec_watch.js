var paths = require('../paths');

module.exports = function(gulp) {
  return gulp.task("spec-watch", function() {
    gulp.watch([paths.specs, paths.scripts, paths.templates], ["karma", "protractor"]);
  });
};
