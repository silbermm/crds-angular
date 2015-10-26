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
    vm.update = update;
    vm.deleting = false;
    vm.impersonateDonorId = undefined;

    activate();

    function activate() {
      vm.impersonateDonorId = GiveTransferService.impersonateDonorId;
      RecurringGiving.loadDonationInformation(vm.programsInput, vm.donation, vm.impersonateDonorId);
    }

    function create(recurringGiveForm) {
      RecurringGiving.createGift(recurringGiveForm, successful, failure, vm.impersonateDonorId);
    }

    function update(recurringGiveForm) {
      RecurringGiving.updateGift(recurringGiveForm, successful, failure, vm.impersonateDonorId);
    }

    function successful() {
      if (vm.deleting) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringRemovedSuccess);
        vm.dto.processing = false;
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupSuccess);
        vm.dto.processing = false;
      }
    }

    function failure() {
      if (vm.deleting) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.failedResponse);
        vm.dto.processing = false;
      } else {
        $rootScope.$emit('notify', $rootScope.MESSAGES.giveRecurringSetupWarning);
        vm.dto.processing = false;
      }
    }
  };

})();
