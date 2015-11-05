(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.CHILDCARE;

  angular.module(MODULE, [])
    .config(require('./childcare.routes'))
    .directive('childCare', require('./childcare.directive'))
    ;

  require('./childcare.html');
})();
