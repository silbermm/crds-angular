(function() {
  'use strict';
  module.exports = GivingHistoryService;

  GivingHistoryService.$inject = ['$resource'];

  function GivingHistoryService($resource) {

    return {
      // api/donations/?donationYear=YYYY&softCredit=true|false
      donations: $resource(__API_ENDPOINT__ + 'api/donations/:donationYear',
        {donationYear: '@donationYear', softCredit: '@includeSoftCredits'}),
      donationYears: $resource(__API_ENDPOINT__ + 'api/donations/years'),
      impersonateDonorId: undefined
    };

  }

})();
