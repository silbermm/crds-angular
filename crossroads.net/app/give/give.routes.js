(function() {

  'use strict';

  module.exports = GiveRoutes;

  GiveRoutes.$inject = ['$httpProvider', '$stateProvider'];

  function GiveRoutes($httpProvider, $stateProvider) {

    $httpProvider.defaults.useXDomain = true;
    $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

    $stateProvider
      .state('give', {
        parent: 'noSideBar',
        url: '/give',
        controller: 'GiveController as give',
        templateUrl: 'give/give.html',
        resolve: {
          Programs: 'Programs',
          programList: function(Programs) {
            // TODO The number one relates to the programType in MP. At some point we should fetch
            // that number from MP based in human readable input here.
            return Programs.Programs.query({
              programType: 1
            }).$promise;
          }
        }
      })
      .state('give.amount', {
        templateUrl: 'give/amount.html'
      })
      .state('give.login', {
        controller: 'LoginCtrl',
        templateUrl: 'give/login.html'
      })
      .state('give.register', {
        controller: 'RegisterCtrl',
        templateUrl: 'give/register.html'
      })
      .state('give.confirm', {
        templateUrl: 'give/confirm.html'
      })
      .state('give.account', {
        templateUrl: 'give/account.html'
      })
      .state('give.change', {
        templateUrl: 'give/change.html'
      })
      .state('give.thank-you', {
        templateUrl: 'give/thank_you.html'
      });
  }

})();
