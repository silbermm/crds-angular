(function() {
  'use strict()';
  var MODULE = 'crossroads.mptools';

  require('./adminRecurringGifthtml');

  angular.
      module(MODULE).
      controller('AdminGivingHistoryController',
      require('./adminGivingHistory.controller'));
})();
