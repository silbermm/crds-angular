(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = ['AddEvent'];

  function AddRoom(AddEvent) {
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
      vm.back = back;

      function back() {
        AddEvent.currentPage = 1;
      }
    }
  }

})();
