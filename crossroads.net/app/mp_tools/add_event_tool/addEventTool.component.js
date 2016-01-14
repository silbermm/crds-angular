(function() {
  'use strict';

  module.exports = AddEventTool;

  AddEventTool.$inject = [
    '$rootScope',
    '$window',
    '$log',
    'MPTools',
    'AuthService',
    'EventService',
    'CRDS_TOOLS_CONSTANTS',
    'AddEvent'
  ];

  function AddEventTool($rootScope, $window, $log, MPTools, AuthService, EventService, CRDS_TOOLS_CONSTANTS, AddEvent) {

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
      vm.back = back;
      vm.currentPage = currentPage;
      vm.event = AddEvent.eventData.event;
      vm.next = next;
      vm.params = MPTools.getParams();
      vm.processing = false;
      vm.rooms = AddEvent.eventData.rooms;
      vm.submit = submit;
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

      function back() {
        AddEvent.currentPage = 1;
      }

      function currentPage() {
        return AddEvent.currentPage;
      }

      function next() {
        vm.allData.eventForm.$setSubmitted();
        // I shouldn't have to do this, but I don't have time to debug it!
        AddEvent.eventData.event = vm.event;

        if (vm.allData.eventForm.$valid) {
          AddEvent.currentPage = 2;
        } else {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        }
      }

      function submit() {
        vm.allData.roomForm.$setSubmitted();
        vm.allData.roomForm.equipmentForm.$setSubmitted();
        AddEvent.eventData.rooms = vm.rooms;
        if (vm.allData.$valid) {
          // build the dto...
          $log.debug('submit form');
          var equipment = AddEvent.getEventDto(AddEvent.eventData);
          EventService.create.save(equipment, function(result) {
            $log.debug(result);
          }, function(result) {
            $log.error(result);                
          });
          return;
        }
        
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        console.log('form errors');
      }
    }
  }
})();
