(function() {
  'use strict()';

  module.exports = VolunteerContactController;

  VolunteerContactController.$inject = ['$rootScope', '$window', '$log', 'MPTools'];

  function VolunteerContactController($rootScope, $window, $log, MPTools) {

    $log.debug('VolunteerContactController');
    var vm = this;

    vm.cancel = cancel;
    vm.multipleRecordsSelected = true;
    vm.params = MPTools.getParams();
    vm.processing = false;
    vm.viewReady = false;

    activate();

    //////////////////////

    function activate() {
      $log.debug('pageInfo: ' + vm.pageInfo);
      vm.multipleRecordsSelected = showError();
      vm.viewReady = true;
    }

    function cancel() {
      $window.close();
    }

    function showError() {
      return vm.params.selectedCount > 1 || vm.params.recordDescription === undefined || vm.params.recordId === '-1';
    }
  }

})();
