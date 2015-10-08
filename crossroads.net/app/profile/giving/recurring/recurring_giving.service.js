(function() {
  'use strict';

  module.exports = RecurringGivingService;

  RecurringGivingService.$inject = ['$resource'];

  function RecurringGivingService($resource) {

    return {
      recurringGifts: $resource(__API_ENDPOINT__ + 'api/donor/recurrence')
    };

  }

})();
