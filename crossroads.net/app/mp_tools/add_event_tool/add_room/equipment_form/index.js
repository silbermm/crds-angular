(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  angular.module(MODULE)
    .directive('equipmentForm', require('./equipmentForm.component'))
    .directive('uniqueEquipment', require('./uniqueEquipment.directive'))
  ;

  require('./equipmentForm.html');
})();
