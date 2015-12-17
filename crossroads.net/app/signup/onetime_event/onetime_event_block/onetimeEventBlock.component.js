(function() {
  'use strict';

  module.exports = OnetimeEventBlock;

  OnetimeEventBlock.$inject = ['$rootScope', 'EventService'];

  function OnetimeEventBlock($rootScope, EventService) {
    return {
      restrict: 'E',
      replace: true,
      scope: {
        event: '=',
        family: '=',
        group: '='
      },
      bindToController: true,
      controller: OnetimeEventBlockController,
      controllerAs: 'onetimeEventBlock',
      templateUrl: 'onetime_event_block/onetimeEventBlock.html'
    };

    function OnetimeEventBlockController() {
      var vm = this;
      vm.childCareChange = childCareChange;
      vm.childcareRequested = false;
      vm.endDateTime = moment(vm.event.endDate);
      vm.endTime = endTime;
      vm.getDate = getDate;
      vm.isCollapsed = true;
      vm.saving = false;
      vm.showChildcare = showChildcare;
      vm.startDateTime = moment(vm.event.startDate);
      vm.startTime = startTime;
      vm.submit = submit;
      vm.thisFamily = angular.copy(vm.family);
      vm.togglePanel = togglePanel;

      function childCareChange(changedValue) {
        _.forEach(vm.thisFamily, function(member) {
          if (member.participantId === changedValue.participantId) {
            member.childCareNeeded = changedValue.value;
          }
        });
      }

      function endTime() {
        return vm.endDateTime.format('h:mm A');
      }

      function getDataToSave() {
        var participants = _.chain(vm.thisFamily).filter(function(member) {
          return member.selected;
        }).map(function(member) {
          return member.participantId;
        }).value();

        console.log('participants: ' + participants);

        var dto = {
          eventId: vm.event.eventId,
          groupId: vm.group.groupId,
          childCareNeeded: vm.childcareRequested,
          participants: participants
        };
        return dto;
      }

      function getDate() {
        return vm.startDateTime.format('MM/DD/YYYY');
      }

      function showChildcare(member) {
        return member.age >= 18 && vm.group.childCareInd;
      }

      function startTime() {
        return vm.startDateTime.format('h:mm A');
      }

      function submit() {
        vm.saving = true;
        var toSave = getDataToSave();

        if (_.isEmpty(toSave)) {
          vm.saving = false;
          $rootScope.$emit('notify', $rootScope.MESSAGES.chooseOne);
          return;
        }

        console.log(vm.event.eventId);

        console.log(toSave);

        // test block
        // vm.saving = false;
        // return false;

        // test block

        EventService.event.save(toSave, function(saved) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.rsvpSaved);
          vm.saving = false;
        },

        function(err) {
          $rootScope.$emit('notify', $rootScope.MESSAGES.rsvpFailed);
          vm.saving = false;
        });

      }

      function togglePanel() {
        vm.isCollapsed = !vm.isCollapsed;
      }

    }

  }

})();
