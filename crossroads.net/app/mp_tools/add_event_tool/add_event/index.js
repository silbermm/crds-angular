(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  require('./add_event.html');

  angular.module(MODULE)
  .directive('addEvent', require('./addEvent.component'))
  .directive('endDate', require('./endDate.directive'));

})();
