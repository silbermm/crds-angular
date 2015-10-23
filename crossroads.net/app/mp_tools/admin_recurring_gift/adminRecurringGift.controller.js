(function() {
  'use strict';

  module.exports = AdminRecurringGiftController;

  AdminRecurringGiftController.$inject = ['$rootScope',
      'DonationService',
      'GiveTransferService',
      'RecurringGiving',
      'donation',
      'programList'];

  function AdminRecurringGiftController($rootScope,
                                 DonationService,
                                 GiveTransferService,
                                 RecurringGiving,
                                 donation,
                                 programList) {
    var vm = this;
    vm.dto = GiveTransferService;
    vm.programsInput = programList;
    vm.donation = donation;
    vm.create = create;
    vm.deleting = false;

    activate();

    function activate() {
      RecurringGiving.loadDonationInformation(vm.programsInput, vm.donation, GiveTransferService.impersonateDonorId);
    }

    function create(recurringGiveForm) {
      RecurringGiving.createGift(recurringGiveForm, successful, failure, GiveTransferService.impersonateDonorId);
    }

    function successful() {
      if (vm.deleting) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringRemovedSuccess);
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupSuccess);
      }
    }

    function failure() {
      if (vm.deleting) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupWarning);
      }
    }
  };

})();
