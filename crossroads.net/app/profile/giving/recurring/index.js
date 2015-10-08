(function() {
  'use strict()';

  require('./templates/recurring_giving_list.html');

  var app = angular.module(constants.MODULES.PROFILE);
  app.factory('RecurringGivingService', require('./recurring_giving.service'));
  app.directive('RecurringGivingList', require('./recurring_giving_list.directive'));

})();
