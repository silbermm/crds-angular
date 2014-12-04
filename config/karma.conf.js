module.exports = {
  preprocessors: {
    "app/js/**/app/templates/**/*.html": ["ng-html2js"]
  },
  ngHtml2JsPreprocessor: {
    stripPrefix: "app"
  },
  browsers: ["PhantomJS"],
  frameworks: ["jasmine"],
  files: [
    "spec/helpers/*.js",
    "vendor/js/jquery.js",
    "vendor/js/underscore.js",
    "vendor/bower/angular/angular.min.js",
    "vendor/bower/angular-bootstrap/ui-bootstrap.min.js",
    "vendor/bower/angular-bootstrap/ui-bootstrap-tpls.min.js",
    "vendor/bower/angular-growl-v2/build/angular-growl.js",
    "vendor/bower/angular-cookies/angular-cookies.min.js",
    "vendor/bower/angular-mocks/angular-mocks.js",
    "vendor/bower/ngstorage/ngStorage.js",
    "vendor/bower/crds-angular-ajax-form/dist/crds-angular-ajax-form.min.js",
    "vendor/bower/crds-ng-security-context/dist/crds-ng-security-context.js",
    "vendor/bower/angular-think-ministry/angular-think-ministry.dist.js",
    ".tmp/js/app.js",
    "app/js/**/spec/**/*.js",
    "app/js/**/app/templates/**/*.html",
    "vendor/bower/angular-fitvids/angular-fitvids.js",
    "vendor/bower/plangular/plangular.js",
    "vendor/bower/angular-scroll/angular-scroll.min.js",
    "vendor/bower/angular-ui-utils/scrollfix.min.js"
  ]
};
