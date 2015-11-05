(function() {
  'use strict';

  var MODULE = require('crds-core').MODULES.CHILDCARE;

  angular.module(MODULE, [])
    .directive('childCare', require('./childcare.directive'))
    ;

  require('./childcare.html');
})();
