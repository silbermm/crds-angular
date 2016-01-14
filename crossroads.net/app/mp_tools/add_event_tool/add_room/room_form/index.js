(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  angular.module(MODULE)
    .directive('roomForm', require('./roomForm.component'));

  require('./roomForm.html');
})();
