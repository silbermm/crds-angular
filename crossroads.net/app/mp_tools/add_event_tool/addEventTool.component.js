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
      vm.isEditMode = isEditMode;
      vm.next = next;
      vm.params = MPTools.getParams();
      vm.processing = false;
      vm.rooms = AddEvent.eventData.rooms;
      vm.submit = submit;
      vm.viewReady = false;

      activate();

      ////////////////////////////

      function activate() {
        vm.currentEventSelected = Number(vm.params.recordId);
        if (vm.currentEventSelected !== -1) {
          // tool was launched from the details view...
          AddEvent.editMode = true;
          EventService.eventTool.get({eventId: vm.currentEventSelected}, function(evt) {
            AddEvent.eventData = AddEvent.fromEventDto(evt);
            vm.event = AddEvent.eventData.event;
            vm.rooms = AddEvent.eventData.rooms;
            AddEvent.currentPage = 2;
            vm.viewReady = true;
          },

          function(err) {
            console.error('failed to get event ' + vm.currentEventSelected + ' + with error ' + err);
            vm.viewReady = true;
          });
        } else {
          vm.viewReady = true;
        }

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

      function dateTime(dateForDate, dateForTime) {
        return new Date(
            dateForDate.getFullYear(),
            dateForDate.getMonth(),
            dateForDate.getDate(),
            dateForTime.getHours(),
            dateForTime.getMinutes(),
            dateForTime.getSeconds(),
            dateForTime.getMilliseconds());
      }

      function isEditMode() {
        return AddEvent.editMode;
      }

      function next() {
        vm.allData.eventForm.$setSubmitted();

        AddEvent.eventData.event = vm.event;

        var start =  dateTime(vm.event.startDate, vm.event.startTime);
        var end = dateTime(vm.event.endDate, vm.event.endTime);

        if (moment(start) <= moment(end)) {
          $log.debug('start less');
          vm.allData.eventForm.endDate.$error.endDate = false;
          vm.allData.eventForm.endDate.$valid = true;
          vm.allData.eventForm.endDate.$invalid = false;
        } else {
          $log.debug('start less than');
          // set the endDate Invalid...
          vm.allData.eventForm.endDate.$error.endDate = true;
          vm.allData.eventForm.endDate.$valid = false;
          vm.allData.eventForm.endDate.$invalid = true;
        }

        if (vm.allData.eventForm.$valid) {

          AddEvent.currentPage = 2;
        } else {
          $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        }
      }

      function submit() {
        vm.processing = true;
        AddEvent.eventData.rooms = vm.rooms;
        if (vm.allData.roomForm) {
          vm.allData.roomForm.$setSubmitted();
          vm.allData.roomForm.equipmentForm.$setSubmitted();
        }

        if (vm.allData.$valid) {
          // build the dto...
          var event = AddEvent.getEventDto(AddEvent.eventData);
          event.startDateTime = moment(event.startDateTime).utc().format();
          event.endDateTime = moment(event.endDateTime).utc().format();

          if (AddEvent.editMode) {
            EventService.eventTool.update({eventId: vm.currentEventSelected}, event, function(result) {
              $rootScope.$emit('notify', $rootScope.MESSAGES.eventUpdateSuccess);
              AddEvent.eventData = {};
              vm.processing = false;
              $window.close();
            },

            function(err) {
              $log.error(err);
              vm.processing = false;
              $rootScope.$emit('notify', $rootScope.MESSAGES.eventToolProblemSaving);
            });
          } else {
            EventService.create.save(event, function(result) {
              $rootScope.$emit('notify', $rootScope.MESSAGES.eventSuccess);
              AddEvent.currentPage = 1;
              AddEvent.eventData = {};
              vm.rooms = [];
              vm.event = {};
              vm.processing = false;
              $window.close();
            },

            function(result) {
              $log.error(result);
              vm.processing = false;
              $rootScope.$emit('notify', $rootScope.MESSAGES.eventToolProblemSaving);
            });
          }

          return;
        }

        vm.processing = false;
        $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        console.log('form errors');
      }
    }
  }
})();
