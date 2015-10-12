(function() {
  'use strict';

  module.exports = RecurringGivingModals;

  RecurringGivingModals.$inject = ['$modalInstance',
    'RecurringGivingService',
    'GiveTransferService',
    'donation'];

  function RecurringGivingModals($modalInstance,
                                 RecurringGivingService,
                                 GiveTransferService,
                                 donation) {
    var vm = this;
    vm.dto = GiveTransferService;

    activate();

    function activate() {
      vm.dto.account = '';
      vm.dto.amount = donation.amount;
      vm.dto.amountSubmitted = true;
      vm.dto.bankinfoSubmitted = true;
      vm.dto.brand = donation.brand;
      vm.dto.donor = {};
      vm.dto.givingType = donation.interval;
      vm.dto.initialized = false;
      vm.dto.last4 = donation.source.last4;
      vm.dto.program = donation.program;
      vm.dto.recurringStartDate = donation.start_date;
    }
  };

})();
