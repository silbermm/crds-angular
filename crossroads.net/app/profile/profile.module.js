(function() {
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.PROFILE, [MODULES.CORE, MODULES.COMMON])
    .config(require('./profile.routes'))
    .controller('ProfileController', require('./profile.controller'))
    ;

  require('./communications');
  require('./services');
  require('./skills');
  require('./giving');

  require('./profile.html');
  require('./personal/profilePersonal.html');
})();
