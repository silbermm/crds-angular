(function() {
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  angular.module(MODULES.ONETIME_SIGNUP, [MODULES.CORE])
    .directive('onetimeEvent', require('./onetimeEvent.component'))
    ;

  require('./onetimeEvent.html');

})();
