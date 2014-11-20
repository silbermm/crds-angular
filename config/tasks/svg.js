var config = {
  className: ".icon-%f",
  defs: true,
  generatePreview: true
};

module.exports = function(gulp, $) {
  return gulp.task("svg", function() {
    var svg;
    svg = $.svgSprites.svg;

    return gulp.src("app/icons/*.svg")
    .pipe(svg(config))
    .pipe(gulp.dest("app/icons/generated"));

  });
};
