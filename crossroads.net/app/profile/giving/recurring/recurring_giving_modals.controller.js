(function() {
  'use strict';

  module.exports = RecurringGivingModals;

  RecurringGivingModals.$inject = ['$modalInstance',
      '$filter',
      'RecurringGivingService',
      'GiveTransferService',
      'donation',
      'programList'];

  function RecurringGivingModals($modalInstance,
                                 $filter,
                                 RecurringGivingService,
                                 GiveTransferService,
                                 donation,
                                 programList) {
    var vm = this;
    vm.dto = GiveTransferService;
    vm.programsInput = programList;

    activate($filter);

    function activate(filter) {
      vm.dto.amount = donation.amount;
      vm.dto.amountSubmitted = false;
      vm.dto.bankinfoSubmitted = false;
      vm.dto.changeAccountInfo = true;
      vm.dto.brand = '#'+donation.source.icon;
      vm.dto.ccNumberClass = donation.source.icon;
      vm.dto.donor = {
        id: donation.donor_id,
      };
      vm.dto.givingType = donation.interval;
      vm.dto.initialized = true;
      vm.dto.last4 = donation.source.last4;
      vm.dto.program = filter('filter')(vm.programsInput, {ProgramId: donation.program})[0];
      vm.dto.recurringStartDate = donation.start_date;
    }
  };

})();
