(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  angular.module(MODULE)
    .directive('room', require('./room.component'));

  require('./room.html');
})();
