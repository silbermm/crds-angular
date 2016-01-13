(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  angular.module(MODULE)
    .directive('equipmentForm', require('./equipmentForm.component'))
  ;

  require('./equipmentForm.html');
})();
