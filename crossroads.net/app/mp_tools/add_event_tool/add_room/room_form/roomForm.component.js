(function() {
  'use strict';

  module.exports = RoomForm;

  RoomForm.$inject = ['AddEvent'];

  function RoomForm(AddEvent) {
    return {
      restrict: 'E',
      scope: {
        currentRoom: '=',
        layouts: '=',
        equipmentLookup: '='
      },
      templateUrl: 'room_form/roomForm.html',
      bindToController: true,
      controller: RoomController,
      controllerAs: 'room'
    };

    function RoomController() {
      var vm = this;
      activate();

      ////////////////////

      function activate() {

        if (vm.currentRoom.equipment === undefined) {
          vm.currentRoom.equipment = [];
        }
      }

    }
  }
})();
