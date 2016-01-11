(function() {
  'use strict';

  module.exports = AddEventTool;

  AddEventTool.$inject = [
    '$rootScope',
    '$window',
    '$log',
    'MPTools',
    'AuthService',
    'CRDS_TOOLS_CONSTANTS'
  ];

  function AddEventTool($rootScope, $window, $log, MPTools, AuthService, CRDS_TOOLS_CONSTANTS) {

    return {
      restrict: 'E',
      scope: {},
      controller: AddEventToolController,
      controllerAs: 'addEvent',
      bindToController: true,
      templateUrl: 'add_event_tool/add_event_tool.html'
    };

    function AddEventToolController() {
      var vm = this;

      vm.allowAccess = allowAccess;
      vm.params = MPTools.getParams();
      vm.processing = false;
      vm.viewReady = false;

      activate();

      ////////////////////////////

      function activate() {
        vm.currentEventSelected = vm.params.recordId;
        vm.viewReady = true;
      }

      function allowAccess() {
        var authenticated = AuthService.isAuthenticated();
        var authorized = AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.EventsRoomsEquipment);
        return (authenticated && authorized);
      }

    }
  }
})();
