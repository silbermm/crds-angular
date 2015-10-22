(function() {

  'use strict';

  module.exports = AdminToolRoutes;

  AdminToolRoutes.$inject = ['$httpProvider', '$stateProvider'];

  /**
   * This holds all of common AdminTool routes
   */
  function AdminToolRoutes($httpProvider, $stateProvider) {

    $httpProvider.defaults.useXDomain = true;

    //TODO: I think this is done globally, not needed here, I think the above needs to be done globally
    $httpProvider.defaults.headers.common['X-Use-The-Force'] = true;

    $stateProvider
      .state('tools.adminGivingHistoryTool', {
        // This is a "launch" page for the tool, it will check access, etc, then forward
        // on to the actual page with the history.
        url: '/adminGivingHistoryTool',
        controller: 'AdminToolController as AdminToolController',
        templateUrl: 'admin_tool_common/adminTool.html',
        resolve: {
          $state: '$state',
          GIVE_ROLES: 'GIVE_ROLES',
          GivingHistoryService: 'GivingHistoryService',
          role: function(GIVE_ROLES) {
            return GIVE_ROLES.StewardshipDonationProcessor;
          },

          goToFunction: function(GivingHistoryService, $state) {
            return function(donorId) {
              GivingHistoryService.impersonateDonorId = donorId;
              $state.go('tools.adminGivingHistory');
            };
          }
        }
      })
      .state('tools.adminGivingHistory', {
        url: '/adminGivingHistory',
        controller: 'GivingHistoryController as admin_giving_history_controller',
        templateUrl: 'admin_giving_history/adminGivingHistory.html',
        data: {
          isProtected: true,
          meta: {
            title: 'Giving History - Admin View',
            description: ''
          }
        }
      });
  }

})();
