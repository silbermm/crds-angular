(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  angular.module(MODULE)
		.controller('RemoveRoomController', require('./removeRoom.controller'));

  require('./remove_room.html');
})();
