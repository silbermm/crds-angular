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
        removeRoom: '&',
        editMode: '='
      },
      templateUrl: 'room_form/roomForm.html',
      bindToController: true,
      controller: RoomController,
      controllerAs: 'room'
    };

    function RoomController() {
      var vm = this;
      vm.existing = existing;
      vm.isCancelled = isCancelled;
      vm.remove = remove;
      vm.validation = Validation;

      activate();

      ////////////////////

      function activate() {

        if (vm.currentRoom.equipment === undefined) {
          vm.currentRoom.equipment = [];
        }
      }

      function existing() {
        return _.has(vm.currentRoom, 'cancelled');
      }

      function isCancelled() {
        return existing() && vm.currentRoom.cancelled;
      }

      function remove() {
        vm.removeRoom();
      }

    }
  }
})();
