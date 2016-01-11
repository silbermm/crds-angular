(function() {
  'use strict';

  module.exports = EventsRoomsEquipmentController;

  EventsRoomsEquipmentController.$inject = [
    '$rootScope',
    '$window',
    '$log',
    'MPTools',
    'AuthService',
    'CRDS_TOOLS_CONSTANTS'
  ];

  function EventsRoomsEquipmentController($rootScope, $window, $log, MPTools, AuthService, CRDS_TOOLS_CONSTANTS) {
    var vm = this;

    vm.allowAccess = allowAccess;
    vm.cancel = cancel;
    vm.fieldError = fieldError;
    vm.multipleRecordsSelected = true;
    vm.params = MPTools.getParams();
    vm.processing = false;
    vm.save = save;
    vm.viewReady = false;

    activate();

    ////////////////////////////

    function activate() {
      vm.currentEventSelected = vm.params.recordId;
      vm.multipleRecordsSelected = showError();
      vm.viewReady = true;
    }

    function allowAccess() {
      return (AuthService.isAuthenticated() &&
              AuthService.isAuthorized(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.EventsRoomsEquipment));
    }

    function cancel() {
      $window.close();
    }

    function fieldError(form, field) {
      if (form[field] === undefined) {
        return false;
      }

      if (form.$submitted || form[field].$dirty) {
        return form[field].$invalid;
      }

      return false;
    }

    function save() {
      $log.debug('Save has been clicked');
    }

    function showError() {
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined || vm.params.recordId === '-1';
    }

  }

})();
