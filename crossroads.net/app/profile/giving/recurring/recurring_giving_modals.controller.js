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
      RecurringGiving.loadDonationInformation(vm.donation, vm.programsInput);
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
      vm.dto.processing = true;

      // Amount is not valid
      if (recurringGiveForm.donationDetailsForm !== undefined && !recurringGiveForm.donationDetailsForm.amount.$valid) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.dto.processing = false;
        return;
      }

      // Recurring Start Date was touched and is not valid - We don't want to validate if they are not updating this field
      if (recurringGiveForm.donationDetailsForm !== undefined && recurringGiveForm.donationDetailsForm.recurringStartDate.$dirty &&
          !recurringGiveForm.donationDetailsForm.recurringStartDate.$valid) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.dto.processing = false;
        return;
      }

      // Validate the credit card or bank account form
      if ((recurringGiveForm.creditCardForm !== undefined && !recurringGiveForm.creditCardForm.$valid) ||
          (recurringGiveForm.bankAccountForm !== undefined && !recurringGiveForm.bankAccountForm.$valid)) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        vm.dto.processing = false;
        return;
      }

      // Form is valid so update
      if ((recurringGiveForm.creditCardForm !== undefined && recurringGiveForm.creditCardForm.$dirty) ||
          (recurringGiveForm.bankAccountForm !== undefined && recurringGiveForm.bankAccountForm.$dirty)) {
        // Credit card or bank account info is touched so update token from strip
        DonationService.updateRecurringGift(true).then(function() {
          $modalInstance.close(true);
        }, function(/*error*/) {

          $modalInstance.close(false);
        });
      } else if (recurringGiveForm.donationDetailsForm.$dirty) {
        // Credit card or bank account info was not touched so do not update token from strip
        DonationService.updateRecurringGift(false).then(function() {
          $modalInstance.close(true);
        }, function(/*error*/) {

          $modalInstance.close(false);
        });
      } else {
        // Nothing touched so just close
        $modalInstance.close(true);
      }
    }

  };

})();
