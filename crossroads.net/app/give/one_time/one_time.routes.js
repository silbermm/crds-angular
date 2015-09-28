(function() {

  'use strict';

  module.exports = OneTimeRoutes;

  OneTimeRoutes.$inject = ['$stateProvider'];

  /**
   * This holds all of One-Time Giving routes
   */
  function OneTimeRoutes($stateProvider) {
    $stateProvider
      .state('give.one_time_confirm', {
        templateUrl: 'templates/confirm.html'
      })
      .state('give.one_time_account', {
        templateUrl: 'templates/account.html'
      })
      .state('give.one_time_change', {
        templateUrl: 'templates/change.html'
      })
      .state('give.one_time_thank-you', {
        templateUrl: 'templates/thank_you.html'
      });
  }

})();
