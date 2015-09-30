(function() {
  'use strict';

  module.exports = RecurringGiving;

  RecurringGiving.$inject = ['DonationService', 'GiveFlow'];

  function RecurringGiving(DonationService, GiveFlow) {
    var service = {
      initDefaultState: initDefaultState,
      goToAccount: goToAccount,
      stateName: stateName,
      goToChange: goToChange,
      goToLogin: goToLogin,
      submitBankInfo: submitBankInfo,
      processChange: processChange,
      getLoggedInUserDonorPaymentInfo: getLoggedInUserDonorPaymentInfo,
    };

    function initDefaultState() {
      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.recurring_account',
        login: 'give.recurring_login',
        register: 'give.register',
        confirm: 'give.recurring_confirm',
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
      DonationService.submitBankInfo(give.giveForm, give.programsInput);
    }

    function processChange() {
      DonationService.processChange();
    }

    function getLoggedInUserDonorPaymentInfo(event, toState) {
    }

    return service;
  }

})();

