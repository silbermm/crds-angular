exports.config = {
  seleniumServerJar: '../node_modules/protractor/selenium/selenium-server-standalone-2.42.2.jar',
  allScriptsTimeout: 20000,
  specs: [
    '../spec/integration/**/*_spec.js',
  ],
  capabilities: {
    'browserName': 'firefox',
  },
  jasmineNodeOpts: {
    showColors: true,
    defaultTimeoutInterval: 30000
  },
  onPrepare: function() {
    require('../spec/integration/helper');
  }
};
