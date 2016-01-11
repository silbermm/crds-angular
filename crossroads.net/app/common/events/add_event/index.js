(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.COMMON;

  require('./add_event.html');

  angular.module(MODULE).directive('addEvent', require('./addEvent.component'));

})();
