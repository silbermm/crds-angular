(function() {
  'use strict()';

  var constants = require('../../constants');

  require('./profile_giving.html');
  require('./recurring/templates/recurring_giving_list.html');

  var app = angular.module(constants.MODULES.PROFILE);
  app.controller('ProfileGivingController', require('./profile_giving.controller'));
  app.factory('RecurringGivingService', require('./recurring/recurring_giving.service'));
  app.directive('recurringGivingList', require('./recurring/recurring_giving_list.directive'));

})();
