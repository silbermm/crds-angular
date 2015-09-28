(function() {

  'use strict';

  module.exports = GiveRoutes;

  GiveRoutes.$inject = ['$httpProvider', '$stateProvider'];

  /**
   * This holds all of One-Time Giving routes
   */
  function GiveRoutes($httpProvider, $stateProvider) {

    $httpProvider.defaults.useXDomain = true;

    //TODO: I think this is done globally, not needed here, I think the above needs to be done globally
    $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

    $stateProvider
      .state('give', {
        parent: 'noSideBar',
        url: '/give',
        controller: 'GiveController as give',
        templateUrl: 'giveTemplates/give.html',
        resolve: {
          Programs: 'Programs',
          programList: function(Programs) {
            // TODO The number one relates to the programType in MP. At some point we should fetch
            // that number from MP based in human readable input here.
            return Programs.Programs.query({
              programType: 1
            }).$promise;
          }
        },
        data: {
          meta: {
            title: 'Give',
            description: ''
          }
        }
      })
      .state('give.amount', {
        templateUrl: 'templates/amount.html'
      })
      .state('give.login', {
        controller: 'LoginCtrl',
        templateUrl: 'templates/login.html'
      })
      .state('give.register', {
        controller: 'RegisterCtrl',
        templateUrl: 'templates/register.html'
      });
  }

})();
