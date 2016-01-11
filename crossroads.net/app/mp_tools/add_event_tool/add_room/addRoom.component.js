(function() {
  'use strict';

  module.exports = AddRoom;

  AddRoom.$inject = [];

  function AddRoom() {
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

    }
  }

})();
