(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./adminGivingHistory.html');

  angular.
      module(MODULE).
      controller('GivingHistoryController',
      require('../../giving_history/giving_history.controller'));

  angular.
      module(MODULE).
      controller('AdminGivingHistoryController',
      require('./adminGivingHistory.controller'));

})();