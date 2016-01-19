(function() {
  'use strict';

  module.exports = RemoveRoomController;

  RemoveRoomController.$inject = ['$modalInstance', 'items'];

  function RemoveRoomController($modalInstance, items) {

    var vm = this;
    vm.cancel = cancel;
    vm.ok = ok;
    vm.room = items;

    function ok() {
      $modalInstance.close();
    }

    function cancel() {
      $modalInstance.dismiss('cancel');
    }
  }
})();
