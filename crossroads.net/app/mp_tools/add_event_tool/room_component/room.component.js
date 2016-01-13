(function() {
  'use strict';

  module.exports = RoomComponent;

  RoomComponent.$inject = ['AddEvent'];

  function RoomComponent(AddEvent) {
    return {
      restrict: 'E',
      scope: {
        roomObj: '='
      },
      templateUrl: 'room_component/room.html',
      bindToController: true,
      controller: RoomController,
      controllerAs: 'room'
    };

    function RoomController() {
      var vm = this;

      vm.currentRoom = angular.copy(vm.room);

    }
  }
})();
