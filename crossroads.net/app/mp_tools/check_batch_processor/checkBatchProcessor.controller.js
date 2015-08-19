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
    vm.processing = false;
    vm.params = MPTools.getParams();

    vm.processBatch = function() {
      vm.processing = true;

      CheckScannerBatches.save({name: vm.batch.name, programId: vm.program.ProgramId},
        function(){
            vm.success = true;
            vm.error = false;
        },
        function(){
          vm.success = false;
          vm.error=true;
      }).finally(function(){ vm.processing = false; });
    };

    activate();
    //////////////////////

    function activate() {
      CheckScannerBatches.query(function(data) {
        vm.batches = data;
      });

      getPrograms.Programs.get({programType: 1}, function(data) {
        vm.programs = data;
      });
    }
  }
})();
