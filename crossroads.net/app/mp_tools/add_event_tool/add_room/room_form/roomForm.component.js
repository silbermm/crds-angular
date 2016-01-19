(function() {
  'use strict';

  module.exports = RoomForm;

  RoomForm.$inject = ['AddEvent', 'Validation'];

  function RoomForm(AddEvent, Validation) {
    return {
      restrict: 'E',
      scope: {
        currentRoom: '=',
        layouts: '=',
        equipmentLookup: '=',
        removeRoom: '&'
      },
      templateUrl: 'room_form/roomForm.html',
      bindToController: true,
      controller: RoomController,
      controllerAs: 'room'
    };

    function RoomController() {
      var vm = this;
      vm.remove = remove;
      vm.validation = Validation;

      activate();

      ////////////////////

      function activate() {

        if (vm.currentRoom.equipment === undefined) {
          vm.currentRoom.equipment = [];
        }
      }

      function remove() {
        vm.removeRoom();
      }

    }
  }
})();
