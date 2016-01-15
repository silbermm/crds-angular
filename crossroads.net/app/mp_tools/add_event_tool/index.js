(function() {
  'use strict()';

  var MODULE = require('crds-constants').MODULES.MPTOOLS;

  // HTML Files
  require('./add_event_tool.html');

  angular.module(MODULE)
    .directive('addEventTool', require('./addEventTool.component'))
    .factory('AddEvent', require('./addEvent.service'));

  require('./add_event');

  // the addRoom component
  require('./add_room');

  // the removeRoom component
  require('./remove_room');
})();