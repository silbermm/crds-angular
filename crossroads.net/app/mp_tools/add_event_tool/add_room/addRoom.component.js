(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['$log', 'AddEvent', 'Room'];

  function AddRoom($log, AddEvent, Room) {
    return {
      restrict: 'E',
      scope: {
        locationId: '='
      },
      templateUrl: 'add_room/add_room.html',
      controller: AddRoomController,
      controllerAs: 'addRoom',
      bindToController: true
    };

    function AddRoomController() {
      var vm = this;
      vm.back = back;
      vm.rooms = {};

      activate();

      //////////////////

      function activate() {
        if (vm.locationId === undefined) {
          // data wasn't passed in... is it in the service?
          if (AddEvent.eventData.congregation !== undefined) {
            vm.rooms = Room.ByLocation.query({congregationId: AddEvent.eventData.congregation});
            return;
          }
        }

        $log.error('Unable to get the list of rooms... handle the error bitch!');
        return;
      }

      function back() {
        AddEvent.currentPage = 1;
      }
    }
  }

})();
