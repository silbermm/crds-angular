(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', 'AddEvent', 'Room'];

  function AddRoom($log, AddEvent, Room) {
    return {
      restrict: 'E',
      scope: {
      
      },
      templateUrl: 'add_room/add_room.html',
      controller: AddRoomController,
      controllerAs: 'addRoom',
      bindToController: true
    };

    function AddRoomController() {
      var vm = this;
      vm.onAdd = onAdd;
      vm.back = back;
      vm.choosenSite = choosenSite;
      vm.formData = { 
        rooms: [] 
      };
      vm.viewReady = false;

      activate();

      //////////////////

      function activate() {
        if (AddEvent.eventData.congregation !== undefined) {
          Room.ByLocation.query({congregationId: AddEvent.eventData.congregation.dp_RecordID}, function(data) {
            vm.rooms = data;
            vm.viewReady = true;
          });

          return;
        }

        $log.error('Unable to get the list of rooms... handle the error somehow!');
        return;
      }

      function onAdd() {
        // add the currently choosen room to the list of rooms...  
        vm.formData.rooms.push(vm.choosenRoom);
      }

      function back() {
        AddEvent.currentPage = 1;
      }

      function choosenSite() {
        // make sure it doesn't already exist first...
        return AddEvent.eventData.congregation.dp_RecordName;  
      }
    }
  }

})();
