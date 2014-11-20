var paths = require('../paths'),
    streamqueue = require('streamqueue');

module.exports = function(gulp, opts, $) {
  return gulp.task('scripts', [], function() {
    var stream = streamqueue({ objectMode: true });

    stream.queue(
      gulp.src(paths.scripts)
        .pipe($.util.noop())
    );

    stream.queue(
      gulp.src(paths.templates)
        .pipe($.angularTemplatecache({ standalone: true }))
    );

    return stream.done()
      .pipe(opts.dev ? gulp.dest(".tmp/js") : $.util.noop())
      .pipe($.concatSourcemap("app.js", { prefix: 2 }))
      .pipe(gulp.dest(".tmp/js"))
      .pipe(opts.dev && opts.n ? $.notify('JavaScript is done') : $.util.noop());
  });
};
