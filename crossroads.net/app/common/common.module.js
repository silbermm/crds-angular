(function() {
  'use strict';

  var constants = require('../constants');

  angular.module(constants.MODULES.COMMON, []);

  // require the validation service
  require('./validation');

  // require all giving common components
  require('./giving');

  // require all profile common components
  require('./profile');

  require('./community_groups');

})();
