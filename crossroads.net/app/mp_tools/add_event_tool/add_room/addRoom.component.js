(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', '$rootScope', 'AddEvent', 'Room'];

  function AddRoom($log, $rootScope, AddEvent, Room) {
    return {
      restrict: 'E',
      scope: {
        roomData: '='
      },
      templateUrl: 'add_room/add_room.html',
      controller: AddRoomController,
      controllerAs: 'addRoom',
      bindToController: true
    };

    function AddRoomController() {
      var vm = this;
      vm.choosenSite = choosenSite;
      vm.equipmentList = [];
      vm.layouts = Room.Layouts.query();
      vm.onAdd = onAdd;
      vm.roomError = false;
      vm.showNoRoomsMessage = showNoRoomsMessage;
      vm.viewReady = false;

      activate();

      //////////////////

      function activate() {
        if (AddEvent.eventData.event.congregation !== undefined) {
          Room.ByCongregation.query({
            congregationId: AddEvent.eventData.event.congregation.dp_RecordID
          }, function(data) {
            vm.rooms = data;
            vm.viewReady = true;
          });

          vm.equipmentList = Room.Equipment.query({congregationId: AddEvent.eventData.event.congregation.dp_RecordID});
          return;
        }

        $log.error('Unable to get the list of rooms... handle the error somehow!');
        return;
      }

      function choosenSite() {
        // make sure it doesn't already exist first...
        return AddEvent.eventData.event.congregation.dp_RecordName;
      }

      function onAdd() {
        if (vm.choosenRoom) {
          // is this room already added?
          var alreadyAdded = _.find(vm.roomData, function(r) {
            return r.id === vm.choosenRoom.id;
          });

          if (alreadyAdded) {
            $rootScope.$emit('notify', $rootScope.MESSAGES.allReadyAdded);
            return;
          }

          vm.roomData.push(vm.choosenRoom);
          return;
        }

        $rootScope.$emit('notify', $rootScope.MESSAGES.chooseARoom);
      }

      function showNoRoomsMessage() {
        return (!vm.viewReady || vm.rooms === undefined || vm.rooms.length < 1);
      }
    }
  }

})();
