(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./templates/adminCreateRecurringGift.html');

  var app = angular.module(MODULE);
  app.controller('AdminRecurringGiftController', require('./adminRecurringGift.controller'));

})();
