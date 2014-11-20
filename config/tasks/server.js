var browserSync = require('browser-sync');

module.exports = function(gulp, opts, $) {
  return gulp.task("server", function() {
    $.nodemon({
      script: "./server/server.js",
      env: opts.sync ? {
        'PORT': 8000
      } : {
        'PORT': 3000
      },
      ignore: ["app/", "_site/", "config/", "generated/", "spec/", "tmp/", "vendor/"]
    });
    if (opts.sync) {
      browserSync.init(null, {
        proxy: "localhost:8000"
      });
    }
  });
};
