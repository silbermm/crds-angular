(function() {
  'use strict';

  module.exports = RecurringGiving;

  RecurringGiving.$inject = ['GiveFlow'];

  function RecurringGiving(GiveFlow) {
    var service = {
      initDefaultState: initDefaultState,
    };

    function initDefaultState() {

      // Setup the give flow service
      GiveFlow.reset({
        amount: 'give.amount',
        account: 'give.recurring_account',
        login: 'give.login',
        register: 'give.register',
        confirm: 'give.recurring_confirm',
        change: 'give.recurring_change',
        thankYou: 'give.recurring_thank-you'
      });
    }

    return service;
  }

})();

