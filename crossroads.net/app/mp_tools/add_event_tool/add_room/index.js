(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  // the html file
  require('./add_room.html');

  angular.module(MODULE)
    .directive('addRoom', require('./addRoom.component'));

})();
