(function() {
  'use strict()';

  module.exports = CheckBatchProcessor;

  CheckBatchProcessor.$inject = ['$rootScope', '$log', 'MPTools', 'CheckScannerBatches', 'getPrograms', 'AuthService'];

  function CheckBatchProcessor($rootScope, $log, MPTools, CheckScannerBatches, getPrograms, AuthService) {
    var vm = this;

    vm.batch = {};
    vm.batches = [];
    vm.programs = [];
    vm.program = {};
    vm.processing = false;
    vm.params = MPTools.getParams();

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

    vm.allowAccess = function() {
      // Role #7 is "Stewardship Donation Processor" in MinistryPlatform
      return(AuthService.isAuthenticated() && AuthService.isAuthorized(7));
    }

    vm.processBatch = function() {
      vm.processing = true;

      CheckScannerBatches.save({name: vm.batch.name, programId: vm.program.ProgramId}).$promise.then(function(){
          vm.success = true;
          vm.error = false;
        }, function(){
          vm.success = false;
          vm.error=true;
      }).finally(function(){
        vm.processing = false;
      });
    };
  }
})();
