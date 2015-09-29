(function() {
  'use strict';

  module.exports = OneTimeGiving;

  OneTimeGiving.$inject = ['GiveTransferService', 'GiveFlow', 'Session', '$state'];

  function OneTimeGiving(GiveTransferService, GiveFlow, Session, $state) {
    var service = {
      initDefaultState: initDefaultState,
      goToAccount: goToAccount,
      stateName: stateName,
      goToChange: goToChange,
      goToLogin: goToLogin,
    };

    function initDefaultState() {
      GiveTransferService.reset();
      GiveTransferService.processing = false;

      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.one_time_account',
        login: 'give.one_time_login',
        register: 'give.register',
        confirm: 'give.one_time_confirm',
        change: 'give.one_time_change',
        thankYou: 'give.one_time_thank-you'
      });

      GiveTransferService.initialized = true;

      Session.removeRedirectRoute();
      $state.go(GiveFlow.amount);
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

    return service;
  }

})();
