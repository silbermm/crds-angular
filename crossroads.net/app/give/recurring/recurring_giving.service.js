(function() {
  'use strict';

  module.exports = RecurringGiving;

  RecurringGiving.$inject = ['GiveTransferService', 'DonationService', 'GiveFlow', 'Session', '$state'];

  function RecurringGiving(GiveTransferService, DonationService, GiveFlow, Session, $state) {
    var service = {
      initDefaultState: initDefaultState,
      resetGiveFlow: resetGiveFlow,
      goToAccount: goToAccount,
      stateName: stateName,
      goToChange: goToChange,
      goToLogin: goToLogin,
      submitBankInfo: submitBankInfo,
      processChange: processChange,
      getLoggedInUserDonorPaymentInfo: getLoggedInUserDonorPaymentInfo,
    };

    function initDefaultState() {
      GiveTransferService.reset();
      GiveTransferService.processing = false;

      resetGiveFlow();
      GiveTransferService.initialized = true;

      Session.removeRedirectRoute();
      $state.go(GiveFlow.amount);
    }

    function resetGiveFlow() {
      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.recurring_account',
        login: 'give.recurring_login',
        register: 'give.register',
        confirm: 'give.recurring_account',
        change: 'give.recurring_change',
        thankYou: 'give.recurring_thank-you'
      });
    }

    function stateName(state) {
      return GiveFlow[state];
    }

    function goToAccount(giveForm) {
      GiveFlow.goToAccount(giveForm);
    }

    function goToChange() {
      GiveFlow.goToChange();
    }

    function goToLogin() {
      GiveFlow.goToLogin();
    }

    function submitBankInfo(giveForm, programsInput) {
      DonationService.createRecurringGift();
    }

    function processChange() {
      DonationService.processChange();
    }

    function getLoggedInUserDonorPaymentInfo(event, toState) {
    }

    return service;
  }

})();

