(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.COMMON;

  angular.module(MODULE)
    .factory('StaffContact', require('./staffContact.service'))
    .factory('Room', require('./room.service'))
    ;

})();
