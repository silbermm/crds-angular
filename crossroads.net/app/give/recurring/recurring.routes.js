(function() {

  'use strict';

  module.exports = RecurringRoutes;

  RecurringRoutes.$inject = ['$stateProvider'];

  /**
   * This holds all of Recurring Giving routes
   */
  function RecurringRoutes($stateProvider) {
    $stateProvider
      .state('give.recurring_account', {
        templateUrl: 'templates/account.html'
      })
      .state('give.recurring_thank-you', {
        templateUrl: 'templates/thank_you.html'
      })
      .state('give.recurring_login', {
        templateUrl: 'templates/login.html'
      });
  }

})();
