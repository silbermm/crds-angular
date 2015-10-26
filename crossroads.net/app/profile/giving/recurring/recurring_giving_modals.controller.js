(function() {
  'use strict';

  module.exports = RecurringGivingModals;

  RecurringGivingModals.$inject = ['$modalInstance',
      '$rootScope',
      'DonationService',
      'GiveTransferService',
      'RecurringGiving',
      'donation',
      'programList'];

  function RecurringGivingModals($modalInstance,
                                 $rootScope,
                                 DonationService,
                                 GiveTransferService,
                                 RecurringGiving,
                                 donation,
                                 programList) {
    var vm = this;
    vm.dto = GiveTransferService;
    vm.programsInput = programList;
    vm.donation = donation;
    vm.cancel = cancel;
    vm.remove = remove;
    vm.edit = edit;

    activate();

    function activate() {
      RecurringGiving.loadDonationInformation(vm.programsInput, vm.donation);
    }

    function cancel() {
      $modalInstance.dismiss('cancel');
    }

    function remove() {
      vm.dto.processing = true;

      DonationService.deleteRecurringGift().then(function() {
        $modalInstance.close(true);
      }, function(/*error*/) {

        $modalInstance.close(false);
      });
    }

    function edit(recurringGiveForm) {
      RecurringGiving.updateGift(recurringGiveForm, successful, failure);
    }

    function successful() {
      $modalInstance.close(true);
    }

    function failure() {
      $modalInstance.close(false);
    }

  };

})();
