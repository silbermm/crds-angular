(function() {
  'use strict';

  var constants = require('../constants');

  angular.module(constants.MODULES.COMMON, []);

  // require all giving common components
  require('./giving');
})();
