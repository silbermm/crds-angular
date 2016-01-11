(function() {
  'use strict()';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  // HTML Files
  require('./events_rooms_equipment.html');

  angular.module(MODULE).controller('EventRoomEquipmentController', require('./eventRoomEquipment.controller'));

})();
