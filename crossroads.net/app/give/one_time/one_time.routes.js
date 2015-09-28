(function() {

  'use strict';

  module.exports = OneTimeRoutes;

  OneTimeRoutes.$inject = ['$httpProvider', '$stateProvider'];

  /**
   * This holds all of One-Time Giving routes
   */
  function OneTimeRoutes($httpProvider, $stateProvider) {

    $httpProvider.defaults.useXDomain = true;

    //TODO: I think this is done globally, not needed here, I think the above needs to be done globally
    $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

    $stateProvider
      .state('give.one_time.confirm', {
        templateUrl: 'templates/confirm.html'
      })
      .state('give.one_time.account', {
        templateUrl: 'templates/account.html'
      })
      .state('give.one_time.change', {
        templateUrl: 'templates/change.html'
      })
      .state('give.one_time.thank-you', {
        templateUrl: 'templates/thank_you.html'
      });
  }

})();
