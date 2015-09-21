(function() {
  'use strict';

  module.exports = OneTimeGiving;

  OneTimeGiving.$inject = ['GiveTransferService', 'GiveFlow', 'Session', '$state'];

  function OneTimeGiving(GiveTransferService, GiveFlow, Session, $state) {
    var service = {
      initDefaultState: initDefaultState,
    };

    function initDefaultState() {
      GiveTransferService.reset();
      GiveTransferService.processing = false;

      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.account',
        login: 'give.login',
        register: 'give.register',
        confirm: 'give.confirm',
        change: 'give.change',
        thankYou: 'give.thank-you'
      });

      GiveTransferService.initialized = true;

      Session.removeRedirectRoute();
      $state.go(GiveFlow.amount);
    }

    return service;
  }

})();
