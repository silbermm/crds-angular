(function() {
  'use strict';

  module.exports = ConfirmPasswordController;

  ConfirmPasswordController.$inject = [
    '$modalInstance',
    'modalTypeItem'
  ];

  function ConfirmPasswordController(
      $modalInstance,
      modalTypeItem) {

      var vm = this;
      vm.ok = ok;
      vm.cancel = cancel;
      vm.passwd = '';
      vm.modalTypeItem = modalTypeItem;

      function ok() {
        $modalInstance.close(vm.passwd);
      }

      function cancel() {
        $modalInstance.dismiss('cancel');
      }

    }
})();
