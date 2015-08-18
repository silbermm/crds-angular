(function() {
  'use strict()';

  module.exports = CheckBatchProcessor;

  CheckBatchProcessor.$inject = ['$rootScope', '$log', 'MPTools', 'CheckScannerBatches', 'getPrograms'];

  function CheckBatchProcessor($rootScope, $log, MPTools, CheckScannerBatches, getPrograms) {
    var vm = this;

    vm.batch = {};
    vm.batches = [];
    vm.programs = [];
    vm.program = {};
    vm.params = MPTools.getParams();

    activate();

    function activate() {
      CheckScannerBatches.query(function(data) {
        vm.batches = data;
      });

      getPrograms.Programs.get({programType: 1}).$promise.then(function(data) {
        vm.programs = data;
      });
    }
  }
})();
