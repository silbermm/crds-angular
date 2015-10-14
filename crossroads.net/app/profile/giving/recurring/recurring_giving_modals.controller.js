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
    vm.donation = donation;
    vm.cancel = cancel;
    vm.remove = remove;
    vm.edit = edit;

    activate($filter);

    function activate(filter) {
      vm.dto.amount = vm.donation.amount;
      vm.dto.amountSubmitted = false;
      vm.dto.bankinfoSubmitted = false;
      vm.dto.changeAccountInfo = true;
      vm.dto.brand = '#'+vm.donation.source.icon;
      vm.dto.ccNumberClass = vm.donation.source.icon;
      vm.dto.donor = {
        id: donation.donor_id,
      };
      vm.dto.givingType = vm.donation.interval;
      vm.dto.initialized = true;
      vm.dto.last4 = vm.donation.source.last4;
      vm.dto.program = filter('filter')(vm.programsInput, {ProgramId: vm.donation.program})[0];
      vm.dto.recurringStartDate = vm.donation.start_date;
      vm.dto.view = vm.donation.source.type === 'CreditCard' ? 'cc' : 'bank';

      if (vm.donation.interval !== null) {
        vm.dto.interval = _.capitalize(vm.donation.interval.toLowerCase()) + 'ly';
      }
    }

    function cancel() {
      $modalInstance.dismiss("cancel");
    }

    function remove() {
      RecurringGivingService.recurringGifts.delete({ recurringGiftId: vm.donation.recurring_gift_id }, function(){
        $modalInstance.close(true);
      }, function(/*error*/) {
        $modalInstance.close(false);
      });
    }

    function edit() {
      $modalInstance.close(vm.donation, true);
    }

  };

})();
