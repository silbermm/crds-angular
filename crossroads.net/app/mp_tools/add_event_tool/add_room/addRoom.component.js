(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', 'AddEvent', 'Room'];

  function AddRoom($log, AddEvent, Room) {
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
        // add the currently choosen room to the list of rooms...
        vm.roomData.push(vm.choosenRoom);
      }
    }
  }

})();
