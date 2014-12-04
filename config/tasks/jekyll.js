var cp = require('child_process'),
    nn = require('node-notifier'),
    notifier = new nn(),
    browserSync = require('browser-sync'),
    fs = require('fs');

module.exports = function(gulp, opts, $) {
  var yamlConfigString = "_config.yml,config/_config.dev.yml";
  if (fs.existsSync("config/_config.local.yml")) {
    yamlConfigString += ",config/_config.local.yml";
  }
  if (opts.burp) {
    yamlConfigString += ",config/_config.exclude.yml";
  }
  return gulp.task("jekyll", function(cb) {
    var bundle;
    bundle = cp.spawn("bundle", ["exec", "jekyll", "build", "--config", yamlConfigString, "--watch", "--verbose"]);
    bundle.on("close", cb);
    bundle.stdout.on("data", function(data) {
      console.log("[jekyll] ", data.toString());
      if (opts.sync) {
        browserSync.reload();
      } else {
        $.util.noop();
      }
      if (opts.n && data.toString().search('done.') !== -1) {
        notifier.notify({
          title: "Gulp",
          message: 'Jekyll is done'
        });
      }
    });
  });
};
